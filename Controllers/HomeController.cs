using cryptoceyda.Models;
using Microsoft.AspNetCore.Mvc;

namespace cryptoceyda.Controllers
{
    public class HomeController : Controller
    {
        // GET: Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: RSA
        public IActionResult RSA()
        {
            return View();
        }

        // GET: AES
        public IActionResult AES()
        {
            return View();
        }

        // POST: Hash işlemi
        [HttpPost]
        public IActionResult Hash(string input)
        {
            var hashedValue = CryptoModel.ComputeSHA256(input);
            ViewBag.HashedValue = hashedValue; // ViewBag üzerinden veriyi gönderiyoruz
            return View("Index"); // Aynı sayfada sonucu göster
        }

        // POST: RSA şifreleme işlemi
        [HttpPost]
        public IActionResult EncryptRSA(string data, string publicKey)
        {
            var encryptedData = CryptoModel.EncryptRSA(data, publicKey);
            ViewBag.EncryptedData = encryptedData; // EncryptedData'yı ViewBag ile gönderiyoruz
            return View("Index"); // Aynı sayfada sonucu göster
        }

        // POST: RSA şifre çözme işlemi
        [HttpPost]
        public IActionResult DecryptRSA(string encryptedData, string privateKey)
        {
            var decryptedData = CryptoModel.DecryptRSA(encryptedData, privateKey);
            ViewBag.DecryptedData = decryptedData; // DecryptedData'yı ViewBag ile gönderiyoruz
            return View("Index"); // Aynı sayfada sonucu göster
        }
    }
}


