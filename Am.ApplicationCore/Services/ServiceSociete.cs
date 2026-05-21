using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceSociete : IServiceSociete
    {
        private readonly IRepository<Societe> _repository;

        public ServiceSociete(IRepository<Societe> repository)
        {
            _repository = repository;
        }

        public void Add(Societe entity)
        {
            _repository.Add(entity);
        }

        public void Update(Societe entity)
        {
            _repository.Update(entity);
        }

        public void Delete(Societe entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<Societe, bool>> where)
        {
            _repository.Delete(where);
        }

        public Societe GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<Societe> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Societe> GetMany(Expression<Func<Societe, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public Societe Get(Expression<Func<Societe, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
