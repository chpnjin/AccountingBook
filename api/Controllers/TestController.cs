using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;

namespace api.Controllers
{
    /// <summary>
    /// 呼叫測試
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public TestController(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// 需認證API測試,若無JWT認證會回傳401 Error
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("NeedAuthorizeTest")]
        public async Task<ActionResult<string>> AuthorizeTest()
        {

            try
            {
                string sql = @"SELECT 'TEST OK'FROM user ";

                _connection.Open();
                var test = await _connection.QuerySingleOrDefaultAsync<string>(sql);

                return "TEST OK";
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 不須認證對照組
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("NoNeedAuthorizeTest")]
        public async Task<ActionResult<string>> NoNeedAuthorizeTest()
        {

            try
            {
                string sql = @"SELECT 'TEST OK'FROM user ";

                _connection.Open();
                var test = await _connection.QuerySingleOrDefaultAsync<string>(sql);

                return "TEST OK";
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
