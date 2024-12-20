using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace api.Service
{
    public static class PasswordHasher
    {
        /// <summary>
        /// 生成隨機鹽
        /// </summary>
        /// <param name="length">字元長度</param>
        /// <returns></returns>
        static byte[] GenerateRandomSalt(int length)
        {
            var salt = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        /// <summary>
        /// 生成加密密碼
        /// </summary>
        /// <param name="password">未加密明碼</param>
        /// <returns></returns>
        public static async Task<(byte[] hash, byte[] salt, int degreeOfParallelism, int iterations, int memorySize)> HashPassword(string password)
        {
            byte[] salt = GenerateRandomSalt(16);
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = Environment.ProcessorCount,
                Iterations = 4,
                MemorySize = 65536
            };

            var hash = await argon2.GetBytesAsync(16);
            return (hash, salt, argon2.DegreeOfParallelism, argon2.Iterations, argon2.MemorySize);
        }

        /// <summary>
        /// 密碼驗證
        /// </summary>
        /// <param name="password">使用者輸入的密碼</param>
        /// <param name="passwordHash">加密值-密碼</param>
        /// <param name="salt">加密值-鹽</param>
        /// <param name="degreeOfParallelism">加密值-並行度</param>
        /// <param name="iterations">加密值-迭代次數</param>
        /// <param name="memorySize">加密值-記憶體大小</param>
        /// <returns></returns>
        public static async Task<bool> VerifyPassword(string password, byte[] passwordHash, byte[] salt, int degreeOfParallelism, int iterations, int memorySize)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = degreeOfParallelism,
                Iterations = iterations,
                MemorySize = memorySize
            };

            var generatedHash = await argon2.GetBytesAsync(passwordHash.Length);
            return passwordHash.SequenceEqual(generatedHash);
        }
    }
}
