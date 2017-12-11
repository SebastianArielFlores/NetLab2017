using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Dtos;

namespace Services
{
    public class OrderDetailsServices :IDisposable
    {
        Repository<Order> orderRepository;
        Repository<Order_Detail> orderDetailsRepository;


        #region OrderDetailsServices CLASS CONSTRUCTOR
        public OrderDetailsServices()
        {
            this.orderRepository = new Repository<Order>();
            this.orderDetailsRepository = new Repository<Order_Detail>();
        }
        #endregion


        #region GET ALL ORDER DETAILS
        public ICollection<OrderDetailDto> GetAll()
        {
            try
            {
                return this.orderDetailsRepository.Set()
                   .ToList()
                   .Select(c => new OrderDetailDto
                   {
                       OrderID = c.OrderID,
                       Quantity = c.Quantity,
                       UnitPrice = c.UnitPrice,
                       ProductID = c.ProductID,
                       Discount = c.Discount,
                   }).ToList();
            }
            catch
            {
                NewLine();
                Console.WriteLine($"Se produjo un ERROR al intentar obtener todos los Detalles de Orden.");
                return null;
            }
        }
        #endregion


        #region GET ORDER DETAIL BY ID
        public OrderDto GetDetailByID(int detailId)
        {
            try
            {
                var detail = this.orderDetailsRepository.Set().ToList()
               .FirstOrDefault(d => d.OrderID == detailId);

                if (detail == null)
                {
                    NewLine();
                    Console.WriteLine("No existe el Detalle de Orden!");
                    return null;
                }

                var orderDetailDto = new OrderDto()
                {
                    OrderID = detail.OrderID,
                };

                return orderDetailDto;
            }
            catch
            {
                NewLine();
                Console.WriteLine($"Se produjo un ERROR al intentar obtener el Detalle de Orden con ID : '{detailId}'.");
                return null;
            }
        }
        #endregion
        

        #region NEW CONSOLE EMPTY COMMAND LINE
        public void NewLine()
        {
            Console.WriteLine("");
        }
        #endregion


        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
