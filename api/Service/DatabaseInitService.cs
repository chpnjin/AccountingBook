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
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        string userSql = @"
                INSERT INTO user (name, email, role_group, password_hash, salt, degree_of_parallelism, iterations, memory_size) 
                VALUES (@name, @email, @role_group, @password_hash, @salt, @degree_of_parallelism, @iterations, @memory_size);
                SELECT CAST(LAST_INSERT_ID() AS INT);";

                        var hashedPassword = await PasswordHasher.HashPassword("111111");

                        // 插入到 user 表，並獲取自動生成的 ID
                        var userId = await connection.ExecuteScalarAsync<int>(userSql, new
                        {
                            name = "admin",
                            email = "admin@example.com",
                            role_group = "admin",
                            password_hash = hashedPassword.hash,
                            salt = hashedPassword.salt,
                            degree_of_parallelism = hashedPassword.degreeOfParallelism,
                            iterations = hashedPassword.iterations,
                            memory_size = hashedPassword.memorySize
                        }, transaction);

                        // 插入到 user_info 表
                        string userInfoSql = @"
                            INSERT INTO user_info (id, full_name) 
                            VALUES (@userId, @full_name);";

                        await connection.ExecuteAsync(userInfoSql, new
                        {
                            userId = userId,
                            full_name = "系統管理員"
                        }, transaction);

                        //完成交易
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
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
