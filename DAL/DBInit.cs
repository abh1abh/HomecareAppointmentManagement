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

        if (!context.Clients.Any())
        {
            var clients = new List<Client>
            {
                new Client { Name = "John Doe", Address = "123 Main St", Phone = "555-1234" },
                new Client { Name = "Jane Smith", Address = "456 Oak Ave", Phone = "555-5678" },
                new Client { Name = "Bob Johnson", Address = "789 Pine Rd", Phone = "555-9012" }
            };
    
            context.AddRange(clients);
            context.SaveChanges();
        }

        
    }
}