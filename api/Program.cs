using Microsoft.OpenApi.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    //新增Swagger支援
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    //開發環境專用
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();
    app.MapControllers();  //一定要加,否則API呼叫會回傳404

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred: " + ex.Message);
    Console.WriteLine(ex.StackTrace);
}