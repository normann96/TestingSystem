using System.ComponentModel.DataAnnotations;

namespace TestingSystem.WEB.Models.Roles
{
    public class RoleEditViewModel
    {
        [Required(ErrorMessage = "Required field RoleName")]
        public string RoleName { get; set; }

        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}