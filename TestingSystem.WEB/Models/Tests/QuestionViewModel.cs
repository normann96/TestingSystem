using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.WEB.Models.Tests
{
    public class QuestionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Question:")]
        [Required(ErrorMessage = "Please enter a question content")]
        [StringLength(500, MinimumLength = 1)]
        public string QuestionContent { get; set; }

        [Required]
        [Display(Name = "Point:")]
        [Range(1, 10, ErrorMessage = "The number must be in the range from 1 to 10")]
        public int Point { get; set; }

        [Required]
        public int TestId { get; set; }

        public virtual IList<AnswerViewModel> Answers { get; set; }
    }
}