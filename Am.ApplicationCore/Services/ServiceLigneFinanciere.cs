using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceLigneFinanciere : IServiceLigneFinanciere
    {
        private readonly IRepository<LigneFinanciere> _repository;

        public ServiceLigneFinanciere(IRepository<LigneFinanciere> repository)
        {
            _repository = repository;
        }

        public void Add(LigneFinanciere entity)
        {
            _repository.Add(entity);
        }

        public void Update(LigneFinanciere entity)
        {
            _repository.Update(entity);
        }

        public void Delete(LigneFinanciere entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<LigneFinanciere, bool>> where)
        {
            _repository.Delete(where);
        }

        public LigneFinanciere GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<LigneFinanciere> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<LigneFinanciere> GetMany(Expression<Func<LigneFinanciere, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public LigneFinanciere Get(Expression<Func<LigneFinanciere, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
