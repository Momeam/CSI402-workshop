using Microsoft.AspNetCore.Mvc;
using Easystay.ViewModels; 
using System.Collections.Generic; // 🟢 เพิ่มบรรทัดนี้เข้ามาเพื่อใช้งาน List

namespace Easystay.Controllers
{
    public class AccountController : Controller
    {
        // 🟢 1. สร้างฐานข้อมูลจำลอง (Static List) ไว้บนสุด เพื่อให้ทุกหน้าจอใช้ข้อมูลร่วมกัน
        private static List<UserViewModel> _userDatabase = new List<UserViewModel>
        {
            new UserViewModel { FullName = "สมศักดิ์ ใจดี", Email = "somsak@email.com", PhoneNumber = "081-111-1111", Role = "Admin", Password = "PassWord@123" },
            new UserViewModel { FullName = "วิไล มาดมั่น", Email = "wilai@email.com", PhoneNumber = "082-222-2222", Role = "Staff", Password = "Wilai_789" },
            new UserViewModel { FullName = "ใจรัก ดีงาม", Email = "jairuk@email.com", PhoneNumber = "083-333-3333", Role = "Customer", Password = "Customer001" }
        };

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
        // 2. ระบบสมัครสมาชิก (Register)
        // ==========================================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
          
            var newUser = new UserViewModel 
            {
                FullName = model.Fullname, 
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                Role = "Customer" 
            };
            _userDatabase.Add(newUser);

        
            return RedirectToAction("VerifyOtp", new { email = model.Email, name = model.Fullname });
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
        // 4. หน้าฟอร์มเพิ่มผู้ใช้งาน (AddUser) สำหรับ Admin
        // ==========================================
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(AddUserViewModel model)
        {
            // 🟢 4. ให้ฟอร์ม AddUser บันทึกข้อมูลลงตารางได้เหมือนกัน
            var newUser = new UserViewModel 
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                Role = model.Role 
            };
            _userDatabase.Add(newUser);

            return RedirectToAction("Userlist");
        }

        // ==========================================
        // 5. หน้าแสดงรายชื่อผู้ใช้งาน (Userlist)
        // ==========================================
       [HttpGet]
        public IActionResult Userlist()
        {
            // 🟢 5. โยนฐานข้อมูลจำลองทั้งหมด ไปโชว์ที่หน้าตาราง
            return View(_userDatabase);
        }
    }
}