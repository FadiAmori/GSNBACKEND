using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceCategorieFinanciere : IServiceCategorieFinanciere
    {
        private readonly IRepository<CategorieFinanciere> _repository;

        public ServiceCategorieFinanciere(IRepository<CategorieFinanciere> repository)
        {
            _repository = repository;
        }

        public void Add(CategorieFinanciere entity)
        {
            _repository.Add(entity);
        }

        public void Update(CategorieFinanciere entity)
        {
            _repository.Update(entity);
        }

        public void Delete(CategorieFinanciere entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<CategorieFinanciere, bool>> where)
        {
            _repository.Delete(where);
        }

        public CategorieFinanciere GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<CategorieFinanciere> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<CategorieFinanciere> GetMany(Expression<Func<CategorieFinanciere, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public CategorieFinanciere Get(Expression<Func<CategorieFinanciere, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
