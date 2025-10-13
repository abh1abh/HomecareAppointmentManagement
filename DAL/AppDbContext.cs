
using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagment.DAL
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Database.EnsureCreated();
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<HealthcareWorker> HealthcareWorkers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentTask> AppointmentTasks { get; set; }
        public DbSet<AvailableSlot> AvailableSlots { get; set; }

        public DbSet<ChangeLog> ChangeLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // keep Identity mappings

            // Keep ChangeLogs even when the Appointment row is deleted:
            modelBuilder.Entity<ChangeLog>()
                .HasOne(c => c.Appointment)
                .WithMany() 
                .HasForeignKey(c => c.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull); // Set FK to null on Appointment deletion
        }
    }
}