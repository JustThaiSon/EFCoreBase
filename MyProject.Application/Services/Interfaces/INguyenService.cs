using MyProject.Domain.Entities;

namespace MyProject.Application.Services.Interfaces
{
    public interface INguyenService
    {
        Task<List<NguyenEntity>> GetAllAsync();
        Task<NguyenEntity> CreateAsync(NguyenEntity entity);
    }
}
