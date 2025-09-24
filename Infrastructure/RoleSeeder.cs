// using System.Security.Claims;
// using HomecareAppointmentManagment.DAL;
// using HomecareAppointmentManagment.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;

// namespace HomecareAppointmentManagment.Infrastructure;

// public static class RoleSeeder
// {
//     private static readonly string[] Roles = { "Admin", "HealthcareWorker", "Client" };

//     public static async Task SeedAsync(IServiceProvider sp)
//     {
//         using var scope = sp.CreateScope();
//         var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//         var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
//         var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//         // 1) Ensure roles
//         foreach (var r in Roles)
//         {
//             if (!await roleMgr.RoleExistsAsync(r))
//                 await roleMgr.CreateAsync(new IdentityRole(r));
//         }

//         // 2) Admin
//         var adminEmail = "admin@homecare.local";
//         var admin = await userMgr.FindByEmailAsync(adminEmail);
//         if (admin is null)
//         {
//             admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
//             await userMgr.CreateAsync(admin, "Admin123!");
//             await userMgr.AddToRoleAsync(admin, "Admin");
//         }

//         // 3) HealthcareWorker user + domain entity + claim
//         var workerEmail = "nurse1@homecare.local";
//         var workerUser = await userMgr.FindByEmailAsync(workerEmail);
//         if (workerUser is null)
//         {
//             workerUser = new IdentityUser { UserName = workerEmail, Email = workerEmail, EmailConfirmed = true };
//             await userMgr.CreateAsync(workerUser, "Nurse123!");
//             await userMgr.AddToRoleAsync(workerUser, "HealthcareWorker");
//         }

//         // Link to domain HealthcareWorker row
//         var worker = await db.HealthcareWorkers
//             .FirstOrDefaultAsync(w => w.IdentityUserId == workerUser.Id);

//         if (worker is null)
//         {
//             worker = new HealthcareWorker
//             {
//                 Name = "Nurse One",
//                 Address = "123 Main St",
//                 Phone = "12345678",
//                 Email = workerEmail,
//                 IdentityUserId = workerUser.Id
//             };
//             db.HealthcareWorkers.Add(worker);
//             await db.SaveChangesAsync();
//         }

//         // Ensure claim HealthcareWorkerId=<pk>
//         var claims = await userMgr.GetClaimsAsync(workerUser);
//         if (!claims.Any(c => c.Type == "HealthcareWorkerId"))
//         {
//             await userMgr.AddClaimAsync(
//                 workerUser,
//                 new Claim("HealthcareWorkerId", worker.HealthcareWorkerId.ToString())
//             );
//         }

//         // 4) Optional client user
//         var clientEmail = "client1@homecare.local";
//         var clientUser = await userMgr.FindByEmailAsync(clientEmail);
//         if (clientUser is null)
//         {
//             clientUser = new IdentityUser { UserName = clientEmail, Email = clientEmail, EmailConfirmed = true };
//             await userMgr.CreateAsync(clientUser, "Client123!");
//             await userMgr.AddToRoleAsync(clientUser, "Client");
//         }

//         // 5) Seed a couple of demo slots for the worker
//         if (!await db.AvailableSlots.AnyAsync(s => s.HealthcareWorkerId == worker.HealthcareWorkerId))
//         {
//             db.AvailableSlots.AddRange(
//                 new AvailableSlot
//                 {
//                     HealthcareWorkerId = worker.HealthcareWorkerId,
//                     StartTime = DateTime.UtcNow.AddHours(2),
//                     DurationMinutes = 45,
//                     Capacity = 1,
//                     VisitType = "Home"
//                 },
//                 new AvailableSlot
//                 {
//                     HealthcareWorkerId = worker.HealthcareWorkerId,
//                     StartTime = DateTime.UtcNow.AddDays(1).AddHours(3),
//                     DurationMinutes = 30,
//                     Capacity = 1,
//                     VisitType = "Telehealth"
//                 }
//             );
//             await db.SaveChangesAsync();
//         }
//     }
// }
