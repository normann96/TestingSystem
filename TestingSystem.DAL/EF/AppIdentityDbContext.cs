using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL.EF
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(string connectionString) : base(connectionString, throwIfV1Schema: false)
        {
        }

        static AppIdentityDbContext()
        {
            System.Data.Entity.Database.SetInitializer(new InitializerDb());
        }


        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
    }

    public class IdentityContextFactory : IDbContextFactory<AppIdentityDbContext>
    {
        public AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext("Server=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\TestingSystemDb.mdf;Initial Catalog=TestingSystemDb;Integrated Security=True;MultipleActiveResultSets=True");
        }
    }
}