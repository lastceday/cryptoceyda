

## CryptoCeyda

**CryptoCeyda**, Bilgi Güvenliği ve Kriptoloji dersi kapsamında geliştirdiğim, hem metin hem de dosya şifreleme/deşifreleme işlemlerini güvenli ve pratik bir şekilde gerçekleştirmeyi amaçlayan bir web uygulamasıdır.

### Projenin Amacı ve Geliştirilme Nedeni

Bu projeyi hem teorik bilgimi pratiğe dökmek hem de modern şifreleme algoritmalarının gerçek hayatta nasıl kullanılabileceğini göstermek amacıyla geliştirdim. Amacım, kullanıcıların hem metin hem de dosya tabanlı verilerini kolayca şifreleyip çözebileceği, sade ve anlaşılır bir arayüze sahip, güvenli bir uygulama ortaya koymaktı.

### Kullanılan Teknolojiler

- **.NET 6 / ASP.NET Core MVC:** Uygulamanın sunucu tarafı ve web arayüzü için.
- **C#:** Tüm iş mantığı ve kriptografi işlemleri için ana programlama dili.
- **Razor View Engine:** Dinamik HTML sayfalarının oluşturulması için.
- **JavaScript:** Anahtar ve IV üretimi, kopyalama işlemleri ve kullanıcı etkileşimi için.
- **Bootstrap:** Modern ve responsive (mobil uyumlu) arayüz tasarımı için.
- **System.Security.Cryptography:** RSA ve AES algoritmalarının güvenli şekilde uygulanması için .NET’in yerleşik kriptografi kütüphanesi.
- **Git & GitHub:** Sürüm kontrolü ve kod paylaşımı için.

### Projenin İşlevleri

- **RSA Anahtar Çifti Oluşturma:** Kullanıcılar, tek tıkla kendi RSA açık ve özel anahtar çiftlerini oluşturabilir.
- **RSA ile Metin Şifreleme/Deşifreleme:** Girilen metinler, RSA algoritması ile güvenli bir şekilde şifrelenip çözülebilir.
- **RSA ile Dosya Şifreleme/Deşifreleme (Hibrit Yöntem):** Büyük dosyalar için hibrit şifreleme (AES+RSA) kullanılır. Dosya, rastgele üretilen bir AES anahtarı ile şifrelenir; bu anahtar ve IV ise RSA ile şifrelenerek güvenli aktarım sağlanır.
- **AES Anahtar ve IV Üretimi:** Uygulama içerisinde güvenli ve rastgele AES anahtarı ve IV (Initialization Vector) otomatik olarak üretilebilir.
- **AES ile Metin ve Dosya Şifreleme/Deşifreleme:** Simetrik şifreleme algoritması olan AES ile hem metin hem de dosya şifreleme/deşifreleme işlemleri yapılabilir.
- **Kullanıcı Dostu ve Modern Arayüz:** Sade ve anlaşılır arayüz sayesinde, teknik bilgi gerektirmeden tüm işlemler kolayca gerçekleştirilebilir.
- **Hata ve Bilgilendirme Mekanizması:** Eksik veya hatalı girişlerde kullanıcıya bilgilendirici uyarılar gösterilir.

---

**CryptoCeyda**, hem kriptografi öğrenmek isteyenler hem de günlük hayatta veri güvenliğine önem veren kullanıcılar için pratik ve güvenli bir çözüm sunar.

---
###EKRAN GÖRÜNTÜLERİ

### ANASAYFA

![Anasayfa](anasayfa.png)

Anasayfamız bu şekilde gözüküyor.

### SHA-256 HASH HESAPLAMA

![Sha-256 hash hesaplama](sha%hash%hesaplama.png)

Bu ekranda, kullanıcı metin kutusuna istediği bir veriyi (örneğin "merhaba ben ceyda") girip Hesapla butonuna tıkladığında, SHA-256 algoritması ile bu verinin hash (özet) değeri hesaplanır ve ekranda gösterilir.
