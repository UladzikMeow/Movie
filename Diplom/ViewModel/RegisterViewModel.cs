using System.ComponentModel.DataAnnotations;

namespace Diplom.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Не указано имя пользователя")]
        [Display(Name = "Имя пользователя")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }
    }
}
