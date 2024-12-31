using api.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public AccountController(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// 取得所有主科目
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetMainAccounts")]
        public async Task<ActionResult> GetMainAccounts()
        {
            try
            {
                string sql = @"SELECT id,no,name,type,description,active 
                             FROM accounts
                            WHERE main_id IS NULL";

                var data = await _connection.QueryAsync<dynamic>(sql);
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
        [Authorize]
        [HttpPost("GetSubAccounts")]
        public async Task<ActionResult> GetSubAccounts([FromBody] GetSubAccountInput parms)
        {
            try
            {
                string sql = @"SELECT id,no,name,type,description,active 
                             FROM accounts
                            WHERE main_id = @main_id "
                ;

                var data = await _connection.QueryAsync<dynamic>(sql, new { @main_id = parms.main_id });
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("CheckAccountExist")]
        public async Task<ActionResult> CheckAccountExist(string accountNo)
        {
            try
            {
                string sql = @"SELECT COUNT(no) FROM accounts WHERE no = @accountNo";

                var data = await _connection.QueryAsync<int>(sql, new { accountNo });

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
        [Authorize]
        [HttpPost("Edit")]
        public async Task<ActionResult> EditAccount(Account parms)
        {
            try
            {
                string sqlStr = "";

                if (parms.id is null)
                {
                    sqlStr = @"INSERT INTO accounts (no, name, type, main_id, description)
                    VALUES (@no, @name, @type, @main_id, @description)
                    ";
                }
                else
                {
                    sqlStr = @"UPDATE accounts
                                  SET no = @no,
                                    name = @name,
                                   type = @type,
                                main_id = @main_id,
                            description = @description,
                                 active = TRUE,
                      last_updated_time = CURRENT_TIMESTAMP() 
                               WHERE id = @id";
                }

                await _connection.ExecuteAsync(sqlStr, parms);

                return Ok("Done");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
