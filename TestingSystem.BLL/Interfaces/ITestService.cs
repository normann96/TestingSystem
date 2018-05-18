using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestingSystem.BLL.EntitiesDto;

namespace TestingSystem.BLL.Interfaces
{
    public interface ITestService : IDisposable
    {
        Task<TestDto> GetByIdAsync(int? testId);
        Task<List<TestDto>> GetAllAsync();
        Task CreateAsync(TestDto testDto);
        Task UpdateAsync(TestDto testDto);
        Task DeleteAsync(int testId);
    }
}