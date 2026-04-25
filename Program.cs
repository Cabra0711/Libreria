using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Services;
using Proyecto.Services.Interfaces;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<IUserServices, UserService>();
builder.Services.AddScoped<IBookServices, BookService>();
builder.Services.AddScoped<ILoanServices, LoansService>();

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<MySqlDbContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles(); 

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=User}/{action=Books}/{id?}")
    .WithStaticAssets();

app.Run();