using System.ComponentModel.DataAnnotations;

namespace FınalProje.ViewModels
{
    public class KullaniciViewModel
    {
        public int KullaniciID { get; set; }

        [Required(ErrorMessage = "Ad alanı boş bırakılamaz.")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz.")]
        public string Soyad { get; set; } = string.Empty;

        // Tam Ad (Listeleme yaparken kolaylık sağlar)
        public string TamAd => $"{Ad} {Soyad}";
    }
}