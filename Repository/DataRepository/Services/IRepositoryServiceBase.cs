using System.Collections.Generic;
using DataRepository.Models;

namespace DataRepository.Services
{
    public interface IRepositoryServiceBase<TEntity> where TEntity : BaseEntity
    {
        TEntity Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> GetAll();

        TEntity GetById(int id);
    }
}
