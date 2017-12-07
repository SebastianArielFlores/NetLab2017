using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServicesController
    {
        public DataAccess.Context context;

        public CustomerServices customerServices;
        public EmployeeServices employeeServices;
        public OrderServices orderServices;
        public OrderDetailsServices orderDetailsServices;
        public ProductServices productServices;

        public ServicesController()
        {
            context = new DataAccess.Context();

            customerServices = new CustomerServices();
            employeeServices = new EmployeeServices();
            orderServices = new OrderServices();
            orderDetailsServices = new OrderDetailsServices();
            productServices = new ProductServices();
        }
        
    }
}
