using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using Microsoft.EntityFrameworkCore;

namespace AnalisisOpiniones.Data.Persistence.Context
{
    public class DwhContext : DbContext
    {
        public DwhContext(DbContextOptions<DwhContext> options) : base(options)
        {
        }

        public DbSet<DimCliente> DimCliente { get; set; }
        public DbSet<DimProducto> DimProducto { get; set; }
        public DbSet<DimFuente> DimFuente { get; set; }
        public DbSet<DimFecha> DimFecha { get; set; }
        public DbSet<DimSentimiento> DimSentimiento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureDimCliente(modelBuilder);
            ConfigureDimProducto(modelBuilder);
            ConfigureDimFuente(modelBuilder);
            ConfigureDimFecha(modelBuilder);
            ConfigureDimSentimiento(modelBuilder);
        }

        private static void ConfigureDimCliente(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DimCliente>(entity =>
            {
                entity.ToTable("DimCliente");

                entity.HasKey(x => x.ClienteKey);

                entity.Property(x => x.ClienteKey)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.IdCliente)
                    .IsRequired();

                entity.Property(x => x.Nombre)
                    .HasMaxLength(200);

                entity.Property(x => x.Email)
                    .HasMaxLength(200);

                entity.HasIndex(x => x.IdCliente)
                    .IsUnique();
            });
        }

        private static void ConfigureDimProducto(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DimProducto>(entity =>
            {
                entity.ToTable("DimProducto");

                entity.HasKey(x => x.ProductoKey);

                entity.Property(x => x.ProductoKey)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.IdProducto)
                    .IsRequired();

                entity.Property(x => x.Nombre)
                    .HasMaxLength(200);

                entity.Property(x => x.Categoria)
                    .HasMaxLength(150);

                entity.HasIndex(x => x.IdProducto)
                    .IsUnique();
            });
        }

        private static void ConfigureDimFuente(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DimFuente>(entity =>
            {
                entity.ToTable("DimFuente");

                entity.HasKey(x => x.FuenteKey);

                entity.Property(x => x.FuenteKey)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.IdFuente)
                    .IsRequired()
                    .HasMaxLength(20); 

                entity.Property(x => x.TipoFuente)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.FechaCarga)
                    .IsRequired();

                entity.HasIndex(x => x.IdFuente)
                    .IsUnique();
            });
        }

        private static void ConfigureDimFecha(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DimFecha>(entity =>
            {
                entity.ToTable("DimFecha");

                entity.HasKey(x => x.FechaKey);

                entity.Property(x => x.FechaKey)
                    .ValueGeneratedNever();

                entity.Property(x => x.Fecha)
                    .IsRequired();

                entity.Property(x => x.NombreMes)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasIndex(x => x.Fecha)
                    .IsUnique();
            });
        }

        private static void ConfigureDimSentimiento(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DimSentimiento>(entity =>
            {
                entity.ToTable("DimSentimiento");

                entity.HasKey(x => x.SentimientoKey);

                entity.Property(x => x.SentimientoKey)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.Clasificacion)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasIndex(x => x.Clasificacion)
                    .IsUnique();
            });
        }
    }
}