using System.Collections.Generic;

namespace TestingSystem.BLL.EntitiesDto
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public int Point { get; set; }
        public int TestId { get; set; }
        public virtual ICollection<AnswerDto> Answers { get; set; }
    }
}