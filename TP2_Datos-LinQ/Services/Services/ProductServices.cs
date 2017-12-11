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

        #region GET ALL PRODUCTS
        public IEnumerable<ProductDto> GetAll()
        {
            return productRepository.Set()
                   .Select(p => new ProductDto
                   {
                       ProductID = p.ProductID,
                       ProductName = p.ProductName,
                       UnitPrice = p.UnitPrice,
                   }).ToList();
        }
        #endregion


        #region GET PRODUCT BY NAME
        public ProductDto GetByName(string name)
        {
            return productRepository.Set()
                   .Where(p => p.ProductName == name)
                   .Select(p => new ProductDto
                   {
                       ProductID = p.ProductID,
                       ProductName = p.ProductName,
                       UnitPrice = p.UnitPrice,
                   }).FirstOrDefault();
        }
        #endregion


        #region GET REAL PRODUCT BY ID (NO DTO)
        public Product GetProductByID(Nullable<int> productId)
        {
            var product = productRepository.Set().ToList()
                .FirstOrDefault(e => e.ProductID == productId);

            if (product == null)
            {
                NewLine();
                Console.WriteLine("No existe el producto!");
                return null;
            }
            /*
            //var productDto = new ProductDto()
            var productDto = new Product()
            {
                ProductID = product.ProductID,
                //ContactName = product.ContactName,
                //CompanyName = product.CompanyName,
            };
            */
            return product;

        }
        #endregion

        public List<BestSellerProductDto> GetBestSellProduct(ServicesController services)
        {

            var customers = services.customerServices.GetAll();

            return customers //services.customerServices.GetAll()
               .Where(c =>(c.CustomerID != null && c.Country != null))
               //.Where(c => (c.CustomerID.Substring(1) != "A"))
               .GroupBy(c => c.Country)
               .Select(k => new BestSellerProductDto
               {
                   Country = k.Key,
                   Name = k
                   .SelectMany(p => p.Orders)
                   .SelectMany(d => d.Order_Details)
                   .GroupBy(d => d.ProductID)
                   .OrderByDescending(d => d.Count())
                   .FirstOrDefault()
                   .Select(d => d.Product.ProductName)
                   .FirstOrDefault()
               }).ToList();
        }

        public void NewLine()
        {
            Console.WriteLine("");
        }
    }
}
