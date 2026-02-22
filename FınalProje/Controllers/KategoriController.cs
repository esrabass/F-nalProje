using FınalProje.Data;
using FınalProje.Models;
using FınalProje.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FınalProje.Controllers
{
    public class KategoriController : Controller
    {
        private readonly AppDbContext _context;

        public KategoriController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var kategoriler = _context.Kategoriler
                .Select(k => new KategoriViewModel
                {
                    KategoriID = k.KategoriID,
                    Ad = k.Ad

                })
                .ToList();

            return View(kategoriler);
        }


        public IActionResult Yonetim()
        {
            var kategoriler = _context.Kategoriler
        .Select(k => new KategoriViewModel
        {
            KategoriID = k.KategoriID,
            Ad = k.Ad,

        }).ToList();

            return View(kategoriler); 
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Kategori kategori, IFormFile? resimDosyasi)
        {

            _context.Kategoriler.Add(kategori);
            _context.SaveChanges(); 

            return RedirectToAction(nameof(Yonetim));
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var kategori = _context.Kategoriler.Find(id);
            if (kategori == null) return NotFound();

            var viewModel = new KategoriViewModel
            {
                KategoriID = kategori.KategoriID,
                Ad = kategori.Ad

            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, KategoriViewModel model)
        {
            if (id != model.KategoriID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var guncellenecekKategori = new Kategori
                    {
                        KategoriID = model.KategoriID,
                        Ad = model.Ad,

                    };

                    _context.Kategoriler.Update(guncellenecekKategori);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Kategoriler.Any(e => e.KategoriID == id))
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
            var kategori = _context.Kategoriler.Find(id);

            if (kategori == null)
            {
                return Json(new { success = false, message = "Kategori bulunamadı!" });
            }

            try
            {
              

                _context.Kategoriler.Remove(kategori);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
