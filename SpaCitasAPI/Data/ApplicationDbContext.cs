using Microsoft.EntityFrameworkCore;
using SpaCitasAPI.Models; // Ajusta si tus modelos están en otro namespace

namespace SpaCitasAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Terapeuta> Terapeutas { get; set; }
        public DbSet<Cita> Citas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.PacienteId);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Servicio)
                .WithMany(s => s.Citas)
                .HasForeignKey(c => c.ServicioId);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Terapeuta)
                .WithMany(t => t.Citas)
                .HasForeignKey(c => c.TerapeutaId);
        }
    }
}
