using System;
using System.Threading.Tasks;
using TestingSystem.BLL.EntitiesDto;

namespace TestingSystem.BLL.Interfaces
{
    public interface IQuestionService : IDisposable
    {
        Task<QuestionDto> GetByIdAsync(int questionId);
        Task CreateAsync(QuestionDto questionDto);
        Task UpdateAsync(QuestionDto questionDto);
        Task DeleteAsync(int questionId);
    }
}