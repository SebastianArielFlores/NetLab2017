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

        #region ServicesController CLASS CONSTRUCTOR
        public ServicesController()
        {
            this.context = new DataAccess.Context();

            this.customerServices = new CustomerServices();
            this.employeeServices = new EmployeeServices();
            this.orderServices = new OrderServices();
            this.orderDetailsServices = new OrderDetailsServices();
            this.productServices = new ProductServices();
        }
        #endregion
    }
}
