namespace FınalProje.ViewModels
{
    public class BildirimListeViewModel
    {
        public int BildirimID { get; set; }

        public string Baslik { get; set; } = string.Empty;

        public string Mesaj { get; set; } = string.Empty;

        public bool OkunduMu { get; set; }

        public string Tarih { get; set; } = string.Empty;
    }
}
