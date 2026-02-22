using FınalProje.Data;
using FınalProje.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FınalProje.Controllers
{
    public class BildirimController : Controller
    {
        private readonly AppDbContext _context;

        public BildirimController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var bildirim = _context.Bildirimler.Find(id);
            if (bildirim == null)
                return NotFound();

            _context.Bildirimler.Remove(bildirim);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            // Sayfa açıldığında okunmamışları okundu yap
            var okunmamislar = _context.Bildirimler
                .Where(b => !b.OkunduMu)
                .ToList();

            foreach (var b in okunmamislar)
            {
                b.OkunduMu = true;
            }

            _context.SaveChanges();

            // ViewModel'e mapleme
            var model = _context.Bildirimler
                .OrderByDescending(b => b.Tarih)
                .Select(b => new BildirimListeViewModel
                {
                    BildirimID = b.BildirimID,
                    Baslik = b.Baslik,
                    Mesaj = b.Mesaj,
                    OkunduMu = b.OkunduMu,
                    Tarih = b.Tarih.ToString("dd.MM.yyyy HH:mm")
                })
                .ToList();

            return View(model);
        }


        public IActionResult TumunuSil()
        {            
            var tumBildirimler = _context.Bildirimler.ToList();
            _context.Bildirimler.RemoveRange(tumBildirimler);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
