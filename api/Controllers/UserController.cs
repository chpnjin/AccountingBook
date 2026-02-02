using api.Models;
using api.Service;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MySqlConnection conn;

        public UserController(IDbConnection connection)
        {
            conn = (MySqlConnection)connection;
            conn.Open();
        }

        /// <summary>
        /// 生成API訪問許可令牌
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GenerateToken(int Id, bool isAdmin)
        {
            // 從環境變數中獲取配置
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT 基本設定未配置");
            }

            if (Encoding.UTF8.GetBytes(key).Length * 8 < 256)
            {
                throw new InvalidOperationException("JWT Key must be at least 256 bits long.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //設置claims
            var claims = new[]
            {
                new Claim(ClaimTypes.UserData,Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            DateTime expires;
            if (isAdmin)
            {
                expires = DateTime.UtcNow.AddYears(99); //管理員令牌無時限
            }
            else
            {
                expires = DateTime.UtcNow.AddMinutes(30); //令牌有效期設置為30分鐘
            }

            //生成Token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }

        /// <summary>
        /// 生成隨機密碼
        /// </summary>
        /// <param name="length">字元長度</param>
        /// <returns></returns>
        public static string GenerateSecurePassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+=-";

            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[length];
                rng.GetBytes(bytes);
                var result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    result[i] = chars[bytes[i] % chars.Length];
                }

                return new string(result);
            }
        }

        /// <summary>
        /// 送出E-mail
        /// </summary>
        /// <returns></returns>
        public static bool SendActionEmail(string to,string from,string name, string password)
        {
            var message = new MailMessage();
            message.To.Add(to);
            message.Subject = "帳號啟用通知";
            message.Body = "初始化密碼";
            message.From = new MailAddress(from);
            message.IsBodyHtml = true; // 設定郵件內容為 HTML 格式

            var smtpClient = new SmtpClient("smtp.gmail.com"); // 使用 Gmail 的 SMTP 伺服器
            smtpClient.Port = 587; // Gmail 的 SMTP 埠號
            smtpClient.Credentials = new NetworkCredential(from, password);
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("郵件寄送失敗：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 登入認證,取得Token令牌
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("LoginVerification")]
        public async Task<ActionResult> LoginVerification(LoginVerificationInput input)
        {
            try
            {
                string sql = @"SELECT t1.id,t2.full_name,status,
                                 password_hash,
                                 salt,
                                 degree_of_parallelism,
                                 iterations,
                                 memory_size
                               FROM user t1
                              INNER JOIN user_info t2 ON t1.id = t2.id
                              WHERE name = @parm_name OR email = @parm_email";

                var user = await conn.QuerySingleOrDefaultAsync<UserVerification>(sql, new
                {
                    parm_name = input.id,
                    parm_email = input.id,
                });

                if (user == null)
                {
                    return Unauthorized("User Not Found");
                }

                //密碼驗證
                var isPasswordValid = await PasswordHasher.VerifyPassword(input.password,
                user.password_hash,
                user.salt,
                user.degree_of_parallelism,
                user.iterations,
                user.memory_size);

                if (!isPasswordValid)
                {
                    return Unauthorized("Invalid Password");
                }

                bool isAdmin = user.full_name == "系統管理員" ? true : false;
                //生成Token
                string token = GenerateToken(user.id, isAdmin);

                return Ok(new
                {
                    token,
                    user.id,
                    user.full_name
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("CheckUserExist")]
        public async Task<ActionResult> CheckUserExist(string name, string email)
        {
            bool nameExist = false;
            bool emailExist = false;

            try
            {
                string sqlCheckName = @"SELECT COUNT(id) FROM user WHERE name = @name ";
                string sqlCheckEmail = @"SELECT COUNT(id) FROM user WHERE email = @email ";

                var countOfName = conn.QueryFirstOrDefault<int>(sqlCheckName, new { name });
                var countOfEmail = conn.QueryFirstOrDefault<int>(sqlCheckEmail, new { email });

                nameExist = countOfName > 0 ? true : false;
                emailExist = countOfEmail > 0 ? true : false;

                return Ok(new
                {
                    name_exist = nameExist,
                    email_exist = emailExist
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 編輯使用者
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Edit")]
        public async Task<ActionResult> EditUser(UserEditInput input)
        {
            string sql, sql_detail, rendomPassword;
            UserVerification hashedPassword = new UserVerification();


            if (input.id.HasValue)
            {
                sql = @"UPDATE user SET 
                            name = @name,
                            email = @email,
                            role_group = @role_group
                        WHERE id = @id ";

                sql_detail = @"UPDATE user_info SET 
                                full_name = @full_name, 
                                    phone = @phone 
                        WHERE id = @id ";
            }
            else
            {
                sql = @"INSERT INTO user 
                        (name, email, password_hash, salt, degree_of_parallelism, iterations, memory_size, role_group,status) 
                        VALUES 
                        (@name, @email, @password_hash, @salt, @degree_of_parallelism, @iterations, @memory_size, @role_group,@status);
                        SELECT CAST(LAST_INSERT_ID() AS INT);
                        ";

                sql_detail = @"INSERT INTO user_info 
                               (id, full_name, department, job_title, phone)
                               VALUES 
                              (@id, @full_name, @department, @job_title, @phone)";

                //新增時隨機生成密碼
                rendomPassword = GenerateSecurePassword(10);
                hashedPassword = await PasswordHasher.HashPassword(rendomPassword);
            }

            using (var transaction = await conn.BeginTransactionAsync())
            {
                try
                {
                    if (input.id.HasValue)
                    {
                        await conn.ExecuteScalarAsync<int>(sql, new
                        {
                            input.id.Value,
                            input.name,
                            input.email,
                            input.role_group,
                            @status = "Inactive" //預設未啟用
                        }, transaction);

                        await conn.ExecuteScalarAsync<int>(sql_detail, new
                        {
                            input.id.Value,
                            input.full_name,
                            input.department,
                            input.job_title,
                            input.phone,
                        });

                    }
                    else //新增人員
                    {
                        var userId = await conn.ExecuteScalarAsync<int>(sql, new
                        {
                            input.name,
                            input.email,
                            input.role_group,
                            hashedPassword.password_hash,
                            hashedPassword.salt,
                            hashedPassword.degree_of_parallelism,
                            hashedPassword.iterations,
                            hashedPassword.memory_size
                        }, transaction);

                        await conn.ExecuteScalarAsync<int>(sql_detail, new
                        {
                            @id = userId,
                            input.full_name,
                            input.department,
                            input.job_title,
                            input.phone,
                        }, transaction);

                        //寄送密碼至E-mail
                        if (input.id.HasValue == false)
                        {

                        }
                    }

                    await transaction.CommitAsync();
                    return Ok("done");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
