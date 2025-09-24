
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagment.DAL
{
    public class AppDbContext : DbContext
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
    }
}