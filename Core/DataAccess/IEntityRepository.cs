﻿using Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        List<T> GetAllAsNoTracking(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        T GetAsNoTracking(Expression<Func<T, bool>> filter);
        T GetFind(int id);
        void Add(T entity);
        void AddRange(List<T> entity);
        void Delete(T entity);
        void DeleteRange(List<T> entity);
        void Update(T entity);
        void UpdateRange(List<T> entity);

    }
}
