using System.ComponentModel.DataAnnotations;

namespace EPIC.Entities.Dto.User
{
    public class CreateUserDtoBase
    {
        private string _displayName { get; set; }
        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        public string DisplayName { get => _displayName; set => _displayName = value?.Trim(); }

        private string _email { get; set; }
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get => _email; set => _email = value?.Trim(); }
    }

    public class CreateUserDto : CreateUserDtoBase
    {
        private string _userName { get; set; }
        [Required(ErrorMessage = "Tên tài khoản không được bỏ trống")]
        public string UserName { get => _userName; set => _userName = value?.Trim(); }

        private string _password { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        public string Password { get => _password; set => _password = value?.Trim(); }
    }
}
