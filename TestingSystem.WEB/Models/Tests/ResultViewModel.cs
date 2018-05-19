namespace TestingSystem.WEB.Models.Tests
{
    public class ResultViewModel
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public string UserName { get; set; }
        public double Score { get; set; }
        public bool IsPassed { get; set; }
    }
}