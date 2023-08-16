using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class CreateManagerInvestorDto
    {
        private string _username { get; set; }
        private string _displayName { get; set; }
        private string _phone { get; set; }
        private string _email { get; set; }
        private string _address { get; set; }
        private string _password { get; set; }

        [Required(ErrorMessage = "Username không được bỏ trống")]
        public string Username { get => _username; set => _username = value?.Trim(); }
        [Required(ErrorMessage = "Tên hiển thị không được bỏ trống")]
        public string DisplayName { get => _displayName; set => _displayName = value?.Trim(); }
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Phone { get => _phone; set => _phone = value?.Trim(); }
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [Required(ErrorMessage = "Địa chỉ email không được bỏ trống")]
        public string Email { get => _email; set => _email = value?.Trim(); }
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        public string Password { get => _password; set => _password = value?.Trim(); }
        public string Address { get => _address; set => _address = value?.Trim(); }
    }
}
