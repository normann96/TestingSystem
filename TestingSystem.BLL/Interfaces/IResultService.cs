using System;
using System.Threading.Tasks;
using TestingSystem.BLL.EntitiesDto;

namespace TestingSystem.BLL.Interfaces
{
    public interface IResultService : IDisposable
    {
        Task<TestResultDto> GetTestResultAsync(string userId, int testId);
        Task CreateTestResult(string userId, int testId);
        Task SaveQuestionResult(string userId, int testId, int questionId, int[] answersId);
    }
}