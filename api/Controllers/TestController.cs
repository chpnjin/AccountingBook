using Dapper;
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

        [HttpGet]
        [Route("GetFromDB")]
        public async Task<ActionResult<string>> Get_From_DB()
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
