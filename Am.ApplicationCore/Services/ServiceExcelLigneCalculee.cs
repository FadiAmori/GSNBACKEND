using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceExcelLigneCalculee : IServiceExcelLigneCalculee
    {
        private readonly IRepository<ExcelLigneCalculee> _repository;

        public ServiceExcelLigneCalculee(IRepository<ExcelLigneCalculee> repository)
        {
            _repository = repository;
        }

        public void Add(ExcelLigneCalculee entity)
        {
            _repository.Add(entity);
        }

        public void Update(ExcelLigneCalculee entity)
        {
            _repository.Update(entity);
        }

        public void Delete(ExcelLigneCalculee entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<ExcelLigneCalculee, bool>> where)
        {
            _repository.Delete(where);
        }

        public ExcelLigneCalculee GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<ExcelLigneCalculee> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<ExcelLigneCalculee> GetMany(Expression<Func<ExcelLigneCalculee, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public ExcelLigneCalculee Get(Expression<Func<ExcelLigneCalculee, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
