using TestingSystem.DAL.EF;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces.IRepository;
using TestingSystem.DAL.Repositories.Base;

namespace TestingSystem.DAL.Repositories
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppIdentityDbContext context) : base(context)
        {
        }
    }
}