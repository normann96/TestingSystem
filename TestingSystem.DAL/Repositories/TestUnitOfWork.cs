using System;
using System.Threading.Tasks;
using TestingSystem.DAL.EF;
using TestingSystem.DAL.Interfaces;
using TestingSystem.DAL.Interfaces.IRepository;

namespace TestingSystem.DAL.Repositories
{
    public class TestUnitOfWork : ITestUnitOfWork
    {
        private readonly AppIdentityDbContext _db;
        public ITestRepository TestRepository { get; private set; }
        public IQuestionRepository QuestionRepository { get; private set; }
        public IAnswerRepository AnswerRepository { get; private set; }

        public TestUnitOfWork(AppIdentityDbContext db,
                                ITestRepository testRepository,
                                IQuestionRepository questionRepository,
                                IAnswerRepository answerRepository)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));

            TestRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
            QuestionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
            AnswerRepository = answerRepository ?? throw new ArgumentNullException(nameof(answerRepository));
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            TestRepository = null;
            QuestionRepository = null;
            AnswerRepository = null;
            _db?.Dispose();
        }
    }
}