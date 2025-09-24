using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.DAL;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(
    builder.Configuration["ConnectionStrings:AppDbContextConnection"]);
});

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IHealthcarePersonnelRepository, HealthcarePersonnelRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentTaskRepository, AppointmentTaskRepository>();
builder.Services.AddScoped<IAvailableSlotRepository, AvailableSlotRepository>();
builder.Services.AddScoped<IChangeLogRepository, ChangeLogRepository>();

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
    DBInit.Seed(app);
}

app.UseStaticFiles();


app.MapDefaultControllerRoute();
app.Run();
