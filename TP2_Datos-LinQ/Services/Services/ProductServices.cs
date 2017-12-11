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

        #region ProductServices CLASS CONSTRUCTOR
        public ProductServices()
        {
            this.productRepository = new Repository<Product>();
        }
        #endregion


        #region GET ALL PRODUCTS
        public IEnumerable<ProductDto> GetAll()
        {
            try
            {
                return this.productRepository.Set()
                   .Select(p => new ProductDto
                   {
                       ProductID = p.ProductID,
                       ProductName = p.ProductName,
                       UnitPrice = p.UnitPrice,
                   }).ToList();
            }
            catch
            {
                NewLine();
                Console.WriteLine("Se produjo un ERROR al intentar obtener todos los Productos.");

                return null;
            }
            
        }
        #endregion


        #region GET PRODUCT BY NAME
        public ProductDto GetByName(string name)
        {
            try
            {
                return this.productRepository.Set()
                   .Where(p => p.ProductName == name)
                   .Select(p => new ProductDto
                   {
                       ProductID = p.ProductID,
                       ProductName = p.ProductName,
                       UnitPrice = p.UnitPrice,
                   }).FirstOrDefault();
            }
            catch
            {
                NewLine();
                Console.WriteLine($"Se produjo un ERROR al intentar obtener el Producto de nombre : '{name}'.");

                return null;
            }
        }
        #endregion


        #region GET REAL PRODUCT BY ID (NO DTO)
        public Product GetProductByID(Nullable<int> productId)
        {
            try
            {
                var product = this.productRepository.Set().ToList()
                .FirstOrDefault(e => e.ProductID == productId);

                if (product == null)
                {
                    NewLine();
                    Console.WriteLine("No existe el producto!");

                    return null;
                }

                return product;

            }
            catch
            {
                NewLine();
                Console.WriteLine($"Se produjo un ERROR al intentar obtener el Producto con ID : '{productId}'.");

                return null;
            }
        }
        #endregion


        #region GET BEST SELLER PRODUCT BY COUNTRY
        public List<BestSellerProductDto> GetBestSellProduct(ServicesController services)
        {
            try
            {
                var customers = services.customerServices.GetAll();

                var bestSellerProducts = customers
                   .Where(c => (c.CustomerID != null && c.Country != null))
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

                return bestSellerProducts;

            }
            catch
            {
                NewLine();
                Console.WriteLine($"Se produjo un ERROR al intentar obtener el Producto más vendido por País.");

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
