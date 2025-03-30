using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.AuthenticationDtos
{
    public class ResetPasswordDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và Xác nhận mật khẩu phải trùng nhau.")]
        public string ConfirmPassword { get; set; }
    }
}
