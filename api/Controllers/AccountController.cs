using api.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
using System.Data;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly MySqlConnection conn;

        public AccountController(IDbConnection connection)
        {
            conn = (MySqlConnection)connection;
            conn.Open();
        }

        /// <summary>
        /// 取得所有主科目
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetMainAccounts")]
        public async Task<ActionResult> GetMainAccounts()
        {
            try
            {
                string sql = @"SELECT id,no,name,type,description,active 
                                 FROM account
                                WHERE main_id IS NULL";

                var data = await conn.QueryAsync<dynamic>(sql);
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 取得特定主科目底下的所有子科目
        /// </summary>
        /// <param name="mainId">主科目ID</param>
        /// <returns></returns>
        [HttpPost("GetSubAccounts")]
        public async Task<ActionResult> GetSubAccounts([FromBody] GetSubAccountInput parms)
        {
            try
            {
                string sql = @"SELECT id,no,name,type,description,active 
                             FROM account
                            WHERE main_id = @main_id "
                ;

                var data = await conn.QueryAsync<dynamic>(sql, new { @main_id = parms.main_id });
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 檢查科目編號是否已存在
        /// </summary>
        /// <param name="accountNo"></param>
        /// <returns></returns>
        [HttpGet("CheckAccountExist")]
        public async Task<ActionResult> CheckAccountExist(string accountNo)
        {
            try
            {
                string sql = @"SELECT COUNT(no) FROM account WHERE no = @accountNo";

                var data = await conn.QueryAsync<int>(sql, new { accountNo });

                var result = data.FirstOrDefault() > 0 ? true : false;

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 編輯科目
        /// </summary>
        /// <returns></returns>
        [HttpPost("Edit")]
        public async Task<ActionResult> EditAccount(Account parms)
        {
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    string sqlStr = "";

                    if (parms.id is null)
                    {
                        sqlStr = @"INSERT INTO account (no, name, type, main_id, description)
                                               VALUES (@no, @name, @type, @main_id, @description)";
                    }
                    else
                    {
                        sqlStr = @"UPDATE account
                                  SET no = @no,
                                    name = @name,
                                   type = @type,
                                main_id = @main_id,
                            description = @description,
                                 active = TRUE,
                      last_updated_time = CURRENT_TIMESTAMP() 
                               WHERE id = @id";
                    }

                    await conn.ExecuteAsync(sqlStr, parms, tran);

                    //若更新的是主科目，一併更新底下所有子科目的類型
                    if (parms.main_id is null)
                    {
                        sqlStr = @"UPDATE account SET type = @type WHERE main_id = @id";
                        await conn.ExecuteAsync(sqlStr, parms, tran);
                    }

                    await tran.CommitAsync();
                    return Ok("Done");
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
