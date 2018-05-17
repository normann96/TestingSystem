using System.ComponentModel.DataAnnotations;

namespace TestingSystem.WEB.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}