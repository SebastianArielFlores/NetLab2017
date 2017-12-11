using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Dtos;

namespace Services
{
    public class CustomerServices
    {
        /* 
         * HACER ALTA, BAJA, MODIFICACIÓN DE CLIENTES (Customers)
         *
         */

        Repository<Customer> customerRepository;

        Repository<Employee> employeeRepository;

        public CustomerServices()
        {
            customerRepository = new Repository<Customer>();

            employeeRepository = new Repository<Employee>();
        }
        


        #region GET ALL CUSTOMERS DTO
        public IEnumerable<CustomerDto> GetAllDto()
        {
            return customerRepository.Set()
                   .ToList()
                   .Select(c => new CustomerDto
                   {
                       Country = c.Country,
                       ContactName = c.ContactName,

                   }).ToList();

            
        }
        #endregion

        #region GET ALL CUSTOMERS
        public IEnumerable<Customer> GetAll()
        {
            return customerRepository.Set()
                   .ToList()
                   .Select(c => new Customer
                   {
                       CustomerID = c.CustomerID,
                       Country = c.Country,
                       ContactName = c.ContactName,
                       Orders = c.Orders,
                   }).ToList();
        }
        #endregion
        
        #region GET CUSTOMER BY ID
        public CustomerDto GetCustomerDtoByID(string customerId, ServicesController services)
        {

            //var customer = customerRepository.Set().ToList()
            var customer = services.customerServices.customerRepository.Set().ToList()
                .FirstOrDefault(c => c.CustomerID == customerId);

            if (customer == null)
            {
                NewLine();
                Console.WriteLine("No existe el Cliente!");
                return null;
            }

            var customerDto = new CustomerDto()
            {
                CustomerID = customer.CustomerID,
                ContactName = customer.ContactName,
                CompanyName = customer.CompanyName,
            };

            return customerDto;

        }
        #endregion

        #region GET REAL CUSTOMER BY ID (NO DTO)
        public Customer GetCustomerByID(string customerId, ServicesController services)
        {

            var customer = services.customerServices.customerRepository.Set().ToList()
                .FirstOrDefault(c => c.CustomerID == customerId);

            if (customer == null)
            {
                NewLine();
                Console.WriteLine("No existe el Cliente!");
                return null;
            }
            //customer.Country = "Argentina";

            var customerByID = new Customer()
            {
                CustomerID = customer.CustomerID,
                ContactName = customer.ContactName,
                CompanyName = customer.CompanyName,
                Country = customer.Country,
            };

            return customerByID;

        }
        #endregion


        #region Update / UPDATE CUSTOMER
        //public void UpdateCustomer(string customerId)
        public void Update(CustomerDto customerDto)
        {

            var customer = customerRepository.Set()
            .FirstOrDefault(x => x.CustomerID == customerDto.CustomerID);

            if (customer == null)
                throw new Exception("El Cliente no existe");

            Console.WriteLine($"Categoria : {customerDto.CustomerID} ha sido encontrado.");
            Console.WriteLine($"Su Nombre de Contacto es : {customerDto.ContactName}.");

            //CustomerID
            NewLine();
            Console.WriteLine("Ingrese el nuevo ID del Cliente:");
            customerDto.CustomerID = Console.ReadLine();

            //ContactName
            NewLine();
            Console.WriteLine("Ingrese el nuevo Nombre de Contacto:");
            customerDto.ContactName = Console.ReadLine();

            //ContactName
            NewLine();
            Console.WriteLine("Ingrese el nuevo Nombre de Compañia:");
            customerDto.CompanyName = Console.ReadLine();

            //...

            customer.Address = customerDto.Address;
            customer.City = customerDto.City;
            customer.CompanyName = customerDto.CompanyName;
            customer.ContactName = customerDto.ContactName;
            customer.ContactTitle = customerDto.ContactTitle;
            customer.Country = customerDto.Country;
            customer.Region = customerDto.Region;
            customer.PostalCode = customerDto.PostalCode;
            customer.Phone = customerDto.Phone;
            customer.Fax = customerDto.Fax;

            customerRepository.Update(customer);
            customerRepository.SaveChanges();
        }
        #endregion



        #region CREATE CUSTOMER

        public void Create(CustomerDto dto)
        {
            customerRepository.Persist(new Customer
            {
                Address = dto.Address,
                City = dto.City,
                CompanyName = dto.CompanyName,
                ContactName = dto.ContactName,
                ContactTitle = dto.ContactTitle,
                Country = dto.Country,
                CustomerID = dto.CustomerID,
                Region = dto.Region,
                PostalCode = dto.PostalCode,
                Phone = dto.Phone,
                Fax = dto.Fax,
            });

            customerRepository.SaveChanges();
        }
        #endregion



        #region REMOVE CUSTOMER

        public void Remove(CustomerDto dto)
        {
            var customer = customerRepository.Set()
                .FirstOrDefault(x => x.CustomerID == dto.CustomerID);

            if (customer == null)
                throw new Exception("El Cliente no existe");

            customerRepository.Remove(customer);
            customerRepository.SaveChanges();
        }
        #endregion


        #region BEST CUSTOMER BY COUNTRY

        public List<BestCustomerDto> GetBestCostumer(ServicesController services)
        {
            var customers = services.customerServices.GetAll();

            return customers //services.customerServices.GetAll()
                .GroupBy(c => c.Country)
                .Select(k => new BestCustomerDto
                {
                    Country = k.Key,

                    Name = k
                        .OrderByDescending(c => c.Orders//services.orderServices.GetAllByIDOfCustomer(c.CustomerID,services)//c.Orders
                            .Sum(o => o.Order_Details
                            .Sum(d => d.Quantity * d.Product.UnitPrice)))
                        .Select(c => c.ContactName)
                        .FirstOrDefault(),

                    TotalPurchased = k.Select(v => v.Orders
                        .Where(c => c.CustomerID == k
                        .OrderByDescending(b => b.Orders
                            .Sum(o => o.Order_Details
                            .Sum(d => d.Quantity * d.Product.UnitPrice)))
                        .Select(b => b.CustomerID)
                        .FirstOrDefault())
                        .Sum(g => g.Order_Details
                            .Sum(d => d.Quantity * d.Product.UnitPrice)))
                        .Sum()
                }).ToList();
        }
        #endregion

        public void NewLine()
        {
            Console.WriteLine("");
        }

        public void UPDATE(ServicesController services)
        {
            var coso = services.customerServices.GetCustomerByID("ANATR", services);

            coso.Country = "Argentina";

            /*
            var coso2 = new Employee
            {
                FirstName = coso.FirstName,
                LastName = coso.LastName,
                Address = coso.Address,
                City = coso.City,
                Country = coso.Country,

            };
            */
            customerRepository.Update(coso);
            customerRepository.SaveChanges();
        }
    }
}
