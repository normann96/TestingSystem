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
        public IQuestionResultRepository QuestionResultRepository { get; private set; }
        public ITestResultRepository TestResultRepository { get; private set; }

        public TestUnitOfWork(AppIdentityDbContext db,
                                ITestRepository testRepository,
                                IQuestionRepository questionRepository,
                                IAnswerRepository answerRepository,
                                ITestResultRepository testResultRepositoryRepository,
                                IQuestionResultRepository questionResultRepository)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));

            TestRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
            QuestionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
            AnswerRepository = answerRepository ?? throw new ArgumentNullException(nameof(answerRepository));
            TestResultRepository = testResultRepositoryRepository ?? throw new ArgumentNullException(nameof(testResultRepositoryRepository));
            QuestionResultRepository = questionResultRepository ?? throw new ArgumentNullException(nameof(questionResultRepository));
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
            TestResultRepository = null;
            QuestionResultRepository = null;
            _db?.Dispose();
        }
    }
}