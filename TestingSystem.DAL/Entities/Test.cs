using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestingSystem.DAL.Entities
{
    public class Test
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string TestDescription { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}