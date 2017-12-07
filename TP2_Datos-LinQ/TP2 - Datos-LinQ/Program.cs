using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using Dtos;
using System.Globalization;

namespace TP2_Datos_LinQ
{
    class Program
    {
        static void Main(string[] args)
        {

            //var servicesController = new ServicesController();
            var services = new ServicesController();


            /*
            var customers = services.GetAll();

            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.CustomerID}   {customer.ContactName}");
            }
            
            Console.ReadLine();
            */
            var accion = "";

            while (accion != "F")
            {
                while (accion != "F")
                //while (accion != "M" && accion != "D" && accion != "C")
                {
                    Console.WriteLine("");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("       M E N U");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("    O R D E N E S ");
                    Console.WriteLine("");
                    Console.WriteLine("Ingrese 'M' para Modificar una Orden");
                    Console.WriteLine("Ingrese 'D' para Eliminar una Orden");
                    Console.WriteLine("Ingrese 'C' para Crear una Orden");
                    Console.WriteLine("Ingrese 'L' para Listar todas las Ordenes");
                    Console.WriteLine("Ingrese 'F' para Finalizar");

                    accion = Console.ReadLine().ToUpper();
                    // accion = accion.ToUpper();

                    switch (accion)
                    {
                        case "M":

                            var customerId = "";

                            Console.WriteLine("MODIFICAR:");
                            Console.WriteLine("----------");
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese Id del cliente a modificar:");

                            customerId = Console.ReadLine();

                            if (customerId != null)
                            {

                                var customerDtoFind = services.customerServices.GetCustomerDtoByID(customerId,services);

                                if (customerDtoFind != null)
                                    services.customerServices.Modify(customerDtoFind);
                            }
                            else
                            {
                                break;
                            }

                            break;

                        case "D":
                            /*
                            Console.WriteLine("ELIMINAR:");
                            Console.WriteLine("----------");
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese Id de la Orden a eliminar:");
                            */

                            var orderDtoRemove = new OrderDto();

                            do
                            {

                                Console.WriteLine("");
                                Console.WriteLine("ELIMINAR ORDEN:");
                                Console.WriteLine("-----------------");
                                Console.WriteLine("Ingrese el ID de la Orden a ELIMINAR:");

                                //var orderRemoveId = Console.ReadLine();

                                var orderRemoveId = -1;

                                //try
                                //{
                                bool isInt = int.TryParse(Console.ReadLine(), out orderRemoveId);

                                //if (int.TryParse(Console.ReadLine(), out orderRemoveId))
                                if (isInt)
                                {
                                    orderDtoRemove = services.orderServices.GetOrderDtoByID(orderRemoveId, services);

                                    if (orderDtoRemove == null)
                                    {
                                        //throw new Exception("ERROR: No se encontró la Orden o no existe!");
                                        Console.WriteLine("");
                                        Console.WriteLine("ERROR: No se encontró la Orden o no existe!");
                                        Console.WriteLine("");
                                        break;
                                    }
                                    else
                                    {
                                        if (orderDtoRemove.ShipCountry == "Mexico" || orderDtoRemove.ShipCountry == "France")
                                        {
                                            Console.WriteLine("");
                                            Console.WriteLine($"No se puede eliminar una Orden de un Cliente de ''{orderDtoRemove.ShipCountry}''!");
                                            Console.WriteLine("");
                                            break;
                                        }
                                        else
                                        {
                                            services.orderServices.Remove(orderDtoRemove, services);
                                            break;
                                        }
                                    }

                                }

                                orderDtoRemove = null;
                                //}
                                //catch
                                //{
                                //throw new Exception("ERROR: No se encontró la Orden o no existe!");
                                Console.WriteLine("");
                                Console.WriteLine("ERROR: No se encontró la Orden o no existe!");
                                Console.WriteLine("");
                                //}
                            }
                            while (orderDtoRemove == null);

                            break;

                        case "C":

                            Console.WriteLine("");
                            Console.WriteLine("CREAR ORDEN");
                            Console.WriteLine("-------------");

                            var orderDto = new OrderDto();

                            /*
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
                            ShipCountry = orderDto.ShipCountry,
                            */

                            //OrderID
                            //Console.WriteLine("");
                            //Console.WriteLine("Ingrese el ID de la Orden:");
                            //orderDto.OrderID = Console.ReadLine();

                            //CustomerID
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese el ID de Cliente de la Orden (5 letras):");
                            orderDto.CustomerID = Console.ReadLine().ToUpper();

                            //EmployeeID
                            /*
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese ID de Empleado de la Orden:");
                            //orderDto.EmployeeID = Console.ReadLine();
                            int employeeID;
                            int.TryParse(Console.ReadLine(), out employeeID);
                            orderDto.EmployeeID = employeeID;
                            */

                            var employeeDto = new EmployeeDto();

                            //OBTENER EMPLEADO POR NOMBRE Y APELLIDO
                            do
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Ingrese el Nombre del Empleado que realiza la Orden:");
                                var employeeFirstName = Console.ReadLine();

                                Console.WriteLine("");
                                Console.WriteLine("Ingrese el Apellido del Empleado que realiza la Orden:");
                                var employeeLastName = Console.ReadLine();

                                employeeDto = services.employeeServices.GetAll()
                                    .Where(e => e.FirstName == employeeFirstName && e.LastName == employeeLastName)
                                    .Select(e => e)
                                    .FirstOrDefault();

                                if (employeeDto == null)
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine($"No existe ningún Empleado con el nombre y apellido ingresado!");
                                    Console.WriteLine($"Por favor ingrese nuevamente...");
                                } 
                                else
                                {
                                    orderDto.EmployeeID = employeeDto.EmployeeID;
                                }
                            }
                            while (employeeDto == null);

                            Console.WriteLine("");
                            Console.WriteLine($"Se encontró el Empleado con nombre : ''{employeeDto.FirstName}'' y apellido : ''{employeeDto.LastName}''.");
                            orderDto.EmployeeID = orderDto.EmployeeID;

                            //OrderDate
                            Console.WriteLine("");
                            //Console.WriteLine("Ingrese Fecha de Orden:");
                            //orderDto.OrderDate = Console.ReadLine();
                            DateTime orderDate;
                            do
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Ingrese la Fecha de Orden (formato dd/MM/yyyy):");
                            }
                            while (!(DateTime.TryParseExact(Console.ReadLine(),
                                                        "dd/MM/yyyy",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None,
                                out orderDate)));
                            orderDto.OrderDate = orderDate;


                            //RequiredDate
                            Console.WriteLine("");
                            //Console.WriteLine("Ingrese Fecha de Orden:");
                            //orderDto.OrderDate = Console.ReadLine();
                            DateTime requiredDate;
                            do
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Ingrese la Fecha Requerida de Orden (formato dd/MM/yyyy):");
                            }
                            while (!(DateTime.TryParseExact(Console.ReadLine(),
                                                        "dd/MM/yyyy",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None,
                                out requiredDate)));
                            orderDto.RequiredDate = requiredDate;


                            //ShippedDate
                            Console.WriteLine("");
                            //Console.WriteLine("Ingrese Fecha de Orden:");
                            //orderDto.OrderDate = Console.ReadLine();
                            DateTime shippedDate;
                            do
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Ingrese la Fecha de Embarco de Orden (formato dd/MM/yyyy):");
                            }
                            while (!(DateTime.TryParseExact(Console.ReadLine(),
                                                        "dd/MM/yyyy",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None,
                                out shippedDate)));
                            orderDto.ShippedDate = shippedDate;


                            //ShipVia
                            int shipVia;
                            do
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Ingrese Via de Embarcado de la Orden ( 1, 2, 3):");
                                Console.WriteLine(" 1- Speedy Express");
                                Console.WriteLine(" 2- United Package");
                                Console.WriteLine(" 3- Federal Shipping");
                                //orderDto.EmployeeID = Console.ReadLine();
                                
                                int.TryParse(Console.ReadLine(), out shipVia);
                                orderDto.ShipVia = shipVia;
                            }

                            while (shipVia <1 && shipVia > 3);


                            //Freight
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese la Carga del envío:");
                            //orderDto.CustomerID = Console.ReadLine();
                            decimal freight;
                            decimal.TryParse(Console.ReadLine(), out freight);
                            orderDto.Freight = freight;

                            //ShipName
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese el Nombre de envío de la Orden:");
                            orderDto.ShipName = Console.ReadLine();

                            //ShipCity
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese Ciudad de Envío de la Orden:");
                            orderDto.ShipCity = Console.ReadLine();

                            //ShipRegion
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese la Región de envío de la Orden:");
                            orderDto.ShipRegion = Console.ReadLine();

                            //ShipPostalCode
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese Código Postal de la Orden:");
                            orderDto.ShipPostalCode = Console.ReadLine();

                            //ShipAddress
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese la Dirección de envío Orden:");
                            orderDto.ShipAddress = Console.ReadLine();

                            //ShipCountry
                            Console.WriteLine("");
                            Console.WriteLine("Ingrese País destino de la Orden:");
                            orderDto.ShipCountry = Console.ReadLine();


                            //TRAIGO Y MAPEO SÓLO LOS DATOS QUE NECESITO
                            /*
                            Address = c.Address,
                            City = c.City,
                            CompanyName = c.CompanyName,
                            OrderID = c.OrderID,
                            ContactName = c.ContactName
                            */

                            services.orderServices.Create(orderDto, services);

                            break;

                        case "L":

                            //LISTAR TODAS LAS ÓRDENES:
                            //mostrar Id de factura, Cliente Nombre e importe total:


                            break;

                        default:

                            Console.WriteLine("");
                            Console.WriteLine("La opción no es válidad.");
                            Console.WriteLine("Por favor seleccion una opción del menú.");

                            break;
                    }
                }
            }

            Console.WriteLine("Programa finalizado.");



        }
    }
}
