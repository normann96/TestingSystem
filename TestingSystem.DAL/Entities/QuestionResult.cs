using System.ComponentModel.DataAnnotations;

namespace TestingSystem.DAL.Entities
{
    public class QuestionResult
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int TestResultId { get; set; }
        public TestResult TestResult { get; set; }

        [Required]
        public bool IsTrueAnswer { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}