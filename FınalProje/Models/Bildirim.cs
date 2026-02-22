using System;
using System.ComponentModel.DataAnnotations;

namespace FınalProje.Models
{
    // Bildirim (notification) modelini temsil eder
    public class Bildirim
    {
        [Key]
        public int BildirimID { get; set; }

        // Bildirim başlığı
        [Required]
        public string Baslik { get; set; } = string.Empty;

        // Bildirim içeriği
        [Required]
        public string Mesaj { get; set; } = string.Empty;

       
        // Bildirim zamanı için aşağıdaki "Tarih" alanı kullanılmaktadır.
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        // Bildirimin okunma durumu
        public bool OkunduMu { get; set; } = false;

        // -----------------------------
        // İLİŞKİLER
        // -----------------------------

        // Bildirimin bağlı olduğu kapsül (opsiyonel)
        public int? KapsulID { get; set; }
        public Kapsul? Kapsul { get; set; }

        // Bildirimin oluşturulma zamanı (aktif kullanılan alan)
        public DateTime Tarih { get; set; }

        // Bildirimin ait olduğu kullanıcı
        public int KullaniciID { get; set; }
    }
}

