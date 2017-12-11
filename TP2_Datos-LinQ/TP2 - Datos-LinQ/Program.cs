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

                    //services.customerServices.UPDATE(services);
                    

                    NewLine();
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("       M E N U");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("    O R D E N E S ");
                    NewLine();

                    Console.WriteLine("- 'C' para Crear una Orden");
                    Console.WriteLine("- 'D' para Eliminar una Orden");
                    Console.WriteLine("- 'M' para Modificar una Orden");
                    Console.WriteLine("- 'L' para Listar todas las Ordenes");
                    Console.WriteLine("- 'A' para mostrar Cliente con mayor compras por País");
                    Console.WriteLine("- 'B' para mostrar Producto más vendido por País");
                    Console.WriteLine("Ingrese 'F' para Finalizar");

                    action = Console.ReadLine().ToUpper();
                    // action = action.ToUpper();

                    switch (action)
                    {
                        case "M":

                            var orderId = 0;
                            var orderUpdateDto = new OrderDto();

                            NewLine();
                            Console.WriteLine("MODIFICAR ORDEN:");
                            Console.WriteLine("----------------");
                            NewLine();
                            Console.WriteLine("Ingrese Id de la Orden a modificar:");
                            if (!int.TryParse(Console.ReadLine(), out orderId))
                            {
                                NewLine();
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
                                //services.customerServices.Update(orderDtoFind);
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

                            NewLine();
                            Console.WriteLine("ORDEN MODIFICADA CON ÉXITO");
                            /*
                            NewLine();
                            Console.WriteLine($"La Orden con Id '{lastAddedOrderMod.OrderID}' con importe $ {totalAmountMod} se ha creado correctamente.");
                            */
                            NewLine();
                            Console.WriteLine("PRESIONE CUALQUIER TECLA PARA CONTINUAR...");
                            Console.ReadKey();

                            break;

                        case "D":

                            var orderDtoRemove = new OrderDto();

                            do
                            {

                                NewLine();
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
                                        NewLine();
                                        Console.WriteLine("ERROR: No se encontró la Orden o no existe!");
                                        NewLine();
                                        break;
                                    }
                                    else
                                    {
                                        if (orderDtoRemove.ShipCountry == "Mexico" || orderDtoRemove.ShipCountry == "France")
                                        {
                                            NewLine();
                                            Console.WriteLine($"No se puede eliminar una Orden de un Cliente de '{orderDtoRemove.ShipCountry}'!");
                                            NewLine();
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
                                NewLine();
                                Console.WriteLine("ERROR: No se encontró la Orden o no existe!");
                                NewLine();
                                //}
                            }
                            while (orderDtoRemove == null);

                            break;

                        case "C":

                            NewLine();
                            Console.WriteLine("CREAR ORDEN");
                            Console.WriteLine("-------------");


                            var orderDtoNew = new OrderDto();

                            var orderDto = OrderCreateEdit("CREATE", services, orderDtoNew);

                            if (orderDto == null)
                            {
                                NewLine();
                                Console.WriteLine("La Orden no se ha generado.");
                                break;
                            }

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

                            NewLine();
                            Console.WriteLine("ORDEN CREADA CON ÉXITO");
                            NewLine();
                            Console.WriteLine($"La Orden con ID '{lastAddedOrder.OrderID}' con importe $ {totalAmount} se ha creado correctamente.");
                            NewLine();
                            Console.WriteLine("PRESIONE CUALQUIER TECLA PARA CONTINUAR...");
                            Console.ReadKey();

                            break;

                        case "L":

                            //LISTAR TODAS LAS ÓRDENES:
                            //mostrar Id de factura, Cliente Nombre e importe total:

                            NewLine();
                            Console.WriteLine("LISTANDO TODAS LAS ÓRDENES...");
                            NewLine();

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


                                    NewLine();
                                    Console.WriteLine($"ID: {order.OrderID} - NOMBRE CLIENTE: '{customer.ContactName}' - IMPORTE TOTAL: ${order.calculateTotalAmount()}");
                                }

                            }

                            PressKeyToContinue();
                            break;

                        case "A":

                            //mostrar por País: cliente con mayor compra:
                            NewLine();
                            Console.WriteLine("LISTANDO CLIENTES...");
                            NewLine();

                            var bestCustomersList = services.customerServices.GetBestCostumer(services);

                            NewLine();
                            Console.WriteLine("LISTADO DE MEJORES CLIENTES (MEJORES COMPRADORES) POR PAÍS");
                            Console.WriteLine("----------------------------------------------------------");
                            
                            foreach (var bestCustomer in bestCustomersList)
                            {
                                NewLine();
                                Console.WriteLine($"PAÍS : {bestCustomer.Country} - NOMBRE MEJOR CLIENTE : '{bestCustomer.Name}'");
                            }

                            PressKeyToContinue();
                            break;

                        case "B":

                            //mostrar por País: producto más vendido:
                            NewLine();
                            Console.WriteLine("LISTANDO PRODUCTOS...");
                            NewLine();

                            var bestSellerProductList = services.productServices.GetBestSellProduct(services);

                            NewLine();
                            Console.WriteLine("LISTADO DEL PRODUCTO MÁS VENDIDO POR PAÍS");
                            Console.WriteLine("-----------------------------------------");

                            foreach (var bestSellerProduct in bestSellerProductList)
                            {
                                NewLine();
                                Console.WriteLine($"PAÍS : {bestSellerProduct.Country} - PRODUCTO MÁS VENDIDO : '{bestSellerProduct.Name}'");
                            }

                            PressKeyToContinue();
                            break;

                        case "F":

                            NewLine();
                            Console.WriteLine("Saliendo...");

                            break;

                        default:

                            NewLine();
                            Console.WriteLine("La opción no es válida.");
                            Console.WriteLine("Por favor selecciona una opción del menú.");

                            break;
                    }
                }
            }

            NewLine();
            Console.WriteLine("Programa finalizado.");
            Console.ReadKey();
        }



        public static OrderDto OrderCreateEdit(string action, ServicesController services, OrderDto orderDto)
        {
            //var orderDto = new OrderDto();


            //CustomerID
            NewLine();
            Console.WriteLine("Ingrese el ID de Cliente de la Orden (5 letras):");
            orderDto.CustomerID = Console.ReadLine().ToUpper();


            //OBTENER EMPLEADO POR NOMBRE Y APELLIDO
            var employeeDto = new EmployeeDto();

            do
            {
                NewLine();
                Console.WriteLine("Ingrese el Primer Nombre del Empleado que realiza la Orden:");
                var employeeFirstName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(employeeFirstName))
                {
                    employeeFirstName = FirstCharToUpper(employeeFirstName);
                }

                NewLine();
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
                    NewLine();
                    Console.WriteLine($"No existe ningún Empleado con el nombre '{employeeFirstName}' y apellido '{employeeLastName}' ingresados!");
                    Console.WriteLine($"Por favor ingrese nuevamente...");
                }
                else
                {
                    orderDto.EmployeeID = employeeDto.EmployeeID;
                }
            }
            while (employeeDto == null);

            NewLine();
            Console.WriteLine($"Se encontró el Empleado llamado : '{employeeDto.FirstName} {employeeDto.LastName}'.");
            //Console.WriteLine($"Se encontró el Empleado con nombre : '{employeeDto.FirstName}' y apellido : '{employeeDto.LastName}'.");
            orderDto.EmployeeID = orderDto.EmployeeID;


            //OrderDate
            NewLine();
            //Console.WriteLine("Ingrese Fecha de Orden:");
            //orderDto.OrderDate = Console.ReadLine();
            DateTime orderDate;
            do
            {
                NewLine();
                Console.WriteLine("Ingrese la Fecha de Orden (formato dd/MM/yyyy):");
            }
            while (!(DateTime.TryParseExact(Console.ReadLine(),
                                        "dd/MM/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                out orderDate)));
            orderDto.OrderDate = orderDate;


            //RequiredDate
            NewLine();
            //Console.WriteLine("Ingrese Fecha de Orden:");
            //orderDto.OrderDate = Console.ReadLine();
            DateTime requiredDate;
            do
            {
                NewLine();
                Console.WriteLine("Ingrese la Fecha Requerida de Orden (formato dd/MM/yyyy):");
            }
            while (!(DateTime.TryParseExact(Console.ReadLine(),
                                        "dd/MM/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                out requiredDate)));
            orderDto.RequiredDate = requiredDate;


            //ShippedDate
            NewLine();
            //Console.WriteLine("Ingrese Fecha de Orden:");
            //orderDto.OrderDate = Console.ReadLine();
            DateTime shippedDate;
            do
            {
                NewLine();
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
                NewLine();
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
            NewLine();
            Console.WriteLine("Ingrese la Carga del envío:");
            //orderDto.CustomerID = Console.ReadLine();
            decimal freight;
            decimal.TryParse(Console.ReadLine(), out freight);
            orderDto.Freight = freight;

            //ShipName
            NewLine();
            Console.WriteLine("Ingrese el Nombre de envío de la Orden:");
            orderDto.ShipCountry = FirstCharToUpper(Console.ReadLine());

            //ShipCity
            NewLine();
            Console.WriteLine("Ingrese Ciudad de Envío de la Orden:");
            orderDto.ShipCity = FirstCharToUpper(Console.ReadLine());

            //ShipRegion
            NewLine();
            Console.WriteLine("Ingrese la Región de envío de la Orden:");
            orderDto.ShipRegion = FirstCharToUpper(Console.ReadLine());

            //ShipPostalCode
            NewLine();
            Console.WriteLine("Ingrese Código Postal de la Orden:");
            orderDto.ShipPostalCode = Console.ReadLine();

            //ShipAddress
            NewLine();
            Console.WriteLine("Ingrese la Dirección de envío Orden:");
            orderDto.ShipAddress = FirstCharToUpper(Console.ReadLine());

            //ShipCountry
            NewLine();
            Console.WriteLine("Ingrese País destino de la Orden:");
            orderDto.ShipCountry = FirstCharToUpper(Console.ReadLine());

            //COMPRUEBO SI ESTOY CREANDO O MODIFICANDO:
            if (action == "CREATE")
            {

                //AGREGO ORDER_DETAILS
                NewLine();
                Console.WriteLine("DETALLES DE ORDEN:");
                Console.WriteLine("------------------");
                Console.WriteLine("Por favor ingrese los datos de la Línea de Orden:");
                NewLine();

                //CREAR LÍNEAS-DETALLES DE ORDEN:

                var addDetail = "";
                var addedProductsNames = new List<string>();

                do
                {
                    if (addDetail == "S")
                    {
                        NewLine();
                        Console.WriteLine("Desea seguir ingresando líneas de Detalle de Orden? (S - N)");
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
                            //var addedProductsNames = new List<string>();

                            var productName = "";

                            var productReapeated = false;

                            var productToAdd = new ProductDto();

                            do
                            {
                                NewLine();
                                Console.WriteLine("Ingrese el Producto (nombre):");
                                productName = Console.ReadLine();

                                if (!string.IsNullOrWhiteSpace(productName))
                                {
                                    productName = FirstCharToUpper(productName);

                                    productToAdd = services.productServices.GetByName(productName);

                                    if (productToAdd == null)
                                    {
                                        NewLine();
                                        Console.WriteLine($"No se encontró el Producto de nombre : '{ productName}'");
                                    }

                                    productReapeated = false;

                                    foreach (var product in addedProductsNames)
                                    {

                                        if(product == productName)
                                        {
                                            NewLine();
                                            Console.WriteLine($"No se puede agregar otra línea con : '{ productName}'");
                                            Console.WriteLine("Ya se ha ingresado una línea con ese producto.");
                                            Console.WriteLine("Asegúrese de ingresar correctamente la cantidad cuando ingrese");
                                            Console.WriteLine("un nuevo producto para evitar este problema.");

                                            productReapeated = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    NewLine();
                                    Console.WriteLine($"Por favor ingrese un nombre de Producto válido...");
                                }
                            }
                            while (string.IsNullOrWhiteSpace(productName) || productToAdd == null || productReapeated);


                            //EL PRODUCTO EXISTE, CONTINUAR...

                            //GUARDO EL NONBRE DEL PRODUCTO QUE ESTOY INGRESANDO, PARA QUE NO VUELVA
                            //A HABER OTRA LINEA DE DETALLE DE ORDEN CON EL MISMO ID DE PRODUCTO
                            //(MISMO ID DE PRODUCTO Y ID DE ORDEN GENERA CONFLICTO DE CLAVES PRIMARIAS
                            //AL CREAR UNA LINEA DE DETALLE DE ORDEN
                            //
                            addedProductsNames.Add(productName);

                            //CANTIDAD DE PRODUCTO
                            short quantity = 0;

                            do
                            {
                                NewLine();
                                Console.WriteLine($"Ingrese la Cantidad de Producto ('{productToAdd.ProductName}'):");
                                Console.WriteLine($"(La Cantidad debe ser mayor a 0)");
                                //orderDto.EmployeeID = Console.ReadLine();

                                if (!short.TryParse(Console.ReadLine(), out quantity))
                                {
                                    NewLine();
                                    Console.WriteLine("Por favor ingrese una Cantidad correcta!");
                                }
                            }
                            while (quantity <= 0);

                            //DESCUENTO DE PRODUCTO
                            var discount = 0.0f;

                            do
                            {
                                NewLine();
                                Console.WriteLine($"Ingrese el Descuento para el Producto ('{productToAdd.ProductName}'):");
                                Console.WriteLine($"(De 0 a 30)");
                                //orderDto.EmployeeID = Console.ReadLine();

                                if (!float.TryParse(Console.ReadLine(), out discount))
                                {
                                    NewLine();
                                    Console.WriteLine("Por favor ingrese un Descuento correcto!");
                                    discount = -1;
                                }
                                discount = discount / 100;
                            }
                            while (discount < 0f || discount > 0.30f);

                            
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
                            NewLine();
                            Console.WriteLine("La línea de Detalle de Orden se ha agregado correctamente.");
                            //}
                            //else if (action == "UPDATE")
                            //{

                            //LÍNEA DE LA ORDEN MODIFICADA CON ÉXITO:
                            //NewLine();
                            //Console.WriteLine("La línea de Detalle de Orden se ha modificado correctamente.");
                            //}


                            break;

                        case "N":

                            NewLine();
                            Console.WriteLine("No se agregarán más líneas de Detalle a la Orden.");

                            break;

                        default:

                            NewLine();
                            Console.WriteLine("Ingrese una opción válida!");
                            NewLine();
                            Console.WriteLine("Desea seguir ingresando líneas de Detalle de Orden? (S / N)");
                            addDetail = Console.ReadLine().ToUpper();
                            break;
                    }
                }
                while (addDetail != "N");

                //
                //ORDEN COMPLETA, GENERARLA Y GUARDARLA???:
                NewLine();
                Console.WriteLine("Se ha terminado de armar la Orden");
                NewLine();
                Console.WriteLine("Desea confimarla y guardarla? (S / N)");

                var response = Console.ReadLine().ToUpper();

                //var generateOrder = false;

                do
                {
                    if (response == "S")
                    {
                        NewLine();
                        Console.WriteLine("Generando Orden...");

                        //generateOrder = true;
                    }
                    else if (response == "N")
                    {
                        NewLine();
                        Console.WriteLine("¡Orden CANCELADA!");

                        //break;
                        return null;
                    }
                    else
                    {
                        Console.WriteLine("Por favor ingrese una opción válida...");
                    }
                }
                while (response != "S");

                //if (!generateOrder) return null;

                //CREAR LA ORDEN:
                services.orderServices.Create(orderDto, services);
            }
            else if (action == "UPDATE")
            {
                services.orderServices.Update(orderDto, services);
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

        public static void NewLine()
        {
            Console.WriteLine("");
        }

        public static void PressKeyToContinue()
        {
            NewLine();
            Console.WriteLine(".............................................");
            Console.WriteLine("...PRESIONE CUALQUIER TECLA PARA CONTINUAR...");
            Console.WriteLine(".............................................");
            Console.ReadKey();
        }
    }
}
