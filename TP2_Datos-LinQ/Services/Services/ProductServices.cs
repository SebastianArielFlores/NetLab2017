using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Dtos;

namespace Services
{
    public class ProductServices
    {
        Repository<Product> productRepository;

        public ProductServices()
        {
            productRepository = new Repository<Product>();
        }

        #region GET REAL EMPLOYEE BY ID (NO DTO)
        //public ProductDto GetProductByID(Nullable<int> productId)
        public Product GetProductByID(Nullable<int> productId)
        {
            var product = productRepository.Set().ToList()
                .FirstOrDefault(e => e.ProductID == productId);

            if (product == null)
            {
                Console.WriteLine("No existe la orden!");
                return null;
            }

            //var productDto = new ProductDto()
            var productDto = new Product()
            {
                ProductID = product.ProductID,
                //ContactName = product.ContactName,
                //CompanyName = product.CompanyName,
            };

            return product;

        }
        #endregion
    }
}
