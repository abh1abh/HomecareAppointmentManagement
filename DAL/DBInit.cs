using System.Security.Claims;
using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagment.DAL;

public static class DBInit
{
    public static async Task SeedAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AppDbContext>();
        var userMgr = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

        // For dev, resets db each time
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Roles for RBAC
        var roles = new[] { "Admin", "HealthcareWorker", "Client" };
        foreach (var r in roles)
            if (!await roleMgr.RoleExistsAsync(r))
                await roleMgr.CreateAsync(new IdentityRole(r));

        // Admin user
        var adminEmail = "admin@homecare.local";
        var adminUser = await userMgr.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userMgr.CreateAsync(adminUser, "Admin123!");
            await userMgr.AddToRoleAsync(adminUser, "Admin");
        }


        // Init of Clients with IdentityUser link
        var clientSeeds = new[]
        {
            new { Name="John Doe",  Address="123 Main St",  Phone="555-1234", Email="john@homecare.local",  Password="Client123!" },
            new { Name="Jane Smith",Address="456 Oak Ave", Phone="555-5678", Email="jane@homecare.local",  Password="Client123!" },
            new { Name="Bob Johnson",Address="789 Pine Rd",Phone="555-9012", Email="bob@homecare.local",   Password="Client123!" }
        };

        var clients = new List<Client>();

        foreach (var c in clientSeeds)
        {
            // 1) Identity user
            var user = await userMgr.FindByEmailAsync(c.Email);
            if (user is null)
            {
                user = new IdentityUser { UserName = c.Email, Email = c.Email, EmailConfirmed = true };
                await userMgr.CreateAsync(user, c.Password);
                await userMgr.AddToRoleAsync(user, "Client");
            }

            // 2) Domain client row linked via IdentityUserId
            var client = await context.Clients.FirstOrDefaultAsync(cl => cl.IdentityUserId == user.Id);
            if (client is null)
            {
                client = new Client
                {
                    Name = c.Name,
                    Address = c.Address,
                    Phone = c.Phone,
                    Email = c.Email,
                    IdentityUserId = user.Id
                };
                context.Clients.Add(client);
                await context.SaveChangesAsync();
            }

            // 3) Ensure claim: ClientId
            var existingClaims = await userMgr.GetClaimsAsync(user);
            if (!existingClaims.Any(cc => cc.Type == "ClientId"))
            {
                await userMgr.AddClaimAsync(user, new Claim("ClientId", client.ClientId.ToString()));
            }

            clients.Add(client);
        }


        // Init of HealthcareWorkers with IdentityUser link and claim
        var workerSeed = new[]
        {
            new { Name="Alice Brown", Address="12 Health St", Phone="555-1111", Email="alice@homecare.local", Password="Nurse123!" },
            new { Name="David Wilson", Address="34 Care Ave",  Phone="555-2222", Email="david@homecare.local", Password="Nurse123!" }
        };

        var workers = new List<HealthcareWorker>();

        foreach (var w in workerSeed)
        {
            // 1) Identity user
            var user = await userMgr.FindByEmailAsync(w.Email);
            if (user is null)
            {
                user = new IdentityUser { UserName = w.Email, Email = w.Email, EmailConfirmed = true };
                await userMgr.CreateAsync(user, w.Password);
                await userMgr.AddToRoleAsync(user, "HealthcareWorker");
            }

            // 2) Domain worker row linked via IdentityUserId (if not existing)
            var worker = await context.HealthcareWorkers.FirstOrDefaultAsync(hw => hw.IdentityUserId == user.Id);
            if (worker is null)
            {
                worker = new HealthcareWorker
                {
                    Name = w.Name,
                    Address = w.Address,
                    Phone = w.Phone,
                    Email = w.Email,
                    IdentityUserId = user.Id
                };
                context.HealthcareWorkers.Add(worker);
                await context.SaveChangesAsync();
            }

            // 3) Ensure claim: HealthcareWorkerId
            var claims = await userMgr.GetClaimsAsync(user);
            if (!claims.Any(c => c.Type == "HealthcareWorkerId"))
            {
                await userMgr.AddClaimAsync(user, new Claim("HealthcareWorkerId", worker.HealthcareWorkerId.ToString()));
            }

            workers.Add(worker);
        }

        // Init Appointments, AvailableSlots, ChangeLogs
        // 1) Create slots
        var slots = new List<AvailableSlot>
        {
            new AvailableSlot
            {
                HealthcareWorkerId = workers[0].HealthcareWorkerId,
                Start = new DateTime(2025, 12, 9, 14, 0, 0), // Dec 9, 2025 at 14:00
                End   = new DateTime(2025, 12, 9, 15, 0, 0), // Dec 9, 2025 at 15:00
                IsBooked = false
            },
            new AvailableSlot
            {
                HealthcareWorkerId = workers[1].HealthcareWorkerId,
                Start = new DateTime(2025, 12, 10, 14, 0, 0), // Dec 10, 2025 at 14:00
                End   = new DateTime(2025, 12, 10, 15, 0, 0), // Dec 10, 2025 at 15:00
                IsBooked = false // set false for now; will flip after linking
            }
        };
        context.AvailableSlots.AddRange(slots);
        await context.SaveChangesAsync();

        // 2) Create appointments and LINK BOTH
        var appts = new List<Appointment>
        {
            new Appointment
            {
                ClientId = clients[1].ClientId,
                HealthcareWorkerId = workers[1].HealthcareWorkerId,
                Start = slots[1].Start,
                End   = slots[1].End,
                Notes = "Assistance with mobility exercises",
                AvailableSlot = slots[1],
                AppointmentTasks = new List<AppointmentTask>
                {
                    new AppointmentTask { Description = "Help with walking exercises" }
                }
            }
        };
        context.Appointments.AddRange(appts);
        await context.SaveChangesAsync();

        // 3) Mark the linked slots as booked (optional convenience flag)
        slots[0].IsBooked = false;
        slots[1].IsBooked = true;
        await context.SaveChangesAsync();

        // 4) Change logs (safe now that appt IDs exist)
        context.ChangeLogs.AddRange(
            new ChangeLog { AppointmentId = appts[0].Id, ChangeDate = DateTime.Now, ChangedByUserId = 2, ChangeDescription = "Rescheduled due to patient request" }
        );
        await context.SaveChangesAsync();


    }
}
