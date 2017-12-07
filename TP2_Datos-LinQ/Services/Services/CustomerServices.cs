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

        public CustomerServices()
        {
            customerRepository = new Repository<Customer>();
        }



        #region GET ALL CUSTOMERS
        public IEnumerable<CustomerDto> GetAll()
        {
            return customerRepository.Set()
                   .ToList()
                   .Select(c => new CustomerDto
                   {
                       //Address = c.Address,
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
        public Customer GetCustomerByID(string customerId,ServicesController services)
        {

            var customer = services.customerServices.customerRepository.Set().ToList()
                .FirstOrDefault(c => c.CustomerID == customerId);

            if (customer == null)
            {
                Console.WriteLine("No existe el Cliente!");
                return null;
            }

            var customerByID = new Customer()
            {
                CustomerID = customer.CustomerID,
                ContactName = customer.ContactName,
                CompanyName = customer.CompanyName,
            };

            return customerByID;

        }
        #endregion


        #region MODIFY / UPDATE CUSTOMER
        //public void ModifyCustomer(string customerId)
        public void Modify(CustomerDto customerDto)
        {

            var customer = customerRepository.Set()
            .FirstOrDefault(x => x.CustomerID == customerDto.CustomerID);

            if (customer == null)
                throw new Exception("El Cliente no existe");

            Console.WriteLine($"Categoria : {customerDto.CustomerID} ha sido encontrado.");
            Console.WriteLine($"Su Nombre de Contacto es : {customerDto.ContactName}.");

            //CustomerID
            Console.WriteLine("");
            Console.WriteLine("Ingrese el nuevo ID del Cliente:");
            customerDto.CustomerID = Console.ReadLine();

            //ContactName
            Console.WriteLine("");
            Console.WriteLine("Ingrese el nuevo Nombre de Contacto:");
            customerDto.ContactName = Console.ReadLine();

            //ContactName
            Console.WriteLine("");
            Console.WriteLine("Ingrese el nuevo Nombre de Compañia:");
            customerDto.CompanyName = Console.ReadLine();


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
    }
}
