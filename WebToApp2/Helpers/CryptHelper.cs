using System.Security.Cryptography;
using System.Text;

namespace WebToApp2.Helpers
{
    public class CryptHelper
    {
        public static string Encrypt(string clearText)
        {
            var EncryptionKey = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var clearBytes = Encoding.Unicode.GetBytes(clearText);
            using Aes encryptor = Aes.Create();
            var pdb = new Rfc2898DeriveBytes(EncryptionKey,
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }

                clearText = Convert.ToBase64String(ms.ToArray());
            }

            return clearText;
        }

        public static string Decrypt(string clearText)
        {
            string EncryptionKey = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            clearText = clearText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[]
                {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    clearText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return clearText;
        }

        public static string CreateMD5Hash2(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = Encoding.Default.GetBytes(str);
            byte[] encoded = md5.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encoded.Length; i++)
                sb.Append(encoded[i].ToString("x2"));

            return sb.ToString();
        }

        public static string Sha512encrypt(string phrase)
        {
            phrase = phrase + "s!m@@ccess3";
            UTF8Encoding encoder = new UTF8Encoding();
            SHA512Managed sha512Hasher = new();
            byte[] hashedDataBytes = sha512Hasher.ComputeHash(encoder.GetBytes(phrase));
            return Convert.ToBase64String(hashedDataBytes);
        }

        public static byte[] ComputeSha256HashAsByte(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
                return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        }

        public static byte[] GetHMAC(byte[] computedSha256AsByte, String key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                return hmac.ComputeHash(computedSha256AsByte);
                // var hashData = hmac.ComputeHash(computedSha256AsByte);
                // var base64Signature = Convert.ToBase64String(hashData);
                // return base64Signature;
            }
        }
    }
}
