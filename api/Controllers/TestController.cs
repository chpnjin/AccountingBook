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
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("GetFromDB")]
        public async Task<ActionResult<string>> Get_From_DB()
        {
            await Task.Delay(100);
            return "TEST OK";
        }
    }
}
