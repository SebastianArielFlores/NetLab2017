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

        public Context()
        {
            context = new TP2DatosLinQEntities();
        }
        public static TP2DatosLinQEntities GetContext()
        {
            return context;
        }
    }
}
