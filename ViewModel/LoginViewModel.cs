
using System.ComponentModel.DataAnnotations;
namespace CarsLab.ViewModel
{
    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Логін")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

}
