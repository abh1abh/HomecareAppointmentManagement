using HomecareAppointmentManagement.DAL;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new
InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

builder.Services.AddControllersWithViews();

// Db
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(
    builder.Configuration["ConnectionStrings:AppDbContextConnection"]);
});

builder.Services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

// Roles for RBAC 
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

// logging 
var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log")
    .MinimumLevel.Information();

loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                            e.Level == LogEventLevel.Information &&
                            e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    await DBInit.SeedAsync(app); // Seeds with users and roles in db

}

app.UseStaticFiles();


app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();
app.Run();
