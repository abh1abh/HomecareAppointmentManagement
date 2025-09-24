using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.DAL;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new
InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(
    builder.Configuration["ConnectionStrings:AppDbContextConnection"]);
});

builder.Services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", p => p.RequireRole("Admin"));
    options.AddPolicy("IsHealthcareWorker", p => p.RequireRole("HealthcareWorker"));
    options.AddPolicy("IsClient", p => p.RequireRole("Client"));
});



builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IHealthcareWorkerRepository, HealthcareWorkerRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentTaskRepository, AppointmentTaskRepository>();
builder.Services.AddScoped<IAvailableSlotRepository, AvailableSlotRepository>();
builder.Services.AddScoped<IChangeLogRepository, ChangeLogRepository>();

builder.Services.AddRazorPages();
builder.Services.AddSession();


var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyy-MM-dd}.txt")
    .MinimumLevel.Information();

loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var source) &&
                            e.Level == LogEventLevel.Information && 
                            e.MessageTemplate.Text.Contains("Executing DbCommand"));

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    await DBInit.SeedAsync(app); // <-- seed roles & an admin

}

app.UseStaticFiles();

// Correct order:
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();
app.Run();
