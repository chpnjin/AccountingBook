using api.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace api.Controllers
{
    /// <summary>
    /// 會計傳票資料操作
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VoucherController : ControllerBase
    {
        private readonly MySqlConnection conn;

        public VoucherController(IDbConnection connection)
        {
            conn = (MySqlConnection)connection;
            conn.Open();
        }

        /// <summary>
        /// 取得最大傳票編號
        /// </summary>
        /// <returns></returns>
        string GenerateNewVoucherNo(MySqlTransaction transaction)
        {
            string getLastOneNo = @"SELECT no FROM voucher ORDER BY create_time DESC LIMIT 1";
            string? lastOneNo = conn.QuerySingleOrDefault<string>(getLastOneNo, null, transaction);
            DateTime now = DateTime.Now;
            string newNo = "";
            string yyyyMM = now.ToString("yyyyMM");

            //生成新編號
            if (lastOneNo == null)
            {
                newNo = yyyyMM + "001";
            }
            else //取末三碼再更新
            {
                //判斷是否為同一天
                string noOfMonYear = lastOneNo.Substring(0, 6);
                if (noOfMonYear == yyyyMM)
                {
                    string lastThreeChars = lastOneNo.Substring(lastOneNo.Length - 3);
                    newNo = yyyyMM + (int.Parse(lastThreeChars) + 1).ToString("000");
                }
                else
                {
                    newNo = yyyyMM + "001";
                }
            }

            return newNo;
        }

        /// <summary>
        /// 傳票總攬
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpPost("List")]
        public async Task<ActionResult> List(SearchCondition condition)
        {
            DynamicParameters parms = new DynamicParameters();

            try
            {
                string sql = @"SELECT a.no,entry_date,SUM(credit_amount) AS amount,a.summary,handler,reviewer,status FROM voucher a ";
                string conditions = "";

                if (!condition.date_start.IsNullOrEmpty())
                {
                    conditions += "WHERE entry_date >= @date_start ";
                    parms.Add("@date_start", condition.date_start);
                }

                if (!condition.date_end.IsNullOrEmpty())
                {
                    conditions += condition.date_start.IsNullOrEmpty() ? "WHERE " : "AND ";
                    conditions += "entry_date <= @date_end ";
                    parms.Add("@date_end", condition.date_end);
                }

                if (!condition.summary.IsNullOrEmpty())
                {
                    conditions += condition.date_end.IsNullOrEmpty() ? "WHERE " : "AND ";
                    conditions += "a.summary LIKE @summary ";
                    parms.Add("@summary", "%" + condition.summary + "%");
                }

                if (condition.account_list.Where(x => x > 0).Any())
                {
                    conditions += condition.summary.IsNullOrEmpty() ? "WHERE " : "AND ";
                    conditions += "b.account_id IN (@account_list)";
                    parms.Add("@account_id", string.Join(",", condition.account_list));
                }

                sql += "INNER JOIN voucher_detail b ON a.no = b.no ";
                sql += conditions;

                sql += "GROUP BY b.no ";

                var data = await conn.QueryAsync<Voucher>(sql, parms);

                if (data == null)
                {
                    return Ok(new List<Voucher>());
                }
                else
                {
                    return Ok(data.ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 取得傳票明細
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDetail")]
        public async Task<ActionResult> GetDetail(string no)
        {
            string masterSql, detailSql;
            VoucherInput result = new VoucherInput();

            try
            {
                masterSql = @"SELECT no,entry_date,voucher_type,summary,handler,reviewer,status FROM voucher WHERE no = @no";
                detailSql = @"SELECT account_id,
                                b.no AS account_no,
                                b.name AS account_name,
                                summary,
                                debit_amount,
                                credit_amount 
                                FROM voucher_detail a
                                LEFT JOIN account b ON a.account_id = b.id
                                WHERE a.no = @no
                                ORDER BY seq";

                var master = conn.Query<Voucher>(masterSql, new { no }).FirstOrDefault();
                var detail = conn.Query<VoucherDetail>(detailSql, new { no }).ToList();

                if (master != null)
                {
                    result.master = master;
                }
                if (detail != null)
                {
                    result.detail = detail;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 編輯
        /// </summary>
        /// <returns></returns>
        [HttpPost("Edit")]
        public async Task<ActionResult> Edit([FromBody] VoucherInput input)
        {
            string masterSql, detailSql;
            bool isNew = false;

            using (var transaction = await conn.BeginTransactionAsync())
            {
                try
                {
                    //單頭
                    if (input.master.no == "")
                    {
                        input.master.no = GenerateNewVoucherNo(transaction);
                        masterSql = @"INSERT INTO voucher 
                            (no,entry_date,voucher_type,summary,handler,status,create_user) 
                            VALUE 
                            (@no, @entry_date, @voucher_type, @summary, @handler, @status, @handler )";

                    }
                    else
                    {
                        masterSql = @"UPDATE voucher 
                                         SET status = @Status, 
                                             entry_date = @entry_date,
                                             voucher_type = @voucher_type,
                                             summary = @summary,
                                             handler = @handler,
                                             status = @status,
                                             last_update_user = @handler, 
                                             last_update_time = CURRENT_TIMESTAMP 
                                       WHERE no = @no";
                        //更新刪除明細後重加
                        await conn.ExecuteAsync("DELETE FROM voucher_detail WHERE no = @no ", new
                        {
                            input.master.no
                        }, transaction);
                    }

                    await conn.ExecuteAsync(masterSql, input.master, transaction);

                    //單身
                    detailSql = @"INSERT INTO voucher_detail 
                                  (no, seq, account_id, summary, debit_amount, credit_amount)
                                  VALUES 
                                  (@no, @seq, @account_id, @summary, @debit_amount, @credit_amount)";

                    int seq = 1;
                    foreach (var item in input.detail)
                    {
                        item.no = input.master.no;
                        item.seq = seq;
                        await conn.ExecuteAsync(detailSql, item, transaction);
                        seq++;
                    }

                    await transaction.CommitAsync();
                    return Ok("Done");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// 複製傳票摘要與所有科目做為新增時基礎資料,節省新增科目作業
        /// </summary>
        /// <param name="voucherNo">傳票編號</param>
        /// <returns></returns>
        [HttpGet("Copy")]
        public async Task<ActionResult> Copy(string voucherNo)
        {
            VoucherInput result = new VoucherInput();
            string masterSql, detailSql;

            try
            {
                masterSql = @"SELECT voucher_type,summary FROM voucher WHERE no = @voucherNo";
                detailSql = @"SELECT account_id,
                                b.no AS account_no,
                                b.name AS account_name,
                                summary 
                                FROM voucher_detail a
                                LEFT JOIN account b ON a.account_id = b.id
                                WHERE a.no = @voucherNo
                                ORDER BY seq";

                var master = conn.Query<Voucher>(masterSql, new { voucherNo }).FirstOrDefault();
                var detail = conn.Query<VoucherDetail>(detailSql, new { voucherNo }).ToList();

                if (master != null)
                {
                    result.master = master;
                }
                if (detail != null)
                {
                    result.detail = detail;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 取得可結帳年份
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCanClosingYears")]
        public async Task<ActionResult> GetCanClosingYears()
        {
            string sql;

            try
            {
                sql = @"SELECT DISTINCT entry_year FROM voucher";

                var canCloseYears = await conn.QueryAsync<int>(sql);
                return Ok(canCloseYears);

            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "發生錯誤",
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }

        /// <summary>
        /// 取得指定結算年度的虛帳戶餘額清單
        /// </summary>
        /// <param name="year">指定年度</param>
        /// <returns></returns>
        [HttpGet("GetNominalAccountBalances")]
        public async Task<ActionResult> GetNominalAccountBalances(string year)
        {
            string sql;

            try
            {
                sql = @"
/* 第一組：支出 Expense (借方 - 貸方 = 支出淨額) */
SELECT b.id,b.name,SUM(a.debit_amount - a.credit_amount) AS 'balance'
FROM voucher_detail a
INNER JOIN account b ON a.account_id = b.id
INNER JOIN voucher c ON a.no = c.no
WHERE c.entry_date BETWEEN @StartDate AND @EndDate
AND b.type = 'Expense' -- 指定科目類型:支出
GROUP BY b.no
;

/* 第二組：收入 Revenue (貸方 - 借方 = 收入淨額) */
SELECT b.id,b.name,SUM(a.credit_amount - a.debit_amount) AS 'balance'
FROM voucher_detail a
INNER JOIN account b ON a.account_id = b.id
INNER JOIN voucher c ON a.no = c.no
WHERE c.entry_date BETWEEN @StartDate AND @EndDate
AND b.type = 'Revenue' -- 指定科目類型:收入
GROUP BY b.no
;

";

                var multi = await conn.QueryMultipleAsync(sql, new
                {
                    StartDate = $"{year}-01-01",
                    EndDate = $"{year}-12-31"
                });

                // 讀取清單 (轉為 List 方便後續計算)
                var expenseList = (await multi.ReadAsync<dynamic>()).ToList();
                var revenueList = (await multi.ReadAsync<dynamic>()).ToList();

                // 在後端順便算出總額
                decimal totalExpense = expenseList.Sum(x => (decimal)x.balance);
                decimal totalRevenue = revenueList.Sum(x => (decimal)x.balance);

                // 封裝成一個包含兩個屬性的匿名物件回傳
                var response = new
                {
                    Expense = expenseList,
                    Revenue = revenueList,
                    Summary = new
                    {
                        TotalExpense = totalExpense,
                        TotalRevenue = totalRevenue,
                        NetIncome = totalRevenue - totalExpense // 損益 = 收入 - 支出
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "計算餘額時發生錯誤",
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }

        /// <summary>
        /// 結轉虛帳戶至權益科目
        /// </summary>
        /// <param name="year">結算年分</param>
        /// <param name="equityAccountId">目標科目</param>
        /// <returns></returns>
        [HttpPost("Closing")]
        public async Task<ActionResult> Closing([FromQuery] string year, [FromQuery] string equityAccountId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.UserData)?.Value;
            string sqlStr;
            int seq = 1;

            //設定傳票編號
            string newClosingNo = "CLOSING_" + year;

            List<VoucherDetail> details = new List<VoucherDetail>();
            Voucher master = new Voucher()
            {
                no = newClosingNo,
                entry_date = DateTime.Parse($"{year}-12-31"),
                voucher_type = "closing",
                summary = $"{year} 年 結轉傳票",
                status = "unapproved",
                handler = int.Parse(userIdClaim)
            };

            try
            {
                //1.取得結算結果
                dynamic data = await GetNominalAccountBalances(year);

                //支出
                foreach (var item in data.Value.Expense)
                {
                    details.Add(new VoucherDetail
                    {
                        no = newClosingNo,
                        seq = seq,
                        account_id = item.id,
                        summary = $"科目:{item.name} 結轉",
                        debit_amount = 0,
                        credit_amount = Convert.ToDecimal(item.balance)
                    });

                    seq++;
                }

                //收入
                foreach (var item in data.Value.Revenue)
                {
                    details.Add(new VoucherDetail
                    {
                        no = newClosingNo,
                        seq = seq,
                        account_id = item.id,
                        summary = $"科目:{item.name} 結轉",
                        debit_amount = Convert.ToDecimal(item.balance), 
                        credit_amount = 0 
                    });

                    seq++;
                }


                //淨值平衡
                decimal netIncome = Convert.ToDecimal(data.Value.Summary.NetIncome);

                details.Add(new VoucherDetail
                {
                    no = newClosingNo,
                    seq = seq,
                    account_id = Convert.ToInt32(equityAccountId),
                    summary = $"{year}年度 本期損益結轉",
                    // 賠錢 (Net < 0) -> 借方增加 (代表權益減少)
                    debit_amount = netIncome < 0 ? Math.Abs(netIncome) : 0,
                    // 賺錢 (Net > 0) -> 貸方增加 (代表權益增加)
                    credit_amount = netIncome > 0 ? netIncome : 0
                });

                //平衡檢查
                if (details.Sum(x => x.debit_amount) != details.Sum(x => x.credit_amount))
                {
                    throw new Exception($"結帳傳票借貸不平衡！借方:{details.Sum(x => x.debit_amount)}, 貸方:{details.Sum(x => x.credit_amount)}");
                }

                //生成SQL
                string sqlVoucher = @"
INSERT INTO voucher (
    no, 
    entry_date, 
    voucher_type, 
    summary, 
    handler, 
    status, 
    create_user, 
    create_time
) VALUES (
    @no, 
    @entry_date, 
    'closing', 
    @summary, 
    @handler, 
    'approved', 
    @handler, 
    NOW()
);";

                string sqlDetail = @"
INSERT INTO voucher_detail (
    no, 
    seq, 
    account_id, 
    summary, 
    debit_amount, 
    credit_amount
) VALUES (
    @no, 
    @seq, 
    @account_id, 
    @summary, 
    @debit_amount, 
    @credit_amount
);";

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 寫入主檔
                        await conn.ExecuteAsync(sqlVoucher, master, transaction);

                        // 寫入明細 (Dapper 自動批次處理)
                        await conn.ExecuteAsync(sqlDetail, details, transaction);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "發生錯誤",
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }
    }
}
