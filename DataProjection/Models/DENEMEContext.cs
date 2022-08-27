using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataProjection.Models
{
    public partial class DENEMEContext : DbContext
    {
        public DENEMEContext()
        {
        }

        public DENEMEContext(DbContextOptions<DENEMEContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Urunler> Urunlers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

            modelBuilder.Entity<Urunler>(entity =>
            {
                entity.ToTable("Urunler");

                entity.Property(e => e.Ad)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Fiyat).HasColumnType("money");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
