using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarsLab
{
    public partial class AutomobilesDBContext : DbContext
    {
        public AutomobilesDBContext()
        {
        }

        public AutomobilesDBContext(DbContextOptions<AutomobilesDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BodyType> BodyType { get; set; }
        public virtual DbSet<Engine> Engine { get; set; }
        public virtual DbSet<ModelCar> ModelCar { get; set; }
        public virtual DbSet<ModelCarYear> ModelCarYear { get; set; }
        public virtual DbSet<PriceCategory> PriceCategory { get; set; }
        public virtual DbSet<YearOfIssue> YearOfIssue { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=user-pc\\sqlexpress; Database=AutomobilesDB; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BodyType>(entity =>
            {
                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Engine>(entity =>
            {
                entity.Property(e => e.EngineCapacity)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ModelCar>(entity =>
            {
                entity.Property(e => e.ModelName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdBodyNavigation)
                    .WithMany(p => p.ModelCar)
                    .HasForeignKey(d => d.IdBody)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModelCar_BodyType");

                entity.HasOne(d => d.IdEngineNavigation)
                    .WithMany(p => p.ModelCar)
                    .HasForeignKey(d => d.IdEngine)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModelCar_ModelCar");

                entity.HasOne(d => d.IdPriceNavigation)
                    .WithMany(p => p.ModelCar)
                    .HasForeignKey(d => d.IdPrice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModelCar_PriceCategory");
            });

            modelBuilder.Entity<ModelCarYear>(entity =>
            {
                entity.HasOne(d => d.IdCarNavigation)
                    .WithMany(p => p.ModelCarYear)
                    .HasForeignKey(d => d.IdCar)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModelCarYear_ModelCar");

                entity.HasOne(d => d.IdYearNavigation)
                    .WithMany(p => p.ModelCarYear)
                    .HasForeignKey(d => d.IdYear)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModelCarYear_YearOfIssue");
            });

            modelBuilder.Entity<PriceCategory>(entity =>
            {
                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
