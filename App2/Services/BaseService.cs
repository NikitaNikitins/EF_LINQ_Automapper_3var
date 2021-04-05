using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Services
{
    public class BaseService<T> where T : class
    {
        protected ExamEntities dataContext;
        protected readonly DbSet<T> dbSet;

        public BaseService(ExamEntities dataContext)
        {
            this.dataContext = dataContext;
            dbSet = dataContext.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            dataContext.SaveChanges();
        }

        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
            dataContext.SaveChanges();
        }
    }
}
