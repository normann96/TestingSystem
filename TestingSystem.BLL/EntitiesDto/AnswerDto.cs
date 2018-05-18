namespace TestingSystem.BLL.EntitiesDto
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string AnswerContent { get; set; }
        public bool IsTrue { get; set; }
        public int QuestionId { get; set; }
    }
}