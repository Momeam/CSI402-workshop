using Microsoft.EntityFrameworkCore;
using test.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<Csi402dbContext>(
    option => option.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"), // เพิ่มลูกน้ำตรงนี้
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")) // แก้คำว่า ServerVersion
    )
);


var app = builder.Build(); 

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Csi402dbContext>();
    db.Database.ExecuteSqlRaw("""
        CREATE TABLE IF NOT EXISTS `login_history` (
            `LoginHistoryID` int NOT NULL AUTO_INCREMENT,
            `UserID` int NULL,
            `Username` varchar(50) NOT NULL,
            `Email` varchar(100) NOT NULL,
            `LoggedInAtUTC` datetime(6) NOT NULL,
            PRIMARY KEY (`LoginHistoryID`),
            KEY `UserID` (`UserID`),
            KEY `LoggedInAtUTC` (`LoggedInAtUTC`)
        ) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
        """);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
