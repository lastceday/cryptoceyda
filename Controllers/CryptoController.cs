using Microsoft.AspNetCore.Mvc;
using cryptoceyda.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace cryptoceyda.Controllers
{
    public class CryptoController : Controller
    {
        // SHA256 özetleme işlemi
        [HttpPost]
        public IActionResult Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                TempData["HashResult"] = "Lütfen bir metin girin!";
                TempData["HashInput"] = null;
                return RedirectToAction("Index", "Home");
            }
            var hashedValue = CryptoModel.ComputeSHA256(input);
            TempData["HashInput"] = input;
            TempData["HashResult"] = hashedValue;
            return RedirectToAction("Index", "Home");
        }

        // RSA şifreleme işlemi
        [HttpPost]
        public IActionResult EncryptRSA(string data, string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                publicKey = HttpContext.Session.GetString("RSAPublicKey");
            }

            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(publicKey))
            {
                TempData["RSAEncryptResult"] = "Lütfen veri ve geçerli bir açık anahtar girin veya oturumdaki anahtarı kullanın!";
                return RedirectToAction("RSA", "Home");
            }
            var encryptedData = CryptoModel.EncryptRSA(data, publicKey);
            TempData["RSAEncryptInput"] = data;
            TempData["RSAEncryptKey"] = publicKey;
            TempData["RSAEncryptResult"] = encryptedData;
            return RedirectToAction("RSA", "Home");
        }

        // RSA şifre çözme işlemi
        [HttpPost]
        public IActionResult DecryptRSA(string encryptedData, string privateKey)
        {
            if (string.IsNullOrEmpty(privateKey))
            {
                privateKey = HttpContext.Session.GetString("RSAPrivateKey");
            }

            if (string.IsNullOrEmpty(encryptedData) || string.IsNullOrEmpty(privateKey))
            {
                TempData["RSADecryptResult"] = "Lütfen şifreli veri ve geçerli bir özel anahtar girin veya oturumdaki anahtarı kullanın!";
                return RedirectToAction("RSA", "Home");
            }
            var decryptedData = CryptoModel.DecryptRSA(encryptedData, privateKey);
            TempData["RSADecryptInput"] = encryptedData;
            TempData["RSADecryptKey"] = privateKey;
            TempData["RSADecryptResult"] = decryptedData;
            return RedirectToAction("RSA", "Home");
        }

        // AES metin şifreleme
        [HttpPost]
        public IActionResult EncryptAES(string plainText, string keyBase64, string ivBase64)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(keyBase64) || string.IsNullOrEmpty(ivBase64))
            {
                TempData["AESEncryptResult"] = "Lütfen metin, anahtar ve IV girin!";
                return RedirectToAction("AES", "Home");
            }
            var encrypted = CryptoModel.EncryptAES(plainText, keyBase64, ivBase64);
            TempData["AESEncryptInput"] = plainText;
            TempData["AESEncryptKey"] = keyBase64;
            TempData["AESEncryptIV"] = ivBase64;
            TempData["AESEncryptResult"] = encrypted;
            return RedirectToAction("AES", "Home");
        }

        // AES metin şifre çözme
        [HttpPost]
        public IActionResult DecryptAES(string cipherTextBase64, string keyBase64, string ivBase64)
        {
            if (string.IsNullOrEmpty(cipherTextBase64) || string.IsNullOrEmpty(keyBase64) || string.IsNullOrEmpty(ivBase64))
            {
                TempData["AESDecryptResult"] = "Lütfen şifreli metin, anahtar ve IV girin!";
                return RedirectToAction("AES", "Home");
            }
            var decrypted = CryptoModel.DecryptAES(cipherTextBase64, keyBase64, ivBase64);
            TempData["AESDecryptInput"] = cipherTextBase64;
            TempData["AESDecryptKey"] = keyBase64;
            TempData["AESDecryptIV"] = ivBase64;
            TempData["AESDecryptResult"] = decrypted;
            return RedirectToAction("AES", "Home");
        }

        // AES dosya şifreleme
        [HttpPost]
        public IActionResult EncryptAESFile(IFormFile file, string keyBase64, string ivBase64)
        {
            if (file == null || string.IsNullOrEmpty(keyBase64) || string.IsNullOrEmpty(ivBase64))
            {
                TempData["AESEncryptFileResult"] = "Lütfen dosya, anahtar ve IV girin!";
                return RedirectToAction("AES", "Home");
            }
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var encryptedBytes = CryptoModel.EncryptAESFile(ms.ToArray(), keyBase64, ivBase64);
                TempData["AESEncryptFileName"] = file.FileName;
                TempData["AESEncryptFileKey"] = keyBase64;
                TempData["AESEncryptFileIV"] = ivBase64;
                TempData["AESEncryptFileResult"] = "Dosya başarıyla şifrelendi. Şifreli Dosya İçeriğini kaydedin!";
                TempData["AESEncryptedFileBase64"] = Convert.ToBase64String(encryptedBytes);
                return RedirectToAction("AES", "Home");
            }
        }

        // AES dosya şifre çözme
        [HttpPost]
        public IActionResult DecryptAESFile(IFormFile file, string keyBase64, string ivBase64)
        {
            if (file == null || string.IsNullOrEmpty(keyBase64) || string.IsNullOrEmpty(ivBase64))
            {
                TempData["AESDecryptFileResult"] = "Lütfen dosya, anahtar ve IV girin!";
                return RedirectToAction("AES", "Home");
            }
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var decrypted = CryptoModel.DecryptAESFile(ms.ToArray(), keyBase64, ivBase64);
                TempData["AESDecryptFileName"] = file.FileName.Replace(".enc", "");
                TempData["AESDecryptFileKey"] = keyBase64;
                TempData["AESDecryptFileIV"] = ivBase64;
                TempData["AESDecryptFileResult"] = "Dosya başarıyla çözüldü. İndirebilirsiniz.";
                TempData["AESDecryptedFileBase64"] = Convert.ToBase64String(decrypted);
                return RedirectToAction("AES", "Home");
            }
        }

        // RSA dosya şifreleme
        [HttpPost]
        public IActionResult EncryptRSAFile(IFormFile file, string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                publicKey = HttpContext.Session.GetString("RSAPublicKey");
            }

            if (file == null || string.IsNullOrEmpty(publicKey))
            {
                TempData["RSAEncryptFileResult"] = "Lütfen dosya ve geçerli bir açık anahtar girin veya oturumdaki anahtarı kullanın!";
                return RedirectToAction("RSA", "Home");
            }
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var encryptedData = CryptoModel.EncryptRSAFileHybrid(ms.ToArray(), publicKey);
                
                // Şifrelenmiş AES anahtarı ve IV'yi TempData'ya kaydet
                TempData["RSAEncryptedAesKey"] = Convert.ToBase64String(encryptedData.EncryptedAesKey);
                TempData["RSAEncryptedAesIV"] = Convert.ToBase64String(encryptedData.EncryptedAesIV);
                TempData["RSAEncryptFileResult"] = "Dosya başarıyla şifrelendi. Şifreli AES Anahtarını ve IV'sini kaydedin!";

                // Geçici olarak dosyayı kaydedin ve dosya adını TempData'ya ekleyin
                var tempFileName = Guid.NewGuid().ToString() + ".enc";
                var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp", tempFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath)); // Klasör yoksa oluştur
                System.IO.File.WriteAllBytes(tempFilePath, encryptedData.EncryptedFileBytes);
                TempData["RSAEncryptedTempFileName"] = tempFileName;
                TempData["RSAOriginalFileName"] = file.FileName; // Orijinal dosya adını da saklayalım

                // RSA sayfasına geri yönlendir ve sonuçları TempData ile taşı
                return RedirectToAction("RSA", "Home");
            }
        }

        [HttpGet]
        public IActionResult DownloadRSAEncryptedFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            // Dosyayı indirdikten sonra geçici dosyayı silebiliriz (isteğe bağlı, güvenlik için önerilir)
            // System.IO.File.Delete(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        // RSA dosya deşifreleme
        [HttpPost]
        public IActionResult DecryptRSAFile(IFormFile file, string privateKey, string encryptedAesKeyBase64, string encryptedAesIVBase64)
        {
            if (string.IsNullOrEmpty(privateKey))
            {
                privateKey = HttpContext.Session.GetString("RSAPrivateKey");
            }

            if (file == null || string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(encryptedAesKeyBase64) || string.IsNullOrEmpty(encryptedAesIVBase64))
            {
                TempData["RSADecryptFileResult"] = "Lütfen dosya, özel anahtar, şifreli AES anahtarı ve IV girin veya oturumdaki anahtarı kullanın!";
                return RedirectToAction("RSA", "Home");
            }
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                byte[] decrypted = CryptoModel.DecryptRSAFileHybrid(ms.ToArray(), Convert.FromBase64String(encryptedAesKeyBase64), Convert.FromBase64String(encryptedAesIVBase64), privateKey);
                
                // Geçici olarak deşifreli dosyayı kaydedin ve dosya adını TempData'ya ekleyin
                var originalFileName = file.FileName.Replace(".enc", "");
                var tempFileName = Guid.NewGuid().ToString() + "_decrypted_" + originalFileName;
                var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp", tempFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath)); // Klasör yoksa oluştur
                System.IO.File.WriteAllBytes(tempFilePath, decrypted);
                TempData["RSADecryptedTempFileName"] = tempFileName;
                TempData["RSADecryptedOriginalFileName"] = originalFileName; // Orijinal dosya adını da saklayalım

                TempData["RSADecryptFileResult"] = "Dosya başarıyla çözüldü. İndirebilirsiniz.";
                
                // RSA sayfasına geri yönlendir ve sonuçları TempData ile taşı
                return RedirectToAction("RSA", "Home");
            }
        }

        [HttpGet]
        public IActionResult DownloadRSADecryptedFile(string fileName, string originalFileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(originalFileName))
            {
                return NotFound();
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            // Dosyayı indirdikten sonra geçici dosyayı silebiliriz (isteğe bağlı, güvenlik için önerilir)
            // System.IO.File.Delete(filePath);
            return File(fileBytes, "application/octet-stream", originalFileName);
        }

        // SHA-256 dosya özeti
        [HttpPost]
        public IActionResult HashFile(IFormFile file)
        {
            if (file == null)
            {
                TempData["HashFileResult"] = "Lütfen bir dosya seçin!";
                return RedirectToAction("Index", "Home");
            }
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var hash = CryptoModel.ComputeSHA256File(ms.ToArray());
                TempData["HashFileName"] = file.FileName;
                TempData["HashFileResult"] = hash;
                return RedirectToAction("Index", "Home");
            }
        }

        // RSA Anahtar Çifti Oluşturma
        [HttpPost]
        public IActionResult GenerateRSAKeys()
        {
            using (var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                HttpContext.Session.SetString("RSAPublicKey", publicKey);
                HttpContext.Session.SetString("RSAPrivateKey", privateKey);
            }
            return RedirectToAction("RSA", "Home");
        }
    }
}

