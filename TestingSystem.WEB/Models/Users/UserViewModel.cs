using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.WEB.Models.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Name")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}