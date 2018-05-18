using System.ComponentModel.DataAnnotations;

namespace TestingSystem.DAL.Entities
{
    public class Answer
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string AnswerContent { get; set; }

        [Required]
        public bool IsTrue { get; set; }

        [Required]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}