using api.Models;
using api.Service;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public UserController(IDbConnection connection)
        {
            _connection = connection;
        }

        [HttpPost]
        [Route("GetUser")]
        public async Task<ActionResult<User>> GetUser(string id, string password)
        {
            try
            {
                string sql = @"SELECT id,name,role_group,status,
                                 password_hash,
                                 salt,
                                 degree_of_parallelism,
                                 iterations,
                                 memory_size
                               FROM user 
                              WHERE name = @parm_name OR email = @parm_email";

                var user = await _connection.QuerySingleOrDefaultAsync<User>(sql, new
                {
                    parm_name = id,
                    parm_email = id,
                });

                if (user == null)
                {
                    return Unauthorized("User Not Found");
                }

                //密碼驗證
                var isPasswordValid = await PasswordHasher.VerifyPassword(password,
                user.password_hash,
                user.salt,
                user.degree_of_parallelism,
                user.iterations,
                user.memory_size);

                if (!isPasswordValid)
                {
                    return Unauthorized("Invalid Password");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
