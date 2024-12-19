using api.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Data;
using System.Data.SqlClient;

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
        public async Task<ActionResult<User>> GetUser(string email,string password)
        {
            string sql = @"SELECT id, name, role_group 
                             FROM user 
                            WHERE email = @parm_email AND Password = @parm_password";

            var user = await _connection.QuerySingleOrDefaultAsync<User>(sql, new
            {
                //parm_email = input.GetValue("email"),
                //parm_password = input.GetValue("password")
                parm_email = email,
                parm_password = password
            });

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(user);
        }
    }
}
