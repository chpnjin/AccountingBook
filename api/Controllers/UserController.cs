using api.Models;
using api.Service;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDbConnection conn;

        public UserController(IDbConnection connection)
        {
            conn = connection;
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
        public async Task<ActionResult> CheckUserExist(string name,string email)
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

                return Ok(new {
                    name_exist = nameExist,
                    email_exist = emailExist
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Edit")]
        public async Task<ActionResult> EditUser(UserInsertInput input)
        {
            string sql;
            if (input.id.HasValue)
            {
                sql = "";
            }
            else
            {
                sql = "";
            }
        }
    }
}
