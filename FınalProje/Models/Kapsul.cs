using System;
using System.ComponentModel.DataAnnotations;

namespace FınalProje.Models
{
    public class Kapsul
    {
        [Key]
        public int KapsulID { get; set; }

        [Required]
        public string Baslik { get; set; } = null!;

        [Required]
        public DateTime AcilmaTarihi { get; set; }

        // Kapsül içeriği (opsiyonel)
        // Not: Nullable olarak tasarlanmıştır, formdan boş gelebilir
        public string? Not { get; set; } = null!;

        // Kapsülün sistem tarafından oluşturulduğu tarih
        // Not: Bu alan formdan alınmaz, server-side olarak set edilir
        public DateTime OlusturmaTarihi { get; set; }


        // KULLANICI İLİŞKİSİ

        public int KullaniciID { get; set; }
        public Kullanici Kullanici { get; set; } = null!;


        // KATEGORİ İLİŞKİSİ

        public int KategoriID { get; set; }
        public Kategori Kategori { get; set; } = null!;
    }
}

