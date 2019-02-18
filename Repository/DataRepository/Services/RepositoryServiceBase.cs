using System.Collections.Generic;
using System.Configuration;
using DataRepository.Models;
using SQLite;

namespace DataRepository.Services
{
    public abstract class RepositoryServiceBase<TEntity> where TEntity : BaseEntity, new()
    {
        protected readonly string DbPath;

        protected RepositoryServiceBase()
        {
            DbPath = ConfigurationManager.AppSettings["DataRepositoryDbPath"];

            using (var db = new SQLiteConnection(DbPath))
            {
                db.CreateTable<TEntity>();
            }
        }

        public TEntity Insert(TEntity entity)
        {
            using (var db = new SQLiteConnection(DbPath))
            {
                db.Insert(entity);

                return entity;
            }
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            using (var db = new SQLiteConnection(DbPath))
            {
                db.InsertAll(entities);
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (var db = new SQLiteConnection(DbPath))
            {
                return db.Table<TEntity>().ToList();
            }
        }

        public TEntity GetById(int id)
        {
            using (var db = new SQLiteConnection(DbPath))
            {
                return db.Table<TEntity>().FirstOrDefault(e => e.Id == id);
            }
        }
    }
}
