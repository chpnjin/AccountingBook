using api.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Data;

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
            string? lastOneNo = conn.QuerySingleOrDefault<string>(getLastOneNo,null, transaction);
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

        [HttpPost("List")]
        public async Task<ActionResult> List(SearchCondition condition)
        {
            DynamicParameters parms = new DynamicParameters();

            try
            {
                string sql = @"SELECT no,entry_date,summary,handler,reviewer,status FROM voucher a ";
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

                if(!condition.summary.IsNullOrEmpty())
                {
                    conditions += condition.date_end.IsNullOrEmpty() ? "WHERE " : "AND ";
                    conditions += "summary LIKE @summary ";
                    parms.Add("@summary", "%" + condition.summary + "%");
                }

                if(condition.account_list.Where(x=> x > 0).Any())
                {
                    conditions += condition.summary.IsNullOrEmpty() ? "WHERE " : "AND ";
                    conditions += "b.account_id IN (@account_list)";
                    parms.Add("@account_id", string.Join(",", condition.account_list));
                }

                sql += conditions;

                if(condition.account_list.Where(x => x > 0).Any())
                {
                    conditions += "INNER JOIN voucher_detail b ON a.no = b.no ";
                }

                sql += "ORDER BY create_time DESC";

                var data = await conn.QueryAsync<Voucher>(sql, parms);

                if(data == null)
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
                detailSql = @"SELECT seq,account_id,
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

                if (master != null) {
                    result.master = master;
                }
                if (detail != null) {
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
    }
}
