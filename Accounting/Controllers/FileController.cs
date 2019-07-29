using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Controllers
{
    [Route("api/[controller]")] //設定個別路由名稱
    public class FileController : Controller
    {
        [HttpGet("GetVueComponent/{fileName}")] //前端呼叫url="api/GetVueComponent/{檔名}"
        public async Task<IActionResult> Get(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            if (Path.GetExtension(fileName) != ".vue")
            {
                throw new ArgumentException("request content is not a Vue Component");
            }

            // 創建檔案路徑與讀取檔案暫存容器
            var path = $@"wwwroot\pages\{fileName}";
            var memoryStream = new MemoryStream();

            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            // 回傳檔案到 Client 需要附上 Content Type，否則瀏覽器會解析失敗。
            return new FileStreamResult(memoryStream, "text/plain");
        }

    }
}
