using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestingSystem.BLL.EntitiesDto;
using TestingSystem.BLL.Interfaces;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.BLL.Services
{
    public class QuestionService : IQuestionService
    {
        private ITestUnitOfWork Database { get; }
        public QuestionService(ITestUnitOfWork uow)
        {
            this.Database = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        #region IQuestionService members

        public async Task<QuestionDto> GetByIdAsync(int questionId)
        {
            var question = await Database.QuestionRepository.GetByIdAsync(questionId);
            if (question == null) return null;

            var questionDto = new QuestionDto
            {
                Id = question.Id,
                TestId = question.TestId,
                Point = question.Point,
                QuestionContent = question.QuestionContent,
                Answers = new List<AnswerDto>(),
            };
            foreach (var answer in question.Answers)
            {
                questionDto.Answers.Add(
                    new AnswerDto
                    {
                        Id = answer.Id,
                        AnswerContent = answer.AnswerContent,
                        IsTrue = answer.IsTrue,
                        QuestionId = answer.QuestionId
                    });
            }

            return questionDto;
        }

        public async Task CreateAsync(QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = questionDto.Id,
                TestId = questionDto.TestId,
                Point = questionDto.Point,
                QuestionContent = questionDto.QuestionContent,
                Answers = new List<Answer>()
            };
            foreach (var answer in questionDto.Answers)
            {
                question.Answers.Add(new Answer
                {
                    Id = answer.Id,
                    AnswerContent = answer.AnswerContent,
                    IsTrue = answer.IsTrue,
                    QuestionId = answer.QuestionId,
                });
            }
            var questResult = Database.QuestionRepository.Create(question);
            await Database.SaveAsync();
        }

        public async Task UpdateAsync(QuestionDto questionDto)
        {
            var question = await Database.QuestionRepository.GetByIdAsync(questionDto.Id);
            if (question == null) return;

            var newQuestion = new Question
            {
                Id = questionDto.Id,
                TestId = questionDto.TestId,
                Point = questionDto.Point,
                QuestionContent = questionDto.QuestionContent,
                Answers = new List<Answer>()
            };
            foreach (var answer in questionDto.Answers)
            {
                var newAnswer = new Answer
                {
                    Id = answer.Id,
                    AnswerContent = answer.AnswerContent,
                    IsTrue = answer.IsTrue,
                    QuestionId = answer.QuestionId,
                    Question = question
                };
                newQuestion.Answers.Add(newAnswer);
                Database.AnswerRepository.Update(newAnswer);
            }

            Database.QuestionRepository.Update(question);
            await Database.SaveAsync();
        }

        public async Task DeleteAsync(int questionId)
        {
            var question = await Database.QuestionRepository.GetByIdAsync(questionId);
            var questionResult = Database.QuestionRepository.Delete(question);
            await Database.SaveAsync();
        }

        #endregion
        public void Dispose()
        {
            Database?.Dispose();
        }
    }
}