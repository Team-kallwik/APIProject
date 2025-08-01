using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Cryptography;
using System.Text;

namespace DapperWebApiWIthAuthentication.Models
{
    public  class EncryptionHelper
    {
        private readonly string _Key;
        public EncryptionHelper(IConfiguration configuration)
        {
            _Key = configuration["EncryptionSettings:Key"];

            if (string.IsNullOrEmpty(_Key) || _Key.Length != 16)
            {
                throw new Exception("Encryption key must be 16 characters long.");
            }
        }
       

        public  string Encrypt(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_Key);
                aes.IV = iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                array = ms.ToArray();
            }

            return Convert.ToBase64String(array);
        }

        public  string Decrypt(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_Key);
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }

}
