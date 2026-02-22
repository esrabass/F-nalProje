using FınalProje.Models;
using Microsoft.EntityFrameworkCore;

namespace FınalProje.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Kapsul> Kapsuller { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Bildirim> Bildirimler { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}

