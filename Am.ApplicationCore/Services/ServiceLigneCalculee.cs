using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceLigneCalculee : IServiceLigneCalculee
    {
        private readonly IRepository<LigneCalculee> _repository;

        public ServiceLigneCalculee(IRepository<LigneCalculee> repository)
        {
            _repository = repository;
        }

        public void Add(LigneCalculee entity)
        {
            _repository.Add(entity);
        }

        public void Update(LigneCalculee entity)
        {
            _repository.Update(entity);
        }

        public void Delete(LigneCalculee entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<LigneCalculee, bool>> where)
        {
            _repository.Delete(where);
        }

        public LigneCalculee GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<LigneCalculee> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<LigneCalculee> GetMany(Expression<Func<LigneCalculee, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public LigneCalculee Get(Expression<Func<LigneCalculee, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
