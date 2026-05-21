using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceSousCategorieFinanciere : IServiceSousCategorieFinanciere
    {
        private readonly IRepository<SousCategorieFinanciere> _repository;

        public ServiceSousCategorieFinanciere(IRepository<SousCategorieFinanciere> repository)
        {
            _repository = repository;
        }

        public void Add(SousCategorieFinanciere entity)
        {
            _repository.Add(entity);
        }

        public void Update(SousCategorieFinanciere entity)
        {
            _repository.Update(entity);
        }

        public void Delete(SousCategorieFinanciere entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<SousCategorieFinanciere, bool>> where)
        {
            _repository.Delete(where);
        }

        public SousCategorieFinanciere GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<SousCategorieFinanciere> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<SousCategorieFinanciere> GetMany(Expression<Func<SousCategorieFinanciere, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public SousCategorieFinanciere Get(Expression<Func<SousCategorieFinanciere, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
