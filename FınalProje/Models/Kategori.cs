using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FınalProje.Models
{
    public class Kategori
    {
        [Key] 
        public int KategoriID { get; set; }

        [Required(ErrorMessage = "Kategori adı boş geçilemez.")]
        [StringLength(50, ErrorMessage = "Kategori adı en fazla 50 karakter olabilir.")]
        [Display(Name = "Kategori Adı")] 
        public string Ad { get; set; } = string.Empty;


        // Navigation Property: Kategoriye ait kapsüller
        public virtual ICollection<Kapsul> Kapsuller { get; set; } = new List<Kapsul>();
    }
}