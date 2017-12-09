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
            var action = "";

            while (action != "F")
            {
                while (action != "F")
                //while (action != "M" && action != "D" && action != "C")
                {
                    NuevaLinea();
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("       M E N U");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("    O R D E N E S ");
                    NuevaLinea();

                    Console.WriteLine("Ingrese 'C' para Crear una Orden");
                    Console.WriteLine("Ingrese 'D' para Eliminar una Orden");
                    Console.WriteLine("Ingrese 'M' para Modificar una Orden");
                    Console.WriteLine("Ingrese 'L' para Listar todas las Ordenes");
                    Console.WriteLine("Ingrese 'P' para mostrar los siguientes datos por País: ");
                    Console.WriteLine("     - Cliente con mayor compra");
                    Console.WriteLine("     - Producto más vendido");
                    Console.WriteLine("Ingrese 'F' para Finalizar");

                    action = Console.ReadLine().ToUpper();
                    // action = action.ToUpper();

                    switch (action)
                    {
                        case "M":

                            var orderId = 0;
                            var orderUpdateDto = new OrderDto();

                            NuevaLinea();
                            Console.WriteLine("MODIFICAR ORDEN:");
                            Console.WriteLine("----------------");
                            NuevaLinea();
                            Console.WriteLine("Ingrese Id de la Orden a modificar:");
                            if (!int.TryParse(Console.ReadLine(), out orderId))
                            {
                                NuevaLinea();
                                Console.WriteLine($"El Id de Orden ingresado no es válido!");
                                break;
                            }

                            if (!string.IsNullOrWhiteSpace(orderId.ToString()))
                            {

                                var orderDtoFind = services.orderServices.GetOrderDtoByID(orderId, services);

                                if (orderDtoFind == null)
                                {
                                    break;
                                }
                                //services.customerServices.Modify(orderDtoFind);
                                orderUpdateDto = orderDtoFind;
                                //orderUpdateDto.OrderID = orderDtoFind.OrderID;
                                //orderUpdateDto.OrderID = orderId;
                                orderUpdateDto = OrderCreateEdit("UPDATE", services, orderUpdateDto);
                            }
                            else
                            {
                                break;
                            }

                            var lastAddedOrderMod = services.orderServices.GetAll(services)
                                .Where(o => o.CustomerID == orderUpdateDto.CustomerID && o.EmployeeID == orderUpdateDto.EmployeeID)
                                .LastOrDefault();

                            var totalAmountMod = orderUpdateDto.calculateTotalAmount();

                            NuevaLinea();
                            Console.WriteLine("ORDEN MODIFICADA CON ÉXITO");
                            /*
                            NuevaLinea();
                            Console.WriteLine($"La Orden con Id ''{lastAddedOrderMod.OrderID}'' con importe $ {totalAmountMod} se ha creado correctamente.");
                            */
                            NuevaLinea();
                            Console.WriteLine("PRESIONE CUALQUIER TECLA PARA CONTINUAR...");
                            Console.ReadKey();

                            break;

                        case "D":

                            var orderDtoRemove = new OrderDto();

                            do
                            {

                                NuevaLinea();
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
                                        NuevaLinea();
                                        Console.WriteLine("ERROR: No se encontró la Orden o no existe!");
                                        NuevaLinea();
                                        break;
                                    }
                                    else
                                    {
                                        if (orderDtoRemove.ShipCountry == "Mexico" || orderDtoRemove.ShipCountry == "France")
                                        {
                                            NuevaLinea();
                                            Console.WriteLine($"No se puede eliminar una Orden de un Cliente de ''{orderDtoRemove.ShipCountry}''!");
                                            NuevaLinea();
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
                                NuevaLinea();
                                Console.WriteLine("ERROR: No se encontró la Orden o no existe!");
                                NuevaLinea();
                                //}
                            }
                            while (orderDtoRemove == null);

                            break;

                        case "C":

                            NuevaLinea();
                            Console.WriteLine("CREAR ORDEN");
                            Console.WriteLine("-------------");


                            var orderDtoNew = new OrderDto();

                            var orderDto = OrderCreateEdit("CREATE", services, orderDtoNew);


                            var lastAddedOrder = services.orderServices.GetAll(services)
                                .Where(o => o.CustomerID == orderDto.CustomerID && o.EmployeeID == orderDto.EmployeeID)
                                .LastOrDefault();

                            if (lastAddedOrder == null)
                            {
                                Console.WriteLine("Hubo un error al intentar obtener la nueva Orden creada.");
                                break;
                            }

                            var totalAmount = orderDto.calculateTotalAmount();

                            if (totalAmount == 0)
                            {
                                Console.WriteLine("Hubo un error al intentar calcular el Importe de la Orden creada.");
                                break;
                            }

                            NuevaLinea();
                            Console.WriteLine("ORDEN CREADA CON ÉXITO");
                            NuevaLinea();
                            Console.WriteLine($"La Orden con ID ''{lastAddedOrder.OrderID}'' con importe $ {totalAmount} se ha creado correctamente.");
                            NuevaLinea();
                            Console.WriteLine("PRESIONE CUALQUIER TECLA PARA CONTINUAR...");
                            Console.ReadKey();

                            break;

                        case "L":

                            //LISTAR TODAS LAS ÓRDENES:
                            //mostrar Id de factura, Cliente Nombre e importe total:

                            NuevaLinea();
                            Console.WriteLine("LISTANDO TODAS LAS ÓRDENES...");
                            NuevaLinea();
                            var ordersList = services.orderServices.GetAll(services);

                            foreach (var order in ordersList)
                            {

                                var customerGetId = order.CustomerID;

                                if (customerGetId != null)
                                {
                                    var customer = services.customerServices.GetCustomerDtoByID(customerGetId, services);


                                    var orderDetails = services.orderDetailsServices.GetAll()//.ToList();
                                        .Where(d => d.OrderID == order.OrderID)
                                        .Select(d => d)
                                        .ToList();

                                    order.Order_Details = orderDetails;

                                    /*
                                    if (!order.Order_Details.Any())
                                    {
                                    var orderDetails2 = services.orderDetailsServices.GetAll()
                                                           .Where(d => d.OrderID == order.OrderID)
                                                           .Select(d => new OrderDetailDto
                                                           {
                                                               OrderID = d.OrderID,
                                                               Discount = d.Discount,
                                                               ProductID = d.ProductID,
                                                               Quantity = d.Quantity,
                                                               UnitPrice = d.UnitPrice,
                                                           }).ToList();

                                        order.Order_Details = orderDetails2;
                                    }
                                    */


                                    NuevaLinea();
                                    Console.WriteLine($"ID: {order.OrderID} - NOMBRE CLIENTE: ''{customer.ContactName}'' - IMPORTE TOTAL: ${order.calculateTotalAmount()}");
                                }

                            }
                            NuevaLinea();
                            Console.WriteLine("PRESIONE CUALQUIER TECLA PARA CONTINUAR...");
                            Console.ReadKey();
                            break;


                        case "P":

                            //mostrar por País: cliente con mayor compra y producto más vendido:

                            /*
                            var cliente = services.customerServices.GetAll()
                                .GroupBy(c => c.Country)

                            var cliente2 = services.customerServices.GetAll()
                            .Where(cliente => cliente - c == Gender.Masculine)
                            .Select(cliente => new PersonModel
                            {
                                Name = person.Name,
                                Weight = person.Weight
                            })
                            .Where(person => person.Weight > 70)
                            .Select(person => new PersonModel
                            {
                                Name = person.Name,
                                Weight = person.Weight
                            });
                    }

                    var cliente3 = services.customerServices.GetAll()
                        .GroupBy(x => new { x.Country }) //el GroupBy debe tener siempre una key pra agrupar
                        .Select(x => new CustomerDto//creo un nuevo tipo anónimo (puede tener variables nuevas, las creo abajo)
                                {
                            Country = x.Key.Country,

                             = x.Select(c => c.)
                        })
                        .Select(c => c.d
                        );


                       

                    var detalles = services.orderServices.GetAll(services)
                        .GroupBy(x => new { x.OrderID })
                        .Select(x => new CustomerDto
                        {
                            
                            //Country = x.Key.Country,
                        })
                        .Select(c => c.d
                        );
                        


                            var product = services.orderDetailsServices.GetAll()
                                .GroupBy(x => new
                                {
                                    x.ProductID
                                })
                                .Select(o => o.Sum(oi => oi.Quantity && o.Select(a => a.))
                                .ToList();
                                */


                            /*
                            var detalles = services.orderDetailsServices.GetAll()
                               .GroupBy(x => new { x.OrderID })
                               .Select(x => new CustomerDto
                               {
                                   =x.Key.,
                                    //Country = x.Key.Country,
                                })
                               .Select(c => c.d
                               );
                               */


                            //var coso = product.Select(c =>c.pro

                            NuevaLinea();
                            //Console.WriteLine("Aún no implementado...");
                            var bestCustomersList = services.customerServices.BestCostumer(services);

                            Console.WriteLine("LISTADO DE MEJORES CLIENTES (MEJORES COMPRADORES) POR PAÍS");
                            Console.WriteLine("----------------------------------------------------------");
                            
                            foreach (var bestCustomer in bestCustomersList)
                            {
                                NuevaLinea();
                                Console.WriteLine($"PAÍS : {bestCustomer.Country} - NOMBRE MEJOR CLIENTE : ''{bestCustomer.Name}''");
                            }
                            break;

                        default:

                            NuevaLinea();
                            Console.WriteLine("La opción no es válida.");
                            Console.WriteLine("Por favor selecciona una opción del menú.");

                            break;
                    }
                }
            }

            Console.WriteLine("Programa finalizado.");
        }



        public static OrderDto OrderCreateEdit(string action, ServicesController services, OrderDto orderDto)
        {
            //var orderDto = new OrderDto();


            //CustomerID
            NuevaLinea();
            Console.WriteLine("Ingrese el ID de Cliente de la Orden (5 letras):");
            orderDto.CustomerID = Console.ReadLine().ToUpper();


            //OBTENER EMPLEADO POR NOMBRE Y APELLIDO
            var employeeDto = new EmployeeDto();

            do
            {
                NuevaLinea();
                Console.WriteLine("Ingrese el Nombre del Empleado que realiza la Orden:");
                var employeeFirstName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(employeeFirstName))
                {
                    employeeFirstName = FirstCharToUpper(employeeFirstName);
                }

                NuevaLinea();
                Console.WriteLine("Ingrese el Apellido del Empleado que realiza la Orden:");
                var employeeLastName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(employeeLastName))
                {
                    employeeLastName = FirstCharToUpper(employeeLastName);
                }

                employeeDto = services.employeeServices.GetAll()
                    .Where(e => e.FirstName == employeeFirstName && e.LastName == employeeLastName)
                    .Select(e => e)
                    .FirstOrDefault();

                if (employeeDto == null)
                {
                    NuevaLinea();
                    Console.WriteLine($"No existe ningún Empleado con el nombre ''{employeeFirstName}'' y apellido ''{employeeLastName}'' ingresados!");
                    Console.WriteLine($"Por favor ingrese nuevamente...");
                }
                else
                {
                    orderDto.EmployeeID = employeeDto.EmployeeID;
                }
            }
            while (employeeDto == null);

            NuevaLinea();
            Console.WriteLine($"Se encontró el Empleado llamado : ''{employeeDto.FirstName} {employeeDto.LastName}''.");
            //Console.WriteLine($"Se encontró el Empleado con nombre : ''{employeeDto.FirstName}'' y apellido : ''{employeeDto.LastName}''.");
            orderDto.EmployeeID = orderDto.EmployeeID;


            //OrderDate
            NuevaLinea();
            //Console.WriteLine("Ingrese Fecha de Orden:");
            //orderDto.OrderDate = Console.ReadLine();
            DateTime orderDate;
            do
            {
                NuevaLinea();
                Console.WriteLine("Ingrese la Fecha de Orden (formato dd/MM/yyyy):");
            }
            while (!(DateTime.TryParseExact(Console.ReadLine(),
                                        "dd/MM/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                out orderDate)));
            orderDto.OrderDate = orderDate;


            //RequiredDate
            NuevaLinea();
            //Console.WriteLine("Ingrese Fecha de Orden:");
            //orderDto.OrderDate = Console.ReadLine();
            DateTime requiredDate;
            do
            {
                NuevaLinea();
                Console.WriteLine("Ingrese la Fecha Requerida de Orden (formato dd/MM/yyyy):");
            }
            while (!(DateTime.TryParseExact(Console.ReadLine(),
                                        "dd/MM/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                out requiredDate)));
            orderDto.RequiredDate = requiredDate;


            //ShippedDate
            NuevaLinea();
            //Console.WriteLine("Ingrese Fecha de Orden:");
            //orderDto.OrderDate = Console.ReadLine();
            DateTime shippedDate;
            do
            {
                NuevaLinea();
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
                NuevaLinea();
                Console.WriteLine("Ingrese Via de Embarcado de la Orden ( 1, 2, 3):");
                Console.WriteLine(" 1- Speedy Express");
                Console.WriteLine(" 2- United Package");
                Console.WriteLine(" 3- Federal Shipping");
                //orderDto.EmployeeID = Console.ReadLine();

                int.TryParse(Console.ReadLine(), out shipVia);
                orderDto.ShipVia = shipVia;
            }

            while (shipVia < 1 && shipVia > 3);


            //Freight
            NuevaLinea();
            Console.WriteLine("Ingrese la Carga del envío:");
            //orderDto.CustomerID = Console.ReadLine();
            decimal freight;
            decimal.TryParse(Console.ReadLine(), out freight);
            orderDto.Freight = freight;

            //ShipName
            NuevaLinea();
            Console.WriteLine("Ingrese el Nombre de envío de la Orden:");
            orderDto.ShipName = Console.ReadLine();

            //ShipCity
            NuevaLinea();
            Console.WriteLine("Ingrese Ciudad de Envío de la Orden:");
            orderDto.ShipCity = Console.ReadLine();

            //ShipRegion
            NuevaLinea();
            Console.WriteLine("Ingrese la Región de envío de la Orden:");
            orderDto.ShipRegion = Console.ReadLine();

            //ShipPostalCode
            NuevaLinea();
            Console.WriteLine("Ingrese Código Postal de la Orden:");
            orderDto.ShipPostalCode = Console.ReadLine();

            //ShipAddress
            NuevaLinea();
            Console.WriteLine("Ingrese la Dirección de envío Orden:");
            orderDto.ShipAddress = Console.ReadLine();

            //ShipCountry
            NuevaLinea();
            Console.WriteLine("Ingrese País destino de la Orden:");
            orderDto.ShipCountry = Console.ReadLine();


            //COMPRUEBO SI ESTOY CREANDO O MODIFICANDO:
            if (action == "CREATE")
            {

                //AGREGO ORDER_DETAILS
                NuevaLinea();
                Console.WriteLine("DETALLES DE ORDEN:");
                Console.WriteLine("------------------");
                Console.WriteLine("Por favor ingrese los datos de la Línea de Orden:");
                NuevaLinea();

                //CREAR LÍNEAS-DETALLES DE ORDEN:

                var addDetail = "";

                do
                {
                    if (addDetail == "S")
                    {
                        NuevaLinea();
                        Console.WriteLine("Desea seguir ingresando líneas de Detalle de Orden?");
                        addDetail = Console.ReadLine().ToUpper();
                    }
                    else
                    {
                        addDetail = "S";
                    }

                    switch (addDetail)
                    {
                        case "S":

                            //INICIO UNA NUEVA LÍNEA DE DETALLE DE ORDEN:
                            //NOMBRE DE PRODUCTO
                            var productName = "";

                            var productToAdd = new ProductDto();

                            do
                            {
                                NuevaLinea();
                                Console.WriteLine("Ingrese el Producto (nombre):");
                                productName = Console.ReadLine();

                                if (!string.IsNullOrWhiteSpace(productName))
                                {
                                    productName = FirstCharToUpper(productName);

                                    productToAdd = services.productServices.GetByName(productName);

                                    if (productToAdd == null)
                                    {
                                        Console.WriteLine($"No se encontró el Producto de nombre : ''{ productName}''");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Por favor ingrese un nombre de Producto válido...");
                                }
                            }
                            while (string.IsNullOrWhiteSpace(productName) || productToAdd == null);

                            //CANTIDAD DE PRODUCTO
                            short quantity = 0;

                            do
                            {
                                NuevaLinea();
                                Console.WriteLine($"Ingrese la Cantidad de Producto (''{productToAdd.ProductName}''):");
                                Console.WriteLine($"(La Cantidad debe ser mayor a 0)");
                                //orderDto.EmployeeID = Console.ReadLine();

                                if (!short.TryParse(Console.ReadLine(), out quantity))
                                {
                                    NuevaLinea();
                                    Console.WriteLine("Por favor ingrese una Cantidad correcta!");
                                }
                            }
                            while (quantity <= 0);

                            //DESCUENTO DE PRODUCTO
                            var discount = 0.0f;

                            do
                            {
                                NuevaLinea();
                                Console.WriteLine($"Ingrese el Descuento para el Producto (''{productToAdd.ProductName}''):");
                                Console.WriteLine($"(De 0 a 30)");
                                //orderDto.EmployeeID = Console.ReadLine();

                                if (!float.TryParse(Console.ReadLine(), out discount))
                                {
                                    NuevaLinea();
                                    Console.WriteLine("Por favor ingrese un Descuento correcto!");
                                    discount = -1;
                                }
                                discount = discount / 100;
                            }
                            while (discount < 0f || discount > 0.30f);



                            //if (action == "CREATE")
                            //{
                            //AGREGO LINEAS A LA ORDEN
                            orderDto.Order_Details.Add(new OrderDetailDto
                            {
                                Product = productToAdd,
                                ProductID = productToAdd.ProductID,
                                UnitPrice = decimal.Parse((productToAdd.UnitPrice).ToString()),
                                Discount = discount,
                                Quantity = quantity,
                            });

                            //LÍNEA CREADA Y AGREGADA CON ÉXITO A LA ORDEN:
                            NuevaLinea();
                            Console.WriteLine("La línea de Detalle de Orden se ha agregado correctamente.");
                            //}
                            //else if (action == "UPDATE")
                            //{

                            //LÍNEA DE LA ORDEN MODIFICADA CON ÉXITO:
                            //NuevaLinea();
                            //Console.WriteLine("La línea de Detalle de Orden se ha modificado correctamente.");
                            //}


                            break;

                        case "N":

                            NuevaLinea();
                            Console.WriteLine("No se agregarán más líneas de Detalle a la Orden.");

                            break;

                        default:

                            NuevaLinea();
                            Console.WriteLine("Ingrese una opción válida!");
                            NuevaLinea();
                            Console.WriteLine("Desea seguir ingresando líneas de Detalle de Orden?");
                            addDetail = Console.ReadLine().ToUpper();
                            break;
                    }
                }
                while (addDetail != "N");


                //CREAR LA ORDEN:
                services.orderServices.Create(orderDto, services);
            }
            else if (action == "UPDATE")
            {
                services.orderServices.Modify(orderDto, services);
            }

            return orderDto;
        }




        public static string FirstCharToUpper(string input)
        {
            input = input.ToLower();

            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} Error! EL valor ingresado es incorrecto!", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static void NuevaLinea()
        {
            Console.WriteLine("");
        }
    }
}
