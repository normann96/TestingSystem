using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.DAL.Entities
{
    public class TestResult
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int TestId { get; set; }

        public float SummaryResult { get; set; }

        public ICollection<QuestionResult> QuestionResults { get; set; }
    }
}