using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class GenericService<TEntity, TDto> : IServices<TEntity, TDto> where TEntity : class where TDto : class
    {

        private IUnitOfWork _unitOfWork;
        private IGenericRepostiory<TEntity> _genericRepostiory;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepostiory<TEntity> genericRepostiory)
        {
            _unitOfWork = unitOfWork;
            _genericRepostiory = genericRepostiory;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

            await _genericRepostiory.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
            return Response<TDto>.Success(newDto, 200);

        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var products = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepostiory.GetAllAsync());
            return Response<IEnumerable<TDto>>.Success(products, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var product = await _genericRepostiory.GetByIdAsync(id);
            if (product == null)
            {
                return Response<TDto>.Failed("It does not Found", 404, true);
            }

            return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product), 200);

        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var isExistEntity = await _genericRepostiory.GetByIdAsync(id);

            if (isExistEntity == null)
            {
                return Response<NoDataDto>.Failed("Id is not Found", 404, true);
            }

             _genericRepostiory.Remove(isExistEntity);
            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(204);

        }

        public async Task<Response<NoDataDto>> Update(TDto entity,int id)
        {
            var isExistEntity = await _genericRepostiory.GetByIdAsync(id);

            if (isExistEntity==null)
            {
                return Response<NoDataDto>.Failed("Id is not Found", 404, true);
            }

            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

            _genericRepostiory.Update(newEntity);

            await _unitOfWork.CommitAsync();

            //204 kodu No Contente cavab verir.
            return Response<NoDataDto>.Success(204);

        }

        public async Task<Response<IEnumerable<TDto>>> Where(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            var list = _genericRepostiory.Where(predicate);

            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()),200);
        }

      
    }
}
