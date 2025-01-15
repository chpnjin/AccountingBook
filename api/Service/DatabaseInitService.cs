using Dapper;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace api.Service
{
    /// <summary>
    /// 資料庫初始化設定,新增系統管理員帳號
    /// </summary>
    public class DatabaseInitService : IHostedService
    {
        private readonly MySqlConnection _connection; //建立DB連線介面

        MySqlConnection connection { get { return _connection; } }

        public DatabaseInitService(IDbConnection connection)
        {
            _connection = (MySqlConnection)connection;
            Console.WriteLine($"連線資料庫:{connection.ConnectionString}");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await connection.OpenAsync(cancellationToken);
            await CheckAndCreateTableAsync();

            Console.WriteLine("開始檢查及建立系統管理員帳號...");
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
                            userId,
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

        private async Task CheckAndCreateTableAsync()
        {
            string[] tables = ["user", "account", "voucher"];
            Console.WriteLine("開始檢查及建立資料庫表格...");

            try
            {
                foreach (var tableName in tables)
                {
                    string checkTableQuery = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = '{tableName}'";
                    int count = await connection.ExecuteScalarAsync<int>(checkTableQuery);

                    if (count == 0)
                    {
                        Console.WriteLine($"資料表 '{tableName}' 不存在，正在從 '{tableName}.sql' 建立...");
                        string sqlFilePath = "#DB/" + tableName + ".sql";

                        if (!File.Exists(sqlFilePath))
                        {
                            throw new FileNotFoundException($"找不到 SQL 檔案：{sqlFilePath}");
                        }

                        string sqlScript = await File.ReadAllTextAsync(sqlFilePath);
                        var response = await connection.ExecuteAsync(sqlScript);
                        Console.WriteLine($"資料表 '{tableName}' 建立完成。");
                    }
                    else
                    {
                        Console.WriteLine($"資料表 '{tableName}' 已存在。");
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"發生 MySQL 錯誤：{ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"檔案錯誤：{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤：{ex.Message}");
            }
        }

        /// <summary>
        /// 服務停止時執行流程
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await connection.CloseAsync();
            throw new NotImplementedException();
        }
    }
}
