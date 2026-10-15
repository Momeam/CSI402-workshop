using Microsoft.AspNetCore.Mvc;
using Easystay.ViewModels; 
using System.Linq;
using test.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace test.Controllers
{
    public class AccountController : Controller
    {
        private readonly Csi402dbContext _db;

        public AccountController(Csi402dbContext db)
        {
            _db = db;
        }   

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User data)
{
    data.Role = "Customer";
    ModelState.Remove(nameof(data.Role));

    // 1. เช็คว่ากรอกรหัสผ่านหรือไม่
    if(!string.IsNullOrEmpty(data.PasswordHash))
    {
        // 2. เช็คว่าข้อมูลในฟอร์มครบถ้วนและถูกต้องตาม Model หรือไม่
        if (ModelState.IsValid)
        {
            var checkExisting = _db.Users.FirstOrDefault(u => u.Email == data.Email || u.UserId == data.UserId);
            
            if (checkExisting == null)
            {
                User newUser = new User();
                newUser.UserId = data.UserId;
                newUser.PasswordHash = data.PasswordHash;
                newUser.Username = data.Username;
                newUser.Email = data.Email;
                newUser.Role = data.Role;

                _db.Users.Add(newUser);
                _db.SaveChanges();

                return RedirectToAction(nameof(Login));
            }
            else
            {
                ViewBag.Status = "อีเมลหรือชื่อผู้ใช้นี้ถูกใช้ไปแล้ว";
                return View(data);
            }
        }
        else
        {
            
            ViewBag.Status = "กรุณากรอกข้อมูลให้ครบถ้วน"; 
            return View(data);
        }
    } 
    else
    {
       
        ViewBag.Status = "กรุณากรอกรหัสผ่าน (Password Null)"; 
        return View(data); 
    }
}
  
      
        // 1. ระบบเข้าสู่ระบบ (Login)
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _db.Users.FirstOrDefault(u =>
                u.Email == model.Email && u.PasswordHash == model.Password);

            if (user == null)
            {
                ViewBag.Status = "อีเมลหรือรหัสผ่านไม่ถูกต้อง";
                return View(model);
            }

            _db.LoginHistories.Add(new LoginHistory
            {
                UserId = user.UserId,
                Username = user.Username ?? string.Empty,
                Email = user.Email ?? string.Empty,
                LoggedInAtUtc = DateTime.UtcNow
            });
            _db.SaveChanges();

            HttpContext.Session.SetString("LoggedInUserId", user.UserId?.ToString() ?? string.Empty);
            return RedirectToAction(nameof(LoginHistory));
        }

        
        // ==========================================
        // 3. ระบบยืนยัน OTP (VerifyOtp)
        // ==========================================
        [HttpGet]
        public IActionResult VerifyOtp(string email, string name)
        {
           ViewBag.UserEmail = email;
           ViewBag.UserName = name;
           
            var model = new VerifyOtpViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        public IActionResult VerifyOtp(VerifyOtpViewModel model)
        {
            return RedirectToAction("Login");
        }

        
     
        // 5. หน้าแสดงรายชื่อผู้ใช้งาน (Userlist)
        
       [HttpGet]
        public IActionResult Userlist()
        {
            var userId = HttpContext.Session.GetString("LoggedInUserId");
            if (!int.TryParse(userId, out _))
            {
                return RedirectToAction(nameof(Login));
            }

            var allUser = _db.Users.ToList();
            return View(allUser);
        }

        [HttpGet]
        public IActionResult LoginHistory()
        {
            var userId = HttpContext.Session.GetString("LoggedInUserId");
            if (!int.TryParse(userId, out _))
            {
                return RedirectToAction(nameof(Login));
            }

            var loginHistory = _db.LoginHistories
                .OrderByDescending(entry => entry.LoggedInAtUtc)
                .ToList();

            return View(loginHistory);
        }
    }
}