using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using test.Models;


namespace test.Controllers;

public class HomeController : Controller
{
    private readonly Csi402dbContext _db;
    public HomeController (Csi402dbContext db)
    {
        _db = db;
    }
    public IActionResult Lab9()
    {
        //Create ||
        return View();
    }
[HttpPost]
public IActionResult Lab9(User data)
{
    // --- เปลี่ยนเงื่อนไขบรรทัดนี้ ---
    if(!string.IsNullOrEmpty(data.PasswordHash)) 
    {
        if(ModelState.IsValid)
        {
            var CheckStd = (from a in _db.Users where a.UserId == data.UserId select a).FirstOrDefault();
            
            User s = new User();
            s.UserId = data.UserId;
            s.PasswordHash = data.PasswordHash;
            s.Username = data.Username;
            s.Email = data.Email;
            s.Role = "Customer"; 

            _db.Users.Add(s);
            _db.SaveChanges();
            
            return View(data);
        }
        else
        {
            ViewBag.Status = "Error";
            return View(data);
        }
    }
    else
    {
        ViewBag.Status = "Password Null";
        return View(data);
    }
}
public IActionResult Lab9_1()
{
    var Student = (from s in _db.Users select s).ToList();
    return View(Student);    
}
    public IActionResult Index()
    {
        var userId = HttpContext.Session.GetString("LoggedInUserId");
        if (!int.TryParse(userId, out var parsedUserId))
        {
            return View();
        }

        var user = _db.Users.FirstOrDefault(u => u.UserId == parsedUserId);

        return View(user);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult FlightBooking()
{
    return View();
}
public IActionResult EmployeeDashboard()
    {
        return View();
    }


}
