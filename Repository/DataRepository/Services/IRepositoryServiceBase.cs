using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataRepository.Models;

namespace DataRepository.Services
{
    public interface IRepositoryServiceBase<TEntity> where TEntity : BaseEntity
    {
        TEntity Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> whereExpression);

        TEntity GetById(int id);
    }
}
