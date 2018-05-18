using Ninject.Modules;
using Ninject.Web.Common;
using TestingSystem.BLL.Interfaces;
using TestingSystem.BLL.Services;
using TestingSystem.DAL.EF;
using TestingSystem.DAL.Interfaces;
using TestingSystem.DAL.Interfaces.IRepository;
using TestingSystem.DAL.Repositories;

namespace TestingSystem.BLL.Infrastructure
{
    public class IdentityNinjectModule : NinjectModule
    {
        private readonly string _connectionString;

        public IdentityNinjectModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<AppIdentityDbContext>().To<AppIdentityDbContext>().InRequestScope().WithConstructorArgument(_connectionString);
            Bind<IIdentityUnitOfWork>().To<IdentityUnitOfWork>().InRequestScope();
            Bind<ITestUnitOfWork>().To<TestUnitOfWork>().InRequestScope();

            Bind<IUserService>().To<UserService>();
            Bind<IRoleService>().To<RoleService>();

            Bind<IUserRepository>().To<UserRepository>();
            Bind<IRoleRepository>().To<RoleRepository>();


            Bind<ITestRepository>().To<TestRepository>();
            Bind<IQuestionRepository>().To<QuestionRepository>();
            Bind<IAnswerRepository>().To<AnswerRepository>();
        }
    }
}