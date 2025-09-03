using Microsoft.EntityFrameworkCore;
using MyProject.Application.Services.Interfaces;
using MyProject.Domain.Entities;
using MyProject.Infrastructure;

namespace MyProject.Application.Services
{
    public class NguyenService : INguyenService
    {
        private readonly IRepositoryAsync<NguyenEntity> _nguyenRepository;

        public NguyenService(IRepositoryAsync<NguyenEntity> nguyenRepository)
        {
            _nguyenRepository = nguyenRepository;
        }

        public async Task<List<NguyenEntity>> GetAllAsync()
        {
            return await _nguyenRepository.AsNoTrackingQueryable().ToListAsync();
        }

        public async Task<NguyenEntity> CreateAsync(NguyenEntity entity)
        {
            await _nguyenRepository.AddAsync(entity);
            await _nguyenRepository.SaveChangesAsync();
            return entity;
        }
    }
}
