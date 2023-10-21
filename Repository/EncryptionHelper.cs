using System.Security.Cryptography;

public class EncryptionHelper
{
    public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(ms, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    csEncrypt.Write(data, 0, data.Length);
                }
                return ms.ToArray();
            }
        }
    }

    public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream csDecrypt = new CryptoStream(ms, aesAlg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    csDecrypt.Write(data, 0, data.Length);
                }
                return ms.ToArray();
            }
        }
    }
}
