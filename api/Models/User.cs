using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    /// <summary>
    /// 傳入參數:登入驗證
    /// </summary>
    public class LoginVerificationInput
    {
        public string id { get; set; }
        public string password { get; set; }
    }

    /// <summary>
    /// 登入驗證回傳資料
    /// </summary>
    public class UserVerification
    {
        [Column("id")]
        public int id { get; set; }

        [Column("name")]
        public string full_name { get; set; }

        [Column("status")]
        public string status { get; set; }

        [Column("password_hash", TypeName = "binary(16)")]
        public byte[] password_hash { get; set; }

        [Column("salt", TypeName = "binary(16)")]
        public byte[] salt { get; set; }

        [Column("degree_of_parallelism")]
        public int degree_of_parallelism { get; set; }

        [Column("iterations")]
        public int iterations { get; set; }

        [Column("memory_size")]
        public int memory_size { get; set; }
    }

    public class UserInsertInput
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string role_group { get; set; }
        public string full_name { get; set; }
        public string department { get; set; }
        public string job_title { get; set; }
        public int phone { get; set; }
    }
}
