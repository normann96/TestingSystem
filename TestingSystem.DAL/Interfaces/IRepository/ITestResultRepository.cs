using System.Threading.Tasks;
using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL.Interfaces.IRepository
{
    public interface ITestResultRepository : IRepository<TestResult>
    {
        Task<TestResult> GeTestResultAsync(string userId, int testId);
    }
}