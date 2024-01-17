using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        private readonly TContext _context;
        public EfEntityRepositoryBase(TContext context)
        {
            _context = context;
        }
        public void Add(TEntity entity)
        {
            var addEntity = _context.Entry(entity);
            addEntity.State = EntityState.Added;
            _context.SaveChanges();
        }

        public void AddRange(List<TEntity> entity)
        {
            _context.AddRange(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {

            var deleteEntity = _context.Entry(entity);
            deleteEntity.State = EntityState.Deleted;
            _context.SaveChanges();
        }



        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? _context.Set<TEntity>().ToList()
                : _context.Set<TEntity>().Where(filter).ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().FirstOrDefault(filter);
        }

        public void Update(TEntity entity)
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual void UpdateRange(List<TEntity> entity)
        {
            _context.UpdateRange(entity);
            _context.SaveChanges();
        }

        public void DeleteRange(List<TEntity> entity)
        {
            _context.RemoveRange(entity);
            _context.SaveChanges();
        }
    }

}
