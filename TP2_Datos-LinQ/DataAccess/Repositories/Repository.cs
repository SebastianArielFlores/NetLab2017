using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Repository<T>
        where T : class
    {
        private PruebaSqlEntities context;

        public Repository()
        {
            context = Context.GetContext();
        }

        public T Persist(T entity)
        {
            return context.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
            //SaveChanges();
        }

        public T Update(T entity)
        {
            //context.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
            context.Entry<T>(entity);
            //SaveChanges();

            return entity;
        }

        public IQueryable<T> Set()
        {
            return context.Set<T>();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}