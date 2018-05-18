using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.DAL.Entities
{
    public class Question
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string QuestionContent { get; set; }

        [Required]
        public int Point { get; set; }

        [Required]
        public int TestId { get; set; }
        public virtual Test Test { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}