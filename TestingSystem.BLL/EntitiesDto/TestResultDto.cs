namespace TestingSystem.BLL.EntitiesDto
{
    public class TestResultDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TestId { get; set; }
        public float SummaryResult { get; set; }
        public bool IsPassed { get; set; }
    }
}