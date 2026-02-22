using System.ComponentModel.DataAnnotations;


namespace FınalProje.ViewModels
{
    public class KategoriViewModel
    {
        public int KategoriID { get; set; }

        [Required(ErrorMessage = "Kategori adı boş geçilemez.")]
        public string Ad { get; set; } = string.Empty;

    }
}
