using Microsoft.EntityFrameworkCore;
using test.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Csi402dbContext>(
    option => option.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"), // เพิ่มลูกน้ำตรงนี้
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")) // แก้คำว่า ServerVersion
    )
);


var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
