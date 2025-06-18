using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace cryptoceyda.Models
{
    public class RSAFileEncryptedData
    {
        public byte[] EncryptedFileBytes { get; set; }
        public byte[] EncryptedAesKey { get; set; }
        public byte[] EncryptedAesIV { get; set; }
    }

    public class CryptoModel
    {
        // SHA256 Özetleme
        public static string ComputeSHA256(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                foreach (byte byteData in data)
                {
                    sBuilder.Append(byteData.ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        // RSA Şifreleme
        public static string EncryptRSA(string data, string publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] encryptedData = rsa.Encrypt(dataBytes, RSAEncryptionPadding.OaepSHA256);
                return Convert.ToBase64String(encryptedData);
            }
        }

        // RSA Şifre Çözme
        public static string DecryptRSA(string encryptedData, string privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
                byte[] decryptedData = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        // AES Metin Şifreleme
        public static string EncryptAES(string plainText, string keyBase64, string ivBase64)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(keyBase64);
                aesAlg.IV = Convert.FromBase64String(ivBase64);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                    swEncrypt.Flush();
                    csEncrypt.FlushFinalBlock();
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        // AES Metin Şifre Çözme
        public static string DecryptAES(string cipherTextBase64, string keyBase64, string ivBase64)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(keyBase64);
                aesAlg.IV = Convert.FromBase64String(ivBase64);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherTextBase64)))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        // AES Dosya Şifreleme
        public static byte[] EncryptAESFile(byte[] fileBytes, string keyBase64, string ivBase64)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(keyBase64);
                aesAlg.IV = Convert.FromBase64String(ivBase64);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(fileBytes, 0, fileBytes.Length);
                    csEncrypt.FlushFinalBlock();
                    return msEncrypt.ToArray();
                }
            }
        }

        // AES Dosya Şifre Çözme
        public static byte[] DecryptAESFile(byte[] cipherBytes, string keyBase64, string ivBase64)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(keyBase64);
                aesAlg.IV = Convert.FromBase64String(ivBase64);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherBytes))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var msOutput = new MemoryStream())
                {
                    csDecrypt.CopyTo(msOutput);
                    return msOutput.ToArray();
                }
            }
        }

        // RSA Dosya Şifreleme (Hibrid Şifreleme)
        public static RSAFileEncryptedData EncryptRSAFileHybrid(byte[] fileBytes, string publicKey)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                aesAlg.GenerateIV();

                byte[] encryptedFileBytes = EncryptAESFileRaw(fileBytes, aesAlg.Key, aesAlg.IV);

                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

                    byte[] encryptedAesKey = rsa.Encrypt(aesAlg.Key, RSAEncryptionPadding.OaepSHA256);
                    byte[] encryptedAesIV = rsa.Encrypt(aesAlg.IV, RSAEncryptionPadding.OaepSHA256);

                    return new RSAFileEncryptedData
                    {
                        EncryptedFileBytes = encryptedFileBytes,
                        EncryptedAesKey = encryptedAesKey,
                        EncryptedAesIV = encryptedAesIV
                    };
                }
            }
        }

        // RSA Dosya Deşifreleme (Hibrid Şifreleme)
        public static byte[] DecryptRSAFileHybrid(byte[] encryptedFileBytes, byte[] encryptedAesKey, byte[] encryptedAesIV, string privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

                byte[] decryptedAesKey = rsa.Decrypt(encryptedAesKey, RSAEncryptionPadding.OaepSHA256);
                byte[] decryptedAesIV = rsa.Decrypt(encryptedAesIV, RSAEncryptionPadding.OaepSHA256);

                return DecryptAESFileRaw(encryptedFileBytes, decryptedAesKey, decryptedAesIV);
            }
        }

        // AES Dosya Şifreleme (Raw Bytes)
        public static byte[] EncryptAESFileRaw(byte[] fileBytes, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(fileBytes, 0, fileBytes.Length);
                    csEncrypt.FlushFinalBlock();
                    return msEncrypt.ToArray();
                }
            }
        }

        // AES Dosya Şifre Çözme (Raw Bytes)
        public static byte[] DecryptAESFileRaw(byte[] cipherBytes, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherBytes))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var msOutput = new MemoryStream())
                {
                    csDecrypt.CopyTo(msOutput);
                    return msOutput.ToArray();
                }
            }
        }

        // SHA-256 Dosya Özeti
        public static string ComputeSHA256File(byte[] fileBytes)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] data = sha256Hash.ComputeHash(fileBytes);
                StringBuilder sBuilder = new StringBuilder();
                foreach (byte byteData in data)
                {
                    sBuilder.Append(byteData.ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
