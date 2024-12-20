using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class User
    {
        [Column("id")]
        public int id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("role_group")]
        public string role_group { get; set; }

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
}
