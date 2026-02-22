using System.ComponentModel.DataAnnotations;

namespace FınalProje.Models
{
    public class Kullanici
    {
        [Key]
        public int KullaniciID { get; set; }

        [Required]
        public string Ad { get; set; } = string.Empty;

        [Required]
        public string Soyad { get; set; } = string.Empty;
    }
}
