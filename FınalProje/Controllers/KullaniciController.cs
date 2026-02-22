using FınalProje.Data;
using FınalProje.Models;
using FınalProje.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FınalProje.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly AppDbContext _context;

        public KullaniciController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var model = _context.Kullanicilar.Select(k => new KullaniciViewModel
            {
                KullaniciID = k.KullaniciID,
                Ad = k.Ad,
                Soyad = k.Soyad
            }).ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Kullanici kullanici)
        {
            // Model doğrulama kontrolü
            if (!ModelState.IsValid)
            {
                return View(kullanici);
            }

            _context.Kullanicilar.Add(kullanici);
            _context.SaveChanges();

            return RedirectToAction(nameof(Yonetim));
        }


        public IActionResult Edit(int id)
        {
            var kullanici = _context.Kullanicilar.Find(id);
            if (kullanici == null) return NotFound();

            var viewModel = new KullaniciViewModel
            {
                KullaniciID = kullanici.KullaniciID,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, KullaniciViewModel model)
        {
            if (id != model.KullaniciID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var guncellenecekKullanici = new Kullanici
                    {
                        KullaniciID = model.KullaniciID,
                        Ad = model.Ad,
                        Soyad = model.Soyad
                    };

                    _context.Kullanicilar.Update(guncellenecekKullanici);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Kullanicilar.Any(e => e.KullaniciID == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Yonetim));
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var kullanici = _context.Kullanicilar.Find(id);

            if (kullanici == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı!" });
            }

            try
            {
                _context.Kullanicilar.Remove(kullanici);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Kullanıcı silinemedi. Bağlı verileri olabilir." });
            }
        }

        public IActionResult Yonetim()
        {
            var kullanicilar = _context.Kullanicilar
        .Select(k => new KullaniciViewModel
        {
            KullaniciID = k.KullaniciID,
            Ad = k.Ad,
            Soyad = k.Soyad
        }).ToList();

            return View(kullanicilar); 
        }
    }
}
