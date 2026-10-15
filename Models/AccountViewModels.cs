using System.ComponentModel.DataAnnotations;

namespace Easystay.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "กรุณากรอกอีกมเล")]
        [EmailAddress(ErrorMessage =  "รูปแบบอีเมลไม่ถูกต้อง")]
        public string? Email {get; set;}
        [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
        public string? Password {get; set;}
        public bool RememberMe {get; set;} 
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "กรุณากรอกชืื่อ-นามสกุล")]
        public string? Fullname {get; set;}

        [Required(ErrorMessage = "กรุกณากรอกอีกเมล")]
        [EmailAddress]
        public string? Email {get; set;}

        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
        public string? PhoneNumber {get; set;}

        [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
        public string? Password {get; set;}

        [Required(ErrorMessage = "กรุณายืนยันรหัสผ่าน")]
        [Compare("Password", ErrorMessage = "รหัสผ่านไม่ตรงกัน")]
        public string? ConfirmPassword {get; set;}
    }

    public class VerifyOtpViewModel
    {
        public string? Email {get; set;}

        [Required(ErrorMessage = "กรุณากรอกรหัส OTP")]
        [StringLength(6, MinimumLength = 6, ErrorMessage ="รหัสOTP ต้องมี6หลัก")]
        public string? OtpCode {get; set;}
    }

    // สำหรับหน้าฟอร์ม AddUser
    public class AddUserViewModel
    {
        [Required(ErrorMessage = "กรุณากรอกชื่อ-นามสกุล")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "กรุณากรอกอีเมล")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "กรุณาตั้งรหัสผ่าน")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกสิทธิ์ผู้ใช้งาน")]
        public string? Role { get; set; } // เช่น Admin, Staff, Customer
    }

    // สำหรับใช้แสดงข้อมูลในตารางหน้า Userlist
    public class UserViewModel
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
    }
}

