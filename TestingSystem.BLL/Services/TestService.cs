using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestingSystem.BLL.EntitiesDto;
using TestingSystem.BLL.Interfaces;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.BLL.Services
{
    public class TestService : ITestService
    {
        private ITestUnitOfWork Database { get; }

        public TestService(ITestUnitOfWork uow)
        {
            this.Database = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        #region ITestService members

        public async Task<TestDto> GetByIdAsync(int? testId)
        {
            var test = await Database.TestRepository.GetByIdAsync(testId);
            if (test == null)
                return null;

            var testDto = new TestDto
            {
                Id = test.Id,
                Name = test.Name,
                TestDescription = test.TestDescription,
                Questions = new List<QuestionDto>()
            };

            foreach (var question in test.Questions)
            {
                var newQuest = new QuestionDto
                {
                    Id = question.Id,
                    QuestionContent = question.QuestionContent,
                    Point = question.Point,
                    TestId = question.TestId,
                    Answers = new List<AnswerDto>()
                };
                testDto.Questions.Add(newQuest);
            }
            return testDto;
        }

        public async Task<List<TestDto>> GetAllAsync()
        {
            var tests = await Database.TestRepository.GetAllAsync();
            var testDtos = new List<TestDto>();
            foreach (var test in tests)
            {
                testDtos.Add(new TestDto
                {
                    Id = test.Id,
                    Name = test.Name,
                    TestDescription = test.TestDescription,
                });
            }
            return testDtos;
        }

        public async Task CreateAsync(TestDto testDto)
        {
            var test = new Test
            {
                Name = testDto.Name,
                TestDescription = testDto.TestDescription,
            };
            var testResult = Database.TestRepository.Create(test);
            await Database.SaveAsync();
        }

        public async Task UpdateAsync(TestDto testDto)
        {
            var test = await Database.TestRepository.GetByIdAsync(testDto.Id);
            if (test == null) return;

            test.Name = testDto.Name;
            test.TestDescription = testDto.TestDescription;

            Database.TestRepository.Update(test);
            await Database.SaveAsync();
        }

        public async Task DeleteAsync(int testId)
        {
            var test = await Database.TestRepository.GetByIdAsync(testId);
            if (test == null)
                return;

            var testResult = Database.TestRepository.Delete(test);
            await Database.SaveAsync();
        }
        #endregion

        public void Dispose()
        {
            Database?.Dispose();
        }
    }
}