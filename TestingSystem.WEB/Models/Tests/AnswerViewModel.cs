using System.ComponentModel.DataAnnotations;

namespace TestingSystem.WEB.Models.Tests
{
    public class AnswerViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string AnswerContent { get; set; }
        [Required]
        public bool IsTrue { get; set; }
        [Required]
        public int QuestionId { get; set; }
    }
}