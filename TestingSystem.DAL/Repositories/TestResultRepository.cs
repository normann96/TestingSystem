using System.Data.Entity;
using System.Threading.Tasks;
using TestingSystem.DAL.EF;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces.IRepository;
using TestingSystem.DAL.Repositories.Base;

namespace TestingSystem.DAL.Repositories
{
    public class TestResultRepository : BaseRepository<TestResult>, ITestResultRepository
    {
        public TestResultRepository(AppIdentityDbContext context) : base(context)
        {
        }

        public async Task<TestResult> GeTestResultAsync(string userId, int testId)
        {
            var testResult = await Set.Include(x => x.QuestionResults).FirstOrDefaultAsync(x => x.TestId == testId && x.UserId == userId);
            return testResult;
        }
    }
}