using Microsoft.AspNetCore.Mvc;
using Easystay.ViewModels; 
using System.Linq;
using test.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Easystay.Controllers
{
    public class AccountController : Controller
    {
        private readonly Csi402dbContext _db;

        public AccountController(Csi402dbContext db)
        {
            _db = db;
        }   

    [HttpPost]
    public IActionResult Register(User data)
{
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
                newUser.Role = "Customer";

                _db.Users.Add(newUser);
                _db.SaveChanges();

                return RedirectToAction("Userlist");
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
  
        // ==========================================
        // 1. ระบบเข้าสู่ระบบ (Login)
        // ==========================================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            return RedirectToAction("Index", "Home", new {email = model.Email});
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

        
        // ==========================================
        // 5. หน้าแสดงรายชื่อผู้ใช้งาน (Userlist)
        // ==========================================
       [HttpGet]
        public IActionResult Userlist()
        {
            var allUser = _db.Users.ToList();
            return View(allUser);
        }
    }
}