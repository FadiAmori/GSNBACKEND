using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceCR : IServiceCR
    {
        private readonly IRepository<CR> _repository;

        public ServiceCR(IRepository<CR> repository)
        {
            _repository = repository;
        }

        public void Add(CR entity)
        {
            _repository.Add(entity);
        }

        public void Update(CR entity)
        {
            _repository.Update(entity);
        }

        public void Delete(CR entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<CR, bool>> where)
        {
            _repository.Delete(where);
        }

        public CR GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<CR> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<CR> GetMany(Expression<Func<CR, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public CR Get(Expression<Func<CR, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
