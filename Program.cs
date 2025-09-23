using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(
    builder.Configuration["ConnectionStrings:AppDbContextConnection"]);
});

builder.Services.AddScoped<IClientRepository, ClientRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();


app.MapDefaultControllerRoute();
app.Run();
