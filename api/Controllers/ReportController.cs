using api.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace api.Controllers
{
    /// <summary>
    /// 統計報表相關資料
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly MySqlConnection conn;

        public ReportController(IDbConnection connection)
        {
            conn = (MySqlConnection)connection;
            conn.Open();
        }

        /// <summary>
        /// 取得科目餘額
        /// </summary>
        /// <param name="accountType">科目類型</param>
        /// <returns></returns>
        [HttpGet("GetAccountBalanceList")]
        public async Task<ActionResult> GetAccountBalanceList(string accountType)
        {
            try
            {
                string sqlStr;
                //資產,支出:借方增加,貸方減少
                if (new string[] { "Asset", "Expense" }.Contains(accountType))
                {
                    sqlStr = @"SELECT b.name,SUM(a.debit_amount) - SUM(a.credit_amount) AS balance
                                  FROM voucher_detail a
                                  INNER JOIN account b ON a.account_id = b.id
                                  WHERE b.type = @type
                                  GROUP BY a.account_id
                                  ;";
                }
                //負債,權益,收入:借方減少,貸方增加
                else if (new string[] { "Liability", "Equity", "Revenue" }.Contains(accountType))
                {
                    sqlStr = @"SELECT b.name,SUM(a.credit_amount) - SUM(a.debit_amount) AS balance
                                  FROM voucher_detail a
                                  INNER JOIN account b ON a.account_id = b.id
                                  WHERE b.type = @type
                                  GROUP BY a.account_id
                                  ;";
                }
                else
                {
                    //防亂輸入例外
                    throw new Exception("accountType not found");
                }

                var result = await conn.QueryAsync<AccountBalance>(sqlStr, new { type = accountType });

                if (result.Count() > 0)
                {
                    return Ok(result.ToList());
                }
                else
                {
                    return Ok(new List<AccountBalance>());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
