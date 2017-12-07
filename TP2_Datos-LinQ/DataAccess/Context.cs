using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Context
    {
        private static PruebaSqlEntities context;

        public Context()
        {
            context = new PruebaSqlEntities();
        }
        public static PruebaSqlEntities GetContext()
        {
            return context;
        }
    }
}
