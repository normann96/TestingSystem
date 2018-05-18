using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.WEB.Models.Tests
{
    public class TestViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field Name")]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required field Description")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters", MinimumLength = 3)]
        [Display(Name = "Description")]
        public string TestDescription { get; set; }

        public IEnumerable<QuestionViewModel> Questions { get; set; }
    }
}