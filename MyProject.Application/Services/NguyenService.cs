using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MyProject.Application.Common.Mapping;
using MyProject.Application.Common.Models;
using MyProject.Application.Services.Interfaces;
using MyProject.Domain.Entities;
using MyProject.Helper.Constants.Globals;
using MyProject.Helper.Utils;
using MyProject.Infrastructure;
using System.Threading;

namespace MyProject.Application.Services
{
    public class NguyenService : INguyenService
    {
        private readonly IRepositoryAsync<NguyenEntity> _nguyenRepository;
        private readonly IMapper _mapper;
        public NguyenService(IRepositoryAsync<NguyenEntity> nguyenRepository, IMapper mapper)
        {
            _nguyenRepository = nguyenRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse<GetNguyenEntityPagingRes>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageNumber <= 0)
                return CommonResponse<GetNguyenEntityPagingRes>.Fail(ResponseCodeEnum.ERR_AGE_NOT_VALID);
            var result = await _nguyenRepository.AsNoTrackingQueryable()
                .ProjectTo<NguyenDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(pageNumber, pageSize, cancellationToken);
            return CommonResponse<GetNguyenEntityPagingRes>.Success(new GetNguyenEntityPagingRes
            {
                TotalRecord = result.TotalCount,
                Records = result.Items
            });
        }

        public async Task<NguyenEntity> CreateAsync(NguyenEntity entity)
        {
            await _nguyenRepository.AddAsync(entity);
            await _nguyenRepository.SaveChangesAsync();
            return entity;
        }
      
    }
}
