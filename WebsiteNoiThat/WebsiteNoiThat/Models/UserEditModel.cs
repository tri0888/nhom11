using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteNoiThat.Models
{
    public class UserEditModel
    {
        public int UserId { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Yêu cầu nhập họ tên")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(100)]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "Độ dài mật khẩu từ 4-32 ký tự")]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu mới")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }

        // Hidden properties
        public string Username { get; set; }
        public string GroupId { get; set; }
        public bool Status { get; set; }
    }
}
