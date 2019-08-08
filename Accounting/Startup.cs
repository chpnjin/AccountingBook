using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Accounting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //引用預設起始頁面與靜態檔案(*.html,*.css,*.js)
            app.UseDefaultFiles();
            app.UseStaticFiles();

            //強制使用HTTPS協定
            //app.UseHttpsRedirection();

            //定義開發環境偵錯頁面
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //HTTP請求第一層Middleware
            app.Use(async (context, next) =>
            {
                //將請求傳至下一層(因只有一層,呼叫next函式將使需求實際傳給Server)
                await next();

                //依照Server端回應狀態判斷是否更改URL
                if (context.Response.StatusCode == 404 &&                       // 該資源不存在
                    !System.IO.Path.HasExtension(context.Request.Path.Value) && // 網址最後沒有帶副檔名(非要求檔案)
                    !context.Request.Path.Value.StartsWith("/api"))             // 網址不是 /api 開頭(非呼叫Web API)
                {
                    // URL轉址:將網址改成 /index.html
                    //var options = new RewriteOptions();
                    //options.AddRewrite(".", "/index.html", skipRemainingRules: true);
                    //options.AddRedirect(".", "/index.html", 200);
                    //app.UseRewriter(options);
                    //context.Response.StatusCode = 200;

                    context.Request.Path = "/index.html";                       // 將網址改成 /index.html
                    context.Response.StatusCode = 200;                          // 並將 HTTP 狀態碼修改為 200 成功
                    await next();
                }
            });

            //定義全域路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}",
                    defaults: new { controller = "Main", action = "Get" });
            });
        }
    }
}
