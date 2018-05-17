using System.ComponentModel.DataAnnotations;

namespace TestingSystem.WEB.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Required field Login")]
        [Display(Name = "Login")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required field Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}