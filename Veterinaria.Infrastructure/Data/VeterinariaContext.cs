using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;

namespace Veterinaria.Infrastructure.Data
{
    public class VeterinariaContext : DbContext
    {
        public VeterinariaContext(DbContextOptions<VeterinariaContext> options)
            : base(options) { }

        // ======= Tablas (DbSet) =======
        public DbSet<Dueno> Duenos { get; set; } = null!;
        public DbSet<Mascota> Mascotas { get; set; } = null!;
        public DbSet<Cita> Citas { get; set; } = null!;
        public DbSet<Veterinario> Veterinarios { get; set; } = null!;
        public DbSet<Servicio> Servicios { get; set; } = null!;

        // ======= Configuración del modelo =======
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // -------------------------
            // ENTIDAD: DUENO
            // -------------------------
            modelBuilder.Entity<Dueno>(entity =>
            {
                entity.ToTable("Duenos");

                entity.Property(p => p.Nombre)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(p => p.Telefono)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.HasIndex(p => p.Telefono)
                      .IsUnique();

                entity.Property(p => p.Direccion)
                      .HasMaxLength(200)
                      .IsRequired();
            });

            // -------------------------
            // ENTIDAD: MASCOTA
            // -------------------------
            modelBuilder.Entity<Mascota>(entity =>
            {
                entity.ToTable("Mascotas");

                entity.Property(p => p.Nombre)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(p => p.Especie)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(p => p.Raza)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.HasOne(p => p.Dueno)
                      .WithMany(d => d.Mascotas)
                      .HasForeignKey(p => p.DuenoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------------
            // ENTIDAD: VETERINARIO
            // -------------------------
            modelBuilder.Entity<Veterinario>(entity =>
            {
                entity.ToTable("Veterinarios");

                entity.Property(p => p.Nombre)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(p => p.Especialidad)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(p => p.Telefono)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.HasIndex(p => p.Telefono)
                      .IsUnique();
            });

            // -------------------------
            // ENTIDAD: SERVICIO
            // -------------------------
            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.ToTable("Servicios");

                entity.Property(p => p.Nombre)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(p => p.Descripcion)
                      .HasMaxLength(300);

                entity.Property(p => p.Precio)
                      .HasColumnType("decimal(10,2)");
            });

            // -------------------------
            // ENTIDAD: CITA
            // -------------------------
            modelBuilder.Entity<Cita>(entity =>
            {
                entity.ToTable("Citas");

                entity.Property(p => p.Motivo)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(p => p.Estado)
                      .HasMaxLength(50)
                      .HasDefaultValue("Pendiente");

                //Relacion con Mascota
                entity.HasOne(p => p.Mascota)
                      .WithMany(m => m.Citas)
                      .HasForeignKey(p => p.MascotaId)
                      .OnDelete(DeleteBehavior.Cascade);

                //Relacion con Veterinario
                entity.HasOne(p => p.Veterinario)
                      .WithMany(v => v.Citas)
                      .HasForeignKey(p => p.VeterinarioId)
                      .OnDelete(DeleteBehavior.Restrict);

                //Relacion con Servicio
                entity.HasOne(p => p.Servicio)
                      .WithMany(s => s.Citas)
                      .HasForeignKey(p => p.ServicioId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
