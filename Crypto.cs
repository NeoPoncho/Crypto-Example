using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public static class Crypto
    {
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, out byte[] iv)
        {
            byte[] encryptedBytes;

            using (MemoryStream ms = new())
            {
                using Aes AES = Aes.Create();

                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Mode = CipherMode.CBC;

                AES.GenerateIV();
                iv = AES.IV;
                AES.Key = SHA256.HashData(passwordBytes);

                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cs.Close();
                }

                encryptedBytes = ms.ToArray();
            }

            return encryptedBytes;
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] iv)
        {
            byte[] decryptedBytes = null;

            using (MemoryStream ms = new())
            {
                using Aes AES = Aes.Create();

                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Mode = CipherMode.CBC;
                AES.Key = SHA256.HashData(passwordBytes);
                AES.IV = iv;

                using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    cs.Close();
                }

                decryptedBytes = ms.ToArray();
            }

            return decryptedBytes;
        }

        public static string Encrypt(string password, string salt = "MySecretSaltWhichIWantToKeepWorking")
        {
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(password);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(salt);
            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes, out byte[] iv);
            string result = Convert.ToBase64String(iv) + ":" + Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        public static string Decrypt(string hash, string salt = "MySecretSaltWhichIWantToKeepWorking")
        {
            try
            {
                string[] parts = hash.Split(':');
                byte[] iv = Convert.FromBase64String(parts[0]);
                byte[] bytesToBeDecrypted = Convert.FromBase64String(parts[1]);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(salt);
                byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes, iv);
                string result = Encoding.UTF8.GetString(bytesDecrypted);

                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
