using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Am.ApplicationCore.Services
{
    public class ServiceExcelVariable : IServiceExcelVariable
    {
        private readonly IRepository<ExcelVariable> _repository;

        public ServiceExcelVariable(IRepository<ExcelVariable> repository)
        {
            _repository = repository;
        }

        public void Add(ExcelVariable entity)
        {
            _repository.Add(entity);
        }

        public void Update(ExcelVariable entity)
        {
            _repository.Update(entity);
        }

        public void Delete(ExcelVariable entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(Expression<Func<ExcelVariable, bool>> where)
        {
            _repository.Delete(where);
        }

        public ExcelVariable GetById(params object[] keyValues)
        {
            return _repository.GetById(keyValues);
        }

        public IEnumerable<ExcelVariable> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<ExcelVariable> GetMany(Expression<Func<ExcelVariable, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public ExcelVariable Get(Expression<Func<ExcelVariable, bool>> where)
        {
            return _repository.Get(where);
        }

        public void Commit()
        {
            _repository.Commit();
        }
    }
}
