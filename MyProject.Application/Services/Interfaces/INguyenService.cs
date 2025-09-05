using MyProject.Application.Common.Models;
using MyProject.Domain.Entities;
using MyProject.Helper.Utils;

namespace MyProject.Application.Services.Interfaces
{
    public interface INguyenService
    {
        Task<CommonResponse<GetNguyenEntityPagingRes>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<CommonResponse<string>> CreateAsync(NguyenEntity entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
