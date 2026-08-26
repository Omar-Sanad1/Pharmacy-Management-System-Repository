using Core.Interfaces;
using Core.Pagination;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly PharmacyManagementDbContext _dbContext;
        public GenericRepository(PharmacyManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<T> GetAllFiltered(Func<T, bool> Filter)
        => _dbContext.Set<T>()
           .Where(Filter)
           .ToList();

        public IEnumerable<T> GetAllPaged(int pageNumber, int pageSize)
        => _dbContext.Set<T>()
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToList();

        public T GetByID(int id)
        => _dbContext.Set<T>().Find(id);
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }


        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
