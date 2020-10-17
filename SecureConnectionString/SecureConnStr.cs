using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecureConnectionString
{
    public static class ConnectionString
    {
        public static string Encrypt(string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(connectionString, "Cannot be null or empity");

            var key = "priveteKey";
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using StreamWriter streamWriter = new StreamWriter(cryptoStream);
            streamWriter.Write(connectionString);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string Decrypt(string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(connectionString, "Cannot be null or empity");
            var key = "priveteKey";
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(connectionString));
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        public static bool IsConnectionStringCrypted(string connstr) =>
        connstr.Contains("Server")
        || connstr.Contains("Database")
        || connstr.Contains("Data Source")
        || connstr.Contains("Provider")
        || connstr.Contains("Initial Catalog")
        || connstr.Contains("User Id")
        || connstr.Contains("Integrated Security")
        || connstr.Contains("Provider")
        || connstr.Contains("Uid");
    }
}
