using api.Service;
using MySqlConnector;
using System.Data;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    //新增Swagger支援
    builder.Services.AddSwaggerGen();

    // 添加對 SQL 連線的依賴注入
    builder.Services.AddTransient<IDbConnection>((provider) =>
    {
        // 使用環境變數獲取連接字符串
        var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("SQL_CONNECTION_STRING environment variable is not set.");
        }
        return new MySqlConnection(connectionString);
    });

    // 註冊你的初始化服務，這將在應用啟動時自動運行
    builder.Services.AddHostedService<DatabaseInitService>();

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