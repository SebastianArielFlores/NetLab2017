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
        
        Repository<Order> orderRepository;
        Repository<Order_Detail> orderDetailsRepository;

        #region OrderServices CLASS CONSTRUCTOR
        public OrderServices()
        {
            this.orderRepository = new Repository<Order>();
            this.orderDetailsRepository = new Repository<Order_Detail>();
        }
        #endregion


        #region GET ALL ORDERS
        public ICollection<OrderDto> GetAll(ServicesController services)
        {
            try
            {
                return this.orderRepository.Set()
                   .ToList()
                   .Select(o => new OrderDto
                   {
                       OrderID = o.OrderID,
                       CustomerID = o.CustomerID,
                       EmployeeID = o.EmployeeID,
                       ShipName = o.ShipName,
                   }).ToList();
            }
            catch
            {
                Console.WriteLine("Se produjo un ERROR al intentar obtener todas las Ordenes...");

                return null;
            }
            
        }
        #endregion



        #region GET ORDER BY ID
        public Order GetOrderByID(int orderId, ServicesController services)
        {
            try
            {
                var order = this.orderRepository.Set().ToList()
                .FirstOrDefault(c => c.OrderID == orderId);

                if (order == null)
                {
                    NewLine();
                    Console.WriteLine("No existe la orden!");

                    return null;
                }

                
                var orderDetails = new HashSet<Order_Detail>();

                foreach (var orderDetail in order.Order_Details)
                {
                    orderDetails.Add(new Order_Detail()
                    {
                        Discount = orderDetail.Discount,
                        OrderID = orderDetail.OrderID,
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
                    Customer = services.customerServices.GetCustomerByID(order.CustomerID, services),
                    //Employee
                    Employee = services.employeeServices.GetEmployeeByID(order.EmployeeID, services),

                    Order_Details = orderDetails,

                    //Shipper
                };

                return orderByID;

            }
            catch
            {
                Console.WriteLine($"Se produjo un ERROR al intentar obtener la Orden con ID : '{orderId}'...");

                return null;
            }
        }
        #endregion


        #region GET ORDER DTO BY ID
        public OrderDto GetOrderDtoByID(int orderId, ServicesController services)
        {
            try
            {
                var order = this.orderRepository.Set().ToList()
                .FirstOrDefault(c => c.OrderID == orderId);

                if (order == null)
                {
                    NewLine();
                    Console.WriteLine("No existe la orden!");

                    return null;
                }
                

                var orderDetails = new HashSet<OrderDetailDto>();

                foreach (var orderDetail in order.Order_Details)
                {
                    orderDetails.Add(new OrderDetailDto()
                    {
                        Discount = orderDetail.Discount,
                        OrderID = orderDetail.OrderID,
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
                    Customer = services.customerServices.GetCustomerDtoByID(order.CustomerID, services),
                    //Employee = orderDto.Employee,
                    Employee = services.employeeServices.GetEmployeeDtoByID(order.EmployeeID, services),

                    Order_Details = orderDetails,

                    //Shipper
                };

                return orderDto;
            }
            catch
            {
                Console.WriteLine($"Se produjo un ERROR al intentar obtener la Orden con ID : '{orderId}'...");

                return null;
            }

        }
        #endregion


        #region Update / UPDATE ORDER
        public void Update(OrderDto orderDto, ServicesController services)
        {
            try
            {
                var order = this.orderRepository.Set()
                .FirstOrDefault(x => x.OrderID == orderDto.OrderID);

                if (order == null)
                {
                    //throw new Exception("La orden no existe");
                    NewLine();
                    Console.WriteLine($"La Orden : {orderDto.OrderID} NO ha sido encontrada!.");
                }
                
                    order.CustomerID = orderDto.CustomerID;
                    order.EmployeeID = orderDto.EmployeeID;
                    order.Freight = orderDto.Freight;
                    order.OrderDate = orderDto.OrderDate;
                    order.RequiredDate = orderDto.RequiredDate;
                    order.ShipAddress = orderDto.ShipAddress;
                    order.ShipCity = orderDto.ShipCity;
                    order.ShipCountry = orderDto.ShipCountry;
                    order.ShipName = orderDto.ShipName;
                    order.ShippedDate = orderDto.ShippedDate;
                    order.ShipPostalCode = orderDto.ShipPostalCode;
                    order.ShipRegion = orderDto.ShipRegion;
                    order.ShipVia = orderDto.ShipVia;
                    //order.Order_Details = orderDto.Order_Details;

                NewLine();
                Console.WriteLine($"La Orden con ID : '{orderDto.OrderID}' ha sido modificada.");
                this.orderRepository.Update(order);
                this.orderRepository.SaveChanges();
            }
            catch
            {
                NewLine();
                Console.WriteLine($"Se produjo un ERROR al intentar Modificar la Orden con ID : '{orderDto.OrderID}'.");

                return;
            }

        }
        #endregion
        

        #region CREATE ORDER
        public void Create(OrderDto orderDto, ServicesController services)
        {
            
            var OrderDetails = new HashSet<Order_Detail>();

            try
            {
                foreach (var orderDetail in orderDto.Order_Details)
                {
                    OrderDetails.Add(new Order_Detail
                    {
                        Discount = orderDetail.Discount,
                        OrderID = orderDetail.OrderID,
                        ProductID = orderDetail.ProductID,
                        Product = services.productServices.GetProductByID(orderDetail.ProductID),
                        Quantity = orderDetail.Quantity,
                        UnitPrice = orderDetail.UnitPrice,
                    });
                }
            }
            catch
            {
                NewLine();
                Console.WriteLine("Se produjo un ERROR al agregar los Detalles de Orden ");
                Console.WriteLine("al intentar Crear una nueva Orden...");

                return;
            }

            try
            {
                this.orderRepository.Persist(new Order
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
                });
                
                this.orderRepository.SaveChanges();

            }
            catch
            {
                NewLine();
                Console.WriteLine($"Se produjo un ERROR de guardado al intentar Crear la nueva Orden.");

                return;
            }
        }

        #endregion

        
        #region REMOVE ORDER
        public void Remove(OrderDto orderDto, ServicesController services)
        {
            try
            {
                var deletedOrderId = orderDto.OrderID;

                var orderRemove = this.orderRepository.Set()
                    .FirstOrDefault(x => x.OrderID == orderDto.OrderID);

                if (orderRemove == null)
                    throw new Exception("La Orden no existe!");

                using (var detailsServices = new OrderDetailsServices())
                {
                    if (orderRemove.Order_Details.Any())
                    {
                        var detailsRemove = detailsServices.GetAll()
                                           .Where(d => d.OrderID == orderRemove.OrderID)
                                           .Select(d =>  new Order_Detail
                                           {
                                               OrderID = d.OrderID,
                                               Order = new Order
                                               {
                                                   OrderID = d.OrderID
                                               },
                                               Discount = d.Discount,
                                               Product = services.productServices.GetProductByID(d.ProductID),
                                               ProductID = d.ProductID,
                                               Quantity = d.Quantity,
                                               UnitPrice = d.UnitPrice,

                                           });
                                           
                        foreach (var detail in orderRemove.Order_Details)
                        {
                            var detailRemove = this.orderDetailsRepository.Set()
                                .FirstOrDefault(x => x.OrderID == orderRemove.OrderID);

                            if (detailRemove == null)
                                throw new Exception("No hay Detalle de Orden.");

                            NewLine();
                            Console.WriteLine($"El Detalle con ID de Orden : {detail.OrderID}, Producto : {detail.Product.ProductName}, Cantidad : {detail.Quantity} será eliminado.");

                            this.orderDetailsRepository.Remove(detailRemove);
                            this.orderDetailsRepository.SaveChanges();
                        }

                        NewLine();
                        Console.WriteLine($"Detalles eliminados.");
                    }
                    else
                    {
                        NewLine();
                        Console.WriteLine($"La Orden no tenía Detalles asociados.");
                    }

                    var order = this.orderRepository.Set()
                    .FirstOrDefault(x => x.OrderID == deletedOrderId);

                    if (order == null)
                    {
                        throw new Exception("El cliente no existe");
                    }

                    this.orderRepository.Remove(order);
                    this.orderRepository.SaveChanges();

                    NewLine();
                    Console.WriteLine($"La Orden con ID : {deletedOrderId} ha sido eliminada con éxito.");
                }
            }
            catch
            {
                NewLine();
                Console.WriteLine($"Se produjo un ERROR al intentar Eliminar la Orden con ID: '{orderDto.OrderID}'.");

                return;
            }
            
        }
        #endregion


        #region GET ALL ORDERS OF A CUSTOMER BY ID
        public ICollection<Order> GetAllByIDOfCustomer(string customerID,ServicesController services)
        {
            try
            {
                return this.orderRepository.Set()
                   .Where(o => o.CustomerID == customerID)
                   .Select(o => new Order
                   {
                       OrderID = o.OrderID,
                       CustomerID = o.CustomerID,
                       ShipCountry = o.ShipCountry,
                       Order_Details = o.Order_Details,

                   }).ToList();
            }
            catch
            {
                NewLine();
                Console.WriteLine("Se produjo un ERROR al intentar obtener todas las ");
                Console.WriteLine($"ordenes del Cliente con ID: '{customerID}'.");

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
    }
}