using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestingSystem.BLL.EntitiesDto;
using TestingSystem.BLL.Interfaces;
using TestingSystem.Constants;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.BLL.Services
{
    public class ResultService : IResultService
    {
        private ITestUnitOfWork Database { get; }
        public ResultService(ITestUnitOfWork uow)
        {
            this.Database = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public async Task CreateTestResult(string userId, int testId)
        {
            var testResult = await Database.TestResultRepository.GeTestResultAsync(userId, testId);
            if (testResult != null)
            {
                Database.TestResultRepository.Delete(testResult);
            }
            testResult = new TestResult
            {
                TestId = testId,
                QuestionResults = new List<QuestionResult>(),
                UserId = userId,
                SummaryResult = 0
            };
            Database.TestResultRepository.Create(testResult);
            await Database.SaveAsync();
        }

        public async Task SaveQuestionResult(string userId, int testId, int questionId, int[] answersId)
        {
            var question = await Database.QuestionRepository.GetByIdAsync(questionId);
            bool isTrueAnswer = false;
            
            foreach (var answer in question.Answers)
            {
                if (answer.IsTrue && answersId != null)
                        isTrueAnswer = answersId.All(i => i == answer.Id);
            }

            var testResult = await Database.TestResultRepository.GeTestResultAsync(userId, testId);
            if (testResult == null)
                await CreateTestResult(userId, testId);
           
            testResult.SummaryResult += isTrueAnswer ? question.Point : 0;

            var questionResult = new QuestionResult
            {
                TestResultId = testResult.Id,
                QuestionId = questionId,
                IsTrueAnswer = isTrueAnswer,
            };
            testResult.QuestionResults = testResult.QuestionResults ?? new List<QuestionResult>();
            testResult.QuestionResults.Add(questionResult);


            Database.QuestionResultRepository.Create(questionResult);
            Database.TestResultRepository.Update(testResult);
            await Database.SaveAsync();
        }

        public async Task<TestResultDto> GetTestResultAsync(string userId, int testId)
        {
            var testResult = await Database.TestResultRepository.GeTestResultAsync(userId, testId);
            var result = await CalculateResult(testId);
            var testResultDto = new TestResultDto
            {
                Id = testResult.Id,
                TestId = testResult.TestId,
                UserId = testResult.UserId,
                SummaryResult = testResult.SummaryResult,
                IsPassed = testResult.SummaryResult > result
            };
            return testResultDto;
        }

        private async Task<float> CalculateResult(int testId)
        {
            var questions = await Database.QuestionRepository.GetAllAsync(x => x.TestId == testId);
            return questions.Sum(x => x.Point) * TestPast.PastPercentage;
        }

        public void Dispose()
        {
            Database?.Dispose();
        }
    }
}