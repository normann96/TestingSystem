using System;
using System.Threading.Tasks;
using TestingSystem.DAL.Interfaces.IRepository;

namespace TestingSystem.DAL.Interfaces
{
    public interface ITestUnitOfWork : IDisposable
    {
        ITestRepository TestRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IAnswerRepository AnswerRepository { get; }

        Task SaveAsync();
    }
}