using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceCategorieCR : IServiceCategorieCR
    {
        private readonly IRepository<CategorieCR> _repository;

        public ServiceCategorieCR(IRepository<CategorieCR> repository)
        {
            _repository = repository;
        }

        public void Add(CategorieCR entity)
        {
            _repository.Add(entity);
        }

        public void Update(CategorieCR entity)
        {
            _repository.Update(entity);
        }

        public void Delete(CategorieCR entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<CategorieCR, bool>> where)
        {
            _repository.Delete(where);
        }

        public CategorieCR GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<CategorieCR> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<CategorieCR> GetMany(Expression<Func<CategorieCR, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public CategorieCR Get(Expression<Func<CategorieCR, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
