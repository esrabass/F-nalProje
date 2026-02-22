using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FınalProje.ViewModels
{
    public class KapsulViewModel
    {
        public int KapsulID { get; set; }

        [Required(ErrorMessage = "Kapsül başlığı zorunludur.")]
        [Display(Name = "Kapsül Başlığı")]
        public string Baslik { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açılma tarihi seçilmelidir.")]
        [Display(Name = "Açılma Tarihi")]
        [DataType(DataType.Date)]
        public DateTime AcilmaTarihi { get; set; } = DateTime.Now.AddDays(1); // Varsayılan yarın

        [Display(Name = "Oluşturma Tarihi")]
        [DataType(DataType.Date)]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Kapsül Notu (İçerik)")]
        public string? Not { get; set; }

        // --- ID Bilgileri (Formda Seçim İçin) ---
        public int KullaniciID { get; set; }
        public int KategoriID { get; set; }

        // --- Senin İstediğin Ekstra Alanlar (Listeleme İçin) ---
        // Bu alanlar veritabanı tablosunda yok, sadece ekranda isim göstermek için kullanıyoruz.
        public string? KategoriAdi { get; set; }
        public string? KullaniciAdSoyad { get; set; }

        // --- Dropdown Listeleri ---
        public IEnumerable<SelectListItem>? KategoriListesi { get; set; }
        public IEnumerable<SelectListItem>? KullaniciListesi { get; set; }
    }
}