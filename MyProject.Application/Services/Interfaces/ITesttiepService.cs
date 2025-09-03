using MyProject.Domain.Entities;

namespace MyProject.Application.Services.Interfaces
{
    public interface ITesttiepService
    {
        Task<List<Testtiep>> GetAllAsync();
        Task<Testtiep> CreateAsync(Testtiep entity);
    }
}
