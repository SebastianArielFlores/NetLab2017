using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Dtos;
using System.Globalization;

namespace Services
{
    public class OrderServices
    {


        /* 
             * HACER ALTA, BAJA, MODIFICACIÓN DE ORDENES (Orders)
             *
             */

        Repository<Order> orderRepository;
        Repository<Order_Detail> orderDetailsRepository;

        public OrderServices()
        {
            orderRepository = new Repository<Order>();
            orderDetailsRepository = new Repository<Order_Detail>();
        }



        #region GET ALL ORDERS
        public IEnumerable<OrderDto> GetAll()
        {
            return orderRepository.Set()
                   .ToList()
                   .Select(c => new OrderDto
                   {
                       //Address = c.Address,
                   }).ToList();
        }
        #endregion



        #region GET ORDER BY ID
        public Order GetOrderByID(int orderId,ServicesController services)
        {
            var order = orderRepository.Set().ToList()
                .FirstOrDefault(c => c.OrderID == orderId);

            if (order == null)
            {
                Console.WriteLine("No existe la orden!");
                return null;
            }

            //CARGO LAS ORDENES EN UNA LISTA
            var orderDetails = new HashSet<Order_Detail>();
            foreach (var orderDetail in order.Order_Details)
            {
                orderDetails.Add(new Order_Detail()
                {
                    Discount = orderDetail.Discount,
                    //Order = orderDetail.Order,
                    OrderID = orderDetail.OrderID,
                    //Product = orderDetail.Product,
                    ProductID = orderDetail.ProductID,
                    Quantity = orderDetail.Quantity,
                    UnitPrice = orderDetail.UnitPrice,
                });
            }

            
            var orderByID = new Order()
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry,

                //Customer
                Customer = services.customerServices.GetCustomerByID(order.CustomerID,services),
                //Employee = orderDto.Employee,
                Employee = services.employeeServices.GetEmployeeByID(order.EmployeeID,services),

                Order_Details = orderDetails,

                /*Order_Details = new HashSet<Order_Detail>()
                {
                    foreach (var order in orderDto.Order_Details)
                    {
                    new Order_Detail()
                    {
                        Discount = order.Discount,
                        //Order = order.Order,
                        OrderID = order.OrderID,
                        //Product = order.Product,
                        ProductID = order.ProductID,
                        Quantity = order.Quantity,
                        UnitPrice = order.UnitPrice,
                    };
                } });
                */

                //Shipper
            };
            
            

            return orderByID;

        }
        #endregion


        #region GET ORDER DTO BY ID
        public OrderDto GetOrderDtoByID(int orderId, ServicesController services)
        {
            var order = orderRepository.Set().ToList()
                .FirstOrDefault(c => c.OrderID == orderId);

            if (order == null)
            {
                Console.WriteLine("No existe la orden!");
                return null;
            }

            //CARGO LAS ORDENES EN UNA LISTA
            var orderDetails = new HashSet<OrderDetailDto>();
            foreach (var orderDetail in order.Order_Details)
            {
                orderDetails.Add(new OrderDetailDto()
                {
                    Discount = orderDetail.Discount,
                    //Order = orderDetail.Order,
                    OrderID = orderDetail.OrderID,
                    //Product = orderDetail.Product,
                    ProductID = orderDetail.ProductID,
                    Quantity = orderDetail.Quantity,
                    UnitPrice = orderDetail.UnitPrice,
                });
            }

            var orderDto = new OrderDto()
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry,

                //Customer
                Customer = services.customerServices.GetCustomerDtoByID(order.CustomerID,services),
                //Employee = orderDto.Employee,
                Employee = services.employeeServices.GetEmployeeDtoByID(order.EmployeeID,services),

                Order_Details = orderDetails,

                /*Order_Details = new HashSet<Order_Detail>()
                {
                    foreach (var order in orderDto.Order_Details)
                    {
                    new Order_Detail()
                    {
                        Discount = order.Discount,
                        //Order = order.Order,
                        OrderID = order.OrderID,
                        //Product = order.Product,
                        ProductID = order.ProductID,
                        Quantity = order.Quantity,
                        UnitPrice = order.UnitPrice,
                    };
                } });
                */

                //Shipper
            };

            return orderDto;

        }
        #endregion


        #region MODIFY / UPDATE ORDER
        //public void ModifyOrder(string orderId)
        public void Modify(OrderDto orderDto, ServicesController services)
        {

            var order = orderRepository.Set()
            .FirstOrDefault(x => x.OrderID == orderDto.OrderID);

            if (order == null)
                throw new Exception("La orden no existe");

            Console.WriteLine($"La Orden : {orderDto.OrderID} ha sido encontrada.");
            //Console.WriteLine($"Su Nombre de Contacto es : {orderDto.ContactName}.");

            //OrderID

            var orderId = 0;

            do
            {
                Console.WriteLine("");
                Console.WriteLine("Ingrese el nuevo ID de la Orden:");
            }
            while (int.TryParse(Console.ReadLine(), out orderId));

            orderDto.OrderID = orderId;


            //ContactName
            Console.WriteLine("");
            Console.WriteLine("Ingrese el País destino de la Orden:");
            orderDto.ShipCountry = Console.ReadLine();

            //Date

            DateTime orderDate;
            do
            {
                Console.WriteLine("");
                Console.WriteLine("Ingrese la fecha de Orden (formato dd/MM/yyyy):");
            }
            while (!(DateTime.TryParseExact(Console.ReadLine(),
                                        "dd/MM/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                out orderDate)));

            /*
            if (DateTime.TryParseExact(Console.ReadLine(),
                                        "dd/MM/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                out dt))
            {
                //valid date

            }
            else
            {
                //invalid date
            }
            */
            orderRepository.Update(order);
            orderRepository.SaveChanges();
        }
        #endregion



        #region CREATE ORDER

        public void Create(OrderDto orderDto, ServicesController services)
        {

            //CARGO LAS ORDENES EN UNA LISTA
            var OrderDetails = new HashSet<Order_Detail>();
            foreach (var orderDetail in orderDto.Order_Details)
            {
                OrderDetails.Add(new Order_Detail
                {
                    Discount = orderDetail.Discount,
                    //Order = orderDetail.Order,
                    Order = services.orderServices.GetOrderByID(orderDetail.OrderID,services),
                    OrderID = orderDetail.OrderID,
                    //Product = orderDetail.Product,
                    ProductID = orderDetail.ProductID,
                    Quantity = orderDetail.Quantity,
                    UnitPrice = orderDetail.UnitPrice,
                });
            }

            orderRepository.Persist(new Order
            {
                OrderID = orderDto.OrderID,
                CustomerID = orderDto.CustomerID,
                EmployeeID = orderDto.EmployeeID,
                OrderDate = orderDto.OrderDate,
                RequiredDate = orderDto.RequiredDate,
                ShippedDate = orderDto.ShippedDate,
                Freight = orderDto.Freight,
                ShipName = orderDto.ShipName,
                ShipCity = orderDto.ShipCity,
                ShipRegion = orderDto.ShipRegion,
                ShipPostalCode = orderDto.ShipPostalCode,
                ShipAddress = orderDto.ShipAddress,
                ShipCountry = orderDto.ShipCountry,
                ShipVia = orderDto.ShipVia,
                //Customer
                //Customer = services.customerServices.GetCustomerByID(orderDto.CustomerID,services),
                //Employee = orderDto.Employee,
                //Employee = services.employeeServices.GetEmployeeByID(orderDto.EmployeeID,services),
                //Shipper

                Order_Details = OrderDetails,

                /*Order_Details = new HashSet<Order_Detail>()
                {
                    foreach (var order in orderDto.Order_Details)
                    {
                    new Order_Detail()
                    {
                        Discount = order.Discount,
                        //Order = order.Order,
                        OrderID = order.OrderID,
                        //Product = order.Product,
                        ProductID = order.ProductID,
                        Quantity = order.Quantity,
                        UnitPrice = order.UnitPrice,
                    };
                } });
                */
            });


            orderRepository.SaveChanges();
        }

        #endregion



        #region REMOVE ORDER

        public void Remove(OrderDto orderDto, ServicesController services)
        {
            var deletedOrderId = orderDto.OrderID;

            var orderRemove = orderRepository.Set()
                .FirstOrDefault(x => x.OrderID == orderDto.OrderID);

            if (orderRemove == null)
                throw new Exception("La Orden no existe!");

            //var detailsRemove = orderDetailsRepository.Set()
            using (var detailsServices = new OrderDetailsServices())
            {
                if (orderRemove.Order_Details.Any())
                {
                    var detailsRemove = detailsServices.GetAll()
                                       .Where(d => d.OrderID == orderRemove.OrderID)
                                       .Select(d => new Order_Detail
                                       {
                                           OrderID = d.OrderID,
                                           Order = new Order
                                                { OrderID = d.OrderID },
                                           Discount = d.Discount,
                                           Product = services.productServices.GetProductByID(d.ProductID),//new Product
                                                //{ ProductID = d.ProductID },
                                           ProductID = d.ProductID,
                                           Quantity = d.Quantity,
                                           UnitPrice = d.UnitPrice,

                                       });

                    foreach (var detail in orderRemove.Order_Details)
                    {
                        var detailRemove = orderDetailsRepository.Set()
                            .FirstOrDefault(x => x.OrderID == orderRemove.OrderID);

                        if (detailRemove == null)
                            throw new Exception("No hay Detalle de Orden.");

                        Console.WriteLine($"El Detalle con ID de Orden : {detail.OrderID}, Producto : {detail.Product.ProductName}, Cantidad : {detail.Quantity} será eliminado.");
                        orderDetailsRepository.Remove(detailRemove);
                        orderDetailsRepository.SaveChanges();

                        //orderDetailsRepository.Remove(detail);
                        //orderDetailsRepository.SaveChanges();
                    }
                    Console.WriteLine($"");
                    Console.WriteLine($"Detalles eliminados.");
                }
                else
                {
                    Console.WriteLine($"");
                    Console.WriteLine($"La Orden no tenía Detalles asociados.");
                }

                //orderRepository.Remove(orderRemove);
                //orderRepository.SaveChanges();

                var order = orderRepository.Set()
                .FirstOrDefault(x => x.OrderID == deletedOrderId);

                if (order == null)
                {
                    throw new Exception("El cliente no existe");
                }
                orderRepository.Remove(order);
                orderRepository.SaveChanges();

                Console.WriteLine($"");
                Console.WriteLine($"La Orden con ID : {deletedOrderId} ha sido eliminada con éxito.");
            }
        }
        #endregion
    }
}