using FınalProje.Data;
using FınalProje.Models;
using FınalProje.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FınalProje.Controllers
{
    public class KapsulController : Controller
    {
        private readonly AppDbContext _context;

        public KapsulController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? kategoriId, string? filtre)
        {
            var sorgu = _context.Kapsuller.AsQueryable();

            // 2. Kategori Filtreleme
            if (kategoriId.HasValue)
            {
                sorgu = sorgu.Where(k => k.KategoriID == kategoriId.Value);

                // Başlığı dinamik olarak veritabanından alalım
                var kategori = _context.Kategoriler.Find(kategoriId.Value);
                ViewBag.SayfaBasligi = kategori != null ? $"{kategori.Ad} Kapsüllerim" : "Kapsüllerim";
            }
            else
            {
                ViewBag.SayfaBasligi = "Tüm Kapsüllerim";
            }

            // 3. Kilit/Açık Filtreleme
            if (filtre == "kilitli")
                sorgu = sorgu.Where(k => k.AcilmaTarihi > DateTime.Now);
            else if (filtre == "acik")
                sorgu = sorgu.Where(k => k.AcilmaTarihi <= DateTime.Now);

            // 4. Sadece gerekli veriyi, tek bir sorguyla çek
            var model = sorgu.OrderByDescending(k => k.OlusturmaTarihi).ToList();

            ViewBag.KategoriId = kategoriId;
            ViewBag.AktifFiltre = filtre;

            return View(model);
        }


        [HttpGet]
        public IActionResult Create(int kategoriId)
        {
            var kategori = _context.Kategoriler.Find(kategoriId);

            var model = new KapsulViewModel
            {
                KategoriID = kategoriId,
                KategoriAdi = kategori?.Ad, 
                OlusturmaTarihi = DateTime.Now,
                AcilmaTarihi = DateTime.Now.AddDays(7),
                KullaniciListesi = _context.Kullanicilar.Select(u => new SelectListItem
                {
                    Value = u.KullaniciID.ToString(),
                    Text = u.Ad + " " + u.Soyad
                }).ToList(),
                KategoriListesi = _context.Kategoriler.Select(k => new SelectListItem
                {
                    Value = k.KategoriID.ToString(),
                    Text = k.Ad
                }).ToList(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KapsulViewModel model)
        {
            if (!ModelState.IsValid)
            {

                // Hata varsa listeleri TEKRAR doldurmalısın yoksa dropdownlar boş gelir
                model.KategoriListesi = _context.Kategoriler.Select(k => new SelectListItem
                {
                    Value = k.KategoriID.ToString(),
                    Text = k.Ad
                }).ToList();
                model.KullaniciListesi = _context.Kullanicilar.Select(u => new SelectListItem
                {
                    Value = u.KullaniciID.ToString(),
                    Text = u.Ad + " " + u.Soyad
                }).ToList();

                return View(model);
            }

            var kapsul = new Kapsul
            {
                Baslik = model.Baslik,
                Not = model.Not,
                KategoriID = model.KategoriID,
                KullaniciID = model.KullaniciID,
                OlusturmaTarihi = model.OlusturmaTarihi,
                AcilmaTarihi = model.AcilmaTarihi
            };

            _context.Kapsuller.Add(kapsul);
            _context.SaveChanges(); // Kapsül kaydedildi, ID oluştu.

            // 3. Bildirim Sistemi (Senin kurduğun harika mantık)
            var bugun = DateTime.Now.Date;

            // A - Standart Oluşturma Bildirimi
            _context.Bildirimler.Add(new Bildirim
            {
                Baslik = "Kapsül Oluşturuldu",
                Mesaj = $"'{kapsul.Baslik}' başlıklı kapsülünüz başarıyla kaydedildi.",
                KullaniciID = kapsul.KullaniciID,
                KapsulID = kapsul.KapsulID,
                Tarih = DateTime.Now,
                OkunduMu = false
            });

            // B - Yaklaşan Tarih Kontrolleri
            // 1 Hafta veya daha az kaldıysa
            if (kapsul.AcilmaTarihi.Date <= bugun.AddDays(7))
            {
                _context.Bildirimler.Add(new Bildirim
                {
                    Baslik = "Son 1 Hafta!",
                    Mesaj = $"Dikkat! '{kapsul.Baslik}' kapsülünün açılmasına 7 günden az zaman kaldı.",
                    KullaniciID = kapsul.KullaniciID,
                    KapsulID = kapsul.KapsulID,
                    Tarih = DateTime.Now,
                    OkunduMu = false
                });
            }
            // 1 Ay veya daha az kaldıysa
            else if (kapsul.AcilmaTarihi.Date <= bugun.AddMonths(1))
            {
                _context.Bildirimler.Add(new Bildirim
                {
                    Baslik = "1 Ay Kaldı",
                    Mesaj = $"'{kapsul.Baslik}' kapsülünüzün açılacağı güne 1 aydan az zamanınız kaldı.",
                    KullaniciID = kapsul.KullaniciID,
                    KapsulID = kapsul.KapsulID,
                    Tarih = DateTime.Now,
                    OkunduMu = false
                });
            }

            // Tüm bildirimleri veritabanına işle
            _context.SaveChanges();

            TempData["Mesaj"] = "Kapsül ve ilgili bildirimler başarıyla oluşturuldu.";

            // İşlem bitince yönetim sayfasına dön
            return RedirectToAction("Yonetim");
        }

        [HttpGet]
        public IActionResult Edit(int id) // Parametre ismi genellikle 'id' olur
        {
            // 1. Veritabanından o ID'ye ait kapsülü buluyoruz
            var kapsul = _context.Kapsuller.Find(id);

            if (kapsul == null) return NotFound();

            // 2. ViewModel oluştururken veritabanındaki değerleri içine yazıyoruz
            var model = new KapsulViewModel
            {
                KapsulID = kapsul.KapsulID,
                Baslik = kapsul.Baslik,          // Veritabanındaki Başlık kutuya gelir
                Not = kapsul.Not,                // Veritabanındaki Not kutuya gelir
                AcilmaTarihi = kapsul.AcilmaTarihi, // Veritabanındaki Tarih kutuya gelir
                OlusturmaTarihi = kapsul.OlusturmaTarihi,
                KategoriID = kapsul.KategoriID,  // Seçili kategori dropdown'da işaretlenir
                KullaniciID = kapsul.KullaniciID, // Seçili kullanıcı dropdown'da işaretlenir

                KategoriListesi = _context.Kategoriler.Select(k => new SelectListItem
                {
                    Value = k.KategoriID.ToString(),
                    Text = k.Ad,
                    Selected = k.KategoriID == kapsul.KategoriID // Otomatik seçili gelir
                }).ToList(),

                KullaniciListesi = _context.Kullanicilar.Select(u => new SelectListItem
                {
                    Value = u.KullaniciID.ToString(),
                    Text = u.Ad + " " + u.Soyad,
                    Selected = u.KullaniciID == kapsul.KullaniciID // Otomatik seçili gelir
                }).ToList()
            };

            return View(model);
        }



        [HttpPost]
        public IActionResult Update(Kapsul kapsul)
        {
            var eskiKapsul = _context.Kapsuller.FirstOrDefault(x => x.KapsulID == kapsul.KapsulID);
            if (eskiKapsul == null) return NotFound();

            eskiKapsul.Baslik = kapsul.Baslik;
            eskiKapsul.Not = kapsul.Not;
            eskiKapsul.AcilmaTarihi = kapsul.AcilmaTarihi;

            _context.Bildirimler.Add(new Bildirim
            {
                KullaniciID = eskiKapsul.KullaniciID,
                KapsulID = eskiKapsul.KapsulID,
                Baslik = "Kapsül Güncellendi",
                Mesaj = $"'{eskiKapsul.Baslik}' kapsülü başarıyla düzenlendi.",
                Tarih = DateTime.Now,
                OkunduMu = false
            });

            _context.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            var kapsul = _context.Kapsuller.FirstOrDefault(k => k.KapsulID == id);
            if (kapsul == null) return NotFound();

            // Kapsüle ait eski bildirimleri sil
            var eskiBildirimler = _context.Bildirimler
                .Where(b => b.KapsulID == id)
                .ToList();

            _context.Bildirimler.RemoveRange(eskiBildirimler);

            // Silme bildirimi ekle
            _context.Bildirimler.Add(new Bildirim
            {
                KullaniciID = kapsul.KullaniciID,
                Mesaj = $"'{kapsul.Baslik}' isimli kapsül başarıyla silindi.",
                Tarih = DateTime.Now,
                OkunduMu = false,
                KapsulID = null
            });

            _context.Kapsuller.Remove(kapsul);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            var kapsul = _context.Kapsuller
                .Include(k => k.Kullanici)
                .FirstOrDefault(k => k.KapsulID == id);

            if (kapsul == null) return NotFound();

            bool acikMi = DateTime.Now.Date >= kapsul.AcilmaTarihi.Date;

            return acikMi
                ? View("DetailsOpen", kapsul)
                : View("DetailsLocked", kapsul);
        }

        public IActionResult DetailsOpen(int id)
        {
            var kapsul = _context.Kapsuller
                .Include(k => k.Kullanici)
                .FirstOrDefault(k => k.KapsulID == id);

            return View(kapsul);
        }


        [HttpPost]
        public IActionResult TumunuOkunduYap()
        {
            var bildirimler = _context.Bildirimler
                .Where(b => !b.OkunduMu)
                .ToList();

            foreach (var b in bildirimler)
                b.OkunduMu = true;

            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetDetailJson(int id)
        {
            var kapsul = _context.Kapsuller
                .Include(k => k.Kullanici)
                .FirstOrDefault(k => k.KapsulID == id);

            if (kapsul == null)
                return NotFound();

            return Json(new
            {
                baslik = kapsul.Baslik,
                acilmaTarihi = kapsul.AcilmaTarihi.ToString("dd.MM.yyyy"),
                olusturmaTarihi = kapsul.OlusturmaTarihi.ToString("dd.MM.yyyy"),
                not = kapsul.Not,
                sahipAdSoyad = kapsul.Kullanici != null
                    ? kapsul.Kullanici.Ad + " " + kapsul.Kullanici.Soyad
                    : null
            });
        }
        public IActionResult Yonetim()
        {
            var kapsulListesi = _context.Kapsuller
                .Select(k => new KapsulViewModel
                {
                    KapsulID = k.KapsulID,
                    Baslik = k.Baslik,
                    Not = k.Not,
                    AcilmaTarihi = k.AcilmaTarihi,
                    OlusturmaTarihi = k.OlusturmaTarihi,

                    KategoriAdi = k.Kategori.Ad,

                    KullaniciAdSoyad = k.Kullanici.Ad + " " + k.Kullanici.Soyad
                })
                .OrderByDescending(x => x.OlusturmaTarihi) // En yeni eklenen en üstte
                .ToList();

            return View(kapsulListesi);
        }

    }
}
