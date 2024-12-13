using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace api.Controllers
{
    /// <summary>
    /// 呼叫測試
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Route("API_test")]
        public async Task<ActionResult<string>> API_test()
        {
            await Task.Delay(100);
            return "TEST OK";
        }

        [HttpGet]
        [Route("GetData")]
        public async Task<ActionResult<User>> GetUser()
        {
            await Task.Delay(100);
            return new User()
            {
                Id = 10,
                Name = "Winner"
            };
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
