using Microsoft.EntityFrameworkCore;
using MyProject.Application.Services.Interfaces;
using MyProject.Domain.Entities;
using MyProject.Infrastructure;

namespace MyProject.Application.Services
{
    public class TesttiepService : ITesttiepService
    {
        private readonly IRepositoryAsync<Testtiep> _repository;

        public TesttiepService(IRepositoryAsync<Testtiep> repository)
        {
            _repository = repository;
        }

        public async Task<List<Testtiep>> GetAllAsync()
        {
            return await _repository.AsNoTrackingQueryable().ToListAsync();
        }

        public async Task<Testtiep> CreateAsync(Testtiep entity)
        {
            entity.Id = Guid.NewGuid();
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity;
        }
    }
}
