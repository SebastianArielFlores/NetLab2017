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

        public OrderDetailsServices()
        {
            orderRepository = new Repository<Order>();
            orderDetailsRepository = new Repository<Order_Detail>();
        }



        #region GET ALL ORDER DETAILS
        public IEnumerable<OrderDetailDto> GetAll()
        {
            return orderDetailsRepository.Set()
                   .ToList()
                   .Select(c => new OrderDetailDto
                   {
                       //Address = c.Address,
                   }).ToList();
        }
        #endregion



        #region GET ORDER DETAIL BY ID
        public OrderDto GetDetailByID(int detailId)
        {
            var detail = orderDetailsRepository.Set().ToList()
                .FirstOrDefault(d => d.OrderID == detailId);

            if (detail == null)
            {
                Console.WriteLine("No existe el Detalle de Orden!");
                return null;
            }

            var orderDetailDto = new OrderDto()
            {
                OrderID = detail.OrderID,
                //ContactName = order.ContactName,
                //CompanyName = order.CompanyName,
            };

            return orderDetailDto;

        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
