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
        private TP2DatosLinQEntities context;

        #region Repository CLASS CONSTRUCTOR
        public Repository()
        {
            this.context = Context.GetContext();
        }
        #endregion


        #region PERSISTIR
        public T Persist(T entity)
        {
            return context.Set<T>().Add(entity);
        }
        #endregion


        #region REMOVER / ELIMINAR
        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }
        #endregion


        #region ACTUALIZAR
        public T Update(T entity)
        {
            context.Entry<T>(entity);

            return entity;
        }
        #endregion


        #region SETEAR
        public IQueryable<T> Set()
        {
            return context.Set<T>();
        }
        #endregion


        #region GUARDAR
        public void SaveChanges()
        {
            context.SaveChanges();
        }
        #endregion
    }
}