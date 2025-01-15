using api;
using api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers().AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        // 加入JWT認證模擬，可輸入認證通過後取得的JWT
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "請輸入JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        // 設定傳送Request時標頭自動加入Bearer + JWT
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // 添加對 SQL 連線的依賴注入
    builder.Services.AddTransient<IDbConnection>((provider) =>
    {
        // 使用環境變數獲取連接字符串
        var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
        Console.Write(connectionString);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("SQL_CONNECTION_STRING environment variable is not set.");
        }
        return new MySqlConnection(connectionString);
    });

    // 註冊資料庫初始化服務，在應用啟動時會自動執行
    builder.Services.AddHostedService<DatabaseInitService>();

    // 添加JWT認證服務添加到依賴注入容器
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                //確保令牌的發行者（issuer）宣告與預期的發行者匹配。這表示令牌必須來自一個可信任的發行者。
                ValidateIssuer = true,
                //檢查令牌的對象（audience）宣告是否與預期的對象匹配。用來確保令牌是針對正確的接收者。
                ValidateAudience = true,
                //驗證令牌的有效期，確認它是否已經過期或是尚未生效，根據其 nbf（not before）和 exp（expiration）宣告。
                ValidateLifetime = true,
                //確認令牌的簽名可以用發行者的簽名鍵驗證。這對於確認令牌未被篡改至關重要。
                ValidateIssuerSigningKey = true,
                //設定有效的發行者，令牌必須來自這個發行者。
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                //指定令牌有效的對象。與 ValidIssuer 類似，此值也是從配置中獲取。
                ValidAudience = builder.Configuration["Jwt:Audience"],
                //配置用於簽署令牌的密鑰。
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

    // 新增統一錯誤處裡
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    var app = builder.Build();

    //開發環境專用
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    var settings = builder.Configuration["ALLOWED_ORIGINS"];
    var allowedOrigins = new List<string>();

    if (settings == null)
    {
        allowedOrigins.AddRange(["http://localhost:5173/", "http://chpngen920.synology.me:5005"]);
    }
    else
    {
        allowedOrigins.AddRange(settings.Split(','));
    }

    app.UseCors(builder => builder
    .WithOrigins(allowedOrigins.ToArray())
    .AllowAnyMethod()
    .WithHeaders("Content-Type", "Authorization", "Accept")
    .AllowCredentials()); // 如果需要傳送 cookie 或認證信息

    app.UseRouting();

    // 添加用於認證和授權的中間件
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();  //一定要加,否則API呼叫會回傳404

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred: " + ex.Message);
    Console.WriteLine(ex.StackTrace);
}