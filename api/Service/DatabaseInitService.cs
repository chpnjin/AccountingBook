using Dapper;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;

namespace api.Service
{
    /// <summary>
    /// 資料庫初始化設定,新增系統管理員帳號
    /// </summary>
    public class DatabaseInitService : IHostedService
    {
        private readonly MySqlConnection _connection; //建立DB連線介面

        MySqlConnection connection { get {  return _connection; } }

        public DatabaseInitService(IDbConnection connection)
        {
            _connection = (MySqlConnection)connection;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await connection.OpenAsync();

            // 檢查是否存在 Admin 用戶
            string sqlStr = @"SELECT EXISTS(SELECT 1 FROM user WHERE name = 'admin')";
            var hasAdmin = await connection.ExecuteScalarAsync<bool>(sqlStr);

            //若不存在則自動新增
            if (!hasAdmin)
            {
                string sql = @"
                INSERT INTO user (name,email,role_group,password_hash,salt,degree_of_parallelism,iterations,memory_size) 
                VALUES (@name, @email,@role_group, @password_hash,@salt,@degree_of_parallelism,@iterations,@memory_size)"
                ;

                var hashedPassword = await PasswordHasher.HashPassword("111111");

                await connection.ExecuteAsync(sql, new
                {
                    name = "admin",
                    email = "admin@example.com",
                    role_group = "admin",
                    password_hash = hashedPassword.hash,
                    salt = hashedPassword.salt,
                    degree_of_parallelism = hashedPassword.degreeOfParallelism,
                    iterations = hashedPassword.iterations,
                    memory_size = hashedPassword.memorySize
                });
            }
        }

        /// <summary>
        /// 服務停止時執行流程
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
