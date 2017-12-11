using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Context
    {
        private static TP2DatosLinQEntities context;

        #region Context CLASS CONSTRUCTOR
        public Context()
        {
            context = new TP2DatosLinQEntities();
        }
        #endregion


        #region OBTENER CONTEXTO ACTUAL
        public static TP2DatosLinQEntities GetContext()
        {
            if(context==null)
            {
                context = new TP2DatosLinQEntities();
            }

            return context;
        }
        #endregion
    }
}
