using System.Collections.Generic;

namespace TestingSystem.BLL.EntitiesDto
{
    public class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TestDescription { get; set; }
        public ICollection<QuestionDto> Questions { get; set; }
    }
}