using HomecareAppointmentManagment.Models;

namespace HomecareAppointmentManagment.DAL;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Clients.Any() && !context.HealthcareWorkers.Any())
        {
            // Clients
            var clients = new List<Client>
            {
                new Client { Name = "John Doe", Address = "123 Main St", Phone = "555-1234" },
                new Client { Name = "Jane Smith", Address = "456 Oak Ave", Phone = "555-5678" },
                new Client { Name = "Bob Johnson", Address = "789 Pine Rd", Phone = "555-9012" }
            };

            // Healthcare Workers
            var staff = new List<HealthcareWorker>
            {
                new HealthcareWorker { Name = "Alice Brown", Address = "12 Health St", Phone = "555-1111" },
                new HealthcareWorker { Name = "David Wilson", Address = "34 Care Ave", Phone = "555-2222" }
            };

            context.AddRange(clients);
            context.AddRange(staff);
            context.SaveChanges();

            // Appointments
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    ClientId = clients[0].ClientId,
                    HealthcareWorkerId = staff[0].HealthcareWorkerId,
                    Start = DateTime.Today.AddHours(9),
                    End = DateTime.Today.AddHours(10),
                    Notes = "Medication check and blood pressure",
                    AppointmentTasks = new List<AppointmentTask>
                    {
                        new AppointmentTask { Description = "Check blood pressure", IsCompleted = false },
                        new AppointmentTask { Description = "Administer medication", IsCompleted = false }
                    }
                },
                new Appointment
                {
                    ClientId = clients[1].ClientId,
                    HealthcareWorkerId = staff[1].HealthcareWorkerId,
                    Start = DateTime.Today.AddHours(11),
                    End = DateTime.Today.AddHours(12),
                    Notes = "Assistance with mobility exercises",
                    AppointmentTasks = new List<AppointmentTask>
                    {
                        new AppointmentTask { Description = "Help with walking exercises", IsCompleted = false }
                    }
                }
            };

            context.AddRange(appointments);
            context.SaveChanges();

            // Available slots for staff
            var slots = new List<AvailableSlot>
            {
                new AvailableSlot
                {
                    HealthcareWorkerId = staff[0].HealthcareWorkerId,
                    Start = DateTime.Today.AddHours(14),
                    End = DateTime.Today.AddHours(15),
                    IsBooked = false
                },
                new AvailableSlot
                {
                    HealthcareWorkerId = staff[1].HealthcareWorkerId,
                    Start = DateTime.Today.AddHours(15),
                    End = DateTime.Today.AddHours(16),
                    IsBooked = true
                }
            };

            context.AddRange(slots);
            context.SaveChanges();

            // Change logs
            var changeLogs = new List<ChangeLog>
            {
                new ChangeLog
                {
                    AppointmentId = appointments[0].Id,
                    ChangeDate = DateTime.Now,
                    ChangedByUserId = 1, // mock admin/user
                    ChangeDescription = "Updated notes for medication"
                },
                new ChangeLog
                {
                    AppointmentId = appointments[1].Id,
                    ChangeDate = DateTime.Now,
                    ChangedByUserId = 2,
                    ChangeDescription = "Rescheduled due to patient request"
                }
            };

            context.AddRange(changeLogs);
            context.SaveChanges();
        }
    }
}
