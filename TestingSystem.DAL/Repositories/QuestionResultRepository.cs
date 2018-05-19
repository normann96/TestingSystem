using TestingSystem.DAL.EF;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces.IRepository;
using TestingSystem.DAL.Repositories.Base;

namespace TestingSystem.DAL.Repositories
{
    public class QuestionResultRepository : BaseRepository<QuestionResult>, IQuestionResultRepository
    {
        public QuestionResultRepository(AppIdentityDbContext context) : base(context)
        {
        }
    }
}