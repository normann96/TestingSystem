using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestingSystem.BLL.EntitiesDto;
using TestingSystem.BLL.Exceptions;
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
            if (questionDto == null)
                throw new ArgumentNullException(nameof(questionDto));

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

            try
            {
                var questResult = Database.QuestionRepository.Create(question);
                await Database.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                throw new TestException("Question not created. " + e.Message, e.InnerException);
            }
        }

        public async Task UpdateAsync(QuestionDto questionDto)
        {
            if (questionDto == null)
                throw new ArgumentNullException(nameof(questionDto));

            var question = await Database.QuestionRepository.GetByIdAsync(questionDto.Id);
            if (question == null)
                return;

            var newQuestion = new Question
            {
                Id = questionDto.Id,
                TestId = questionDto.TestId,
                Point = questionDto.Point,
                QuestionContent = questionDto.QuestionContent,
                Answers = new List<Answer>()
            };

            try
            {
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

                Database.QuestionRepository.Update(newQuestion);
                await Database.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                throw new TestException("Question not updated. " + e.Message, e.InnerException);
            }
        }

        public async Task DeleteAsync(int questionId)
        {
            var question = await Database.QuestionRepository.GetByIdAsync(questionId);
            if(question == null)
                throw new TestException("Question not found");

            try
            {
                var questionResult = Database.QuestionRepository.Delete(question);
                await Database.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                throw new TestException("Question not deleted. " + e.Message, e.InnerException);
            }
        }

        public async Task<AnswerDto> GetAnswerByIdAsync(int answerId)
        {
            var answer = await Database.AnswerRepository.GetByIdAsync(answerId);
            if (answer == null) return null;

            var answerDto = new AnswerDto
            {
                Id = answer.Id,
                AnswerContent = answer.AnswerContent,
                IsTrue = answer.IsTrue,
                QuestionId = answer.QuestionId
            };

            return answerDto;
        }

        public async Task DeleteAnswerAsync(int answerId)
        {
            var answer = await Database.AnswerRepository.GetByIdAsync(answerId);
            if(answer == null)
                throw new TestException("Answer not found");

            var answerResult = Database.AnswerRepository.Delete(answer);
            await Database.SaveAsync();
        }

        public async Task<QuestionDto> GetFirstByTestIdAsync(int testId)
        {
            var question = await Database.QuestionRepository.GetSingleAsync(x => x.TestId == testId);
            if (question == null)
                return null;

            var questionDto = new QuestionDto
            {
                Id = question.Id,
                QuestionContent = question.QuestionContent,
                TestId = question.TestId,
                Point = question.Point,
                Answers = new List<AnswerDto>()
            };
            foreach (var answer in question.Answers)
            {
                questionDto.Answers.Add(new AnswerDto
                {
                    Id = answer.Id,
                    QuestionId = answer.QuestionId,
                    AnswerContent = answer.AnswerContent,
                    IsTrue = answer.IsTrue
                });
            }
            return questionDto;
        }

        public async Task<QuestionDto> GetNextQuestion(int testId, int questionId)
        {
            var allQuestions = await Database.QuestionRepository.GetAllAsync(x => x.TestId == testId);
            if(allQuestions == null)
                throw new TestException("Questions not found");

            var questions = allQuestions.OrderBy(x => x.Id);
            var allQuestionsDto = new List<QuestionDto>();
            foreach (var question in questions)
            {
                var questionDto = new QuestionDto
                {
                    Id = question.Id,
                    QuestionContent = question.QuestionContent,
                    Point = question.Point,
                    TestId = question.TestId,
                    Answers = new List<AnswerDto>()
                };
                foreach (var answer in question.Answers)
                {
                    var answerDto = new AnswerDto
                    {
                        Id = answer.Id,
                        QuestionId = answer.QuestionId,
                        AnswerContent = answer.AnswerContent,
                        IsTrue = answer.IsTrue,
                    };
                    questionDto.Answers.Add(answerDto);
                }
                allQuestionsDto.Add(questionDto);
            }

            Queue<QuestionDto> queue = new Queue<QuestionDto>();
            allQuestionsDto.ForEach(x => queue.Enqueue(x));

            QuestionDto currQuest = null;
            while (queue.Count > 0)
            {
                if (questionId == 0)
                    break;

                currQuest = queue.Dequeue();
                if (currQuest.Id == questionId)
                {
                    currQuest = null;
                    break;
                }
            }

            if (queue.Count > 0)
                return queue.Dequeue();

            return currQuest;
        }

        #endregion
        public void Dispose()
        {
            Database?.Dispose();
        }
    }
}