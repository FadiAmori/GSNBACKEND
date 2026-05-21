using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Am.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Am.Infrastructure.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AmContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AmContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbSet.Where(where).AsEnumerable();
            foreach (T obj in objects)
            {
                _dbSet.Remove(obj);
            }
        }

        public virtual T GetById(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).ToList();
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).FirstOrDefault();
        }

        public virtual void Commit()
        {
            _context.SaveChanges();
        }
    }
}
