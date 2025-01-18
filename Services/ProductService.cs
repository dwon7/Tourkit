using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourkit.Interface;
using Tourkit.Models;

namespace Tourkit.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlStored _sqlStored;
        private readonly ApplicationDbContext _context;
        public ProductService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public DataTableData<string> getProducts(int pageIndex, int pageSize, string? keySearch, int category)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                int totalCount = 0;


                if (!string.IsNullOrEmpty(connectionString))
                {

                    string strProduct = SqlStored.GetProducts(connectionString,pageIndex, pageSize, out totalCount, keySearch, category);

                    return new DataTableData<string>
                    {
                        recordsTotal = totalCount,
                        data = strProduct,
                        recordsFiltered = totalCount
                    };
                }
                else
                {
                    
                    return new DataTableData<string>
                    {
                        recordsTotal = 0,
                        data = string.Empty,
                        recordsFiltered = 0
                    };
                }
            }
            catch (Exception ex)
            {
                return new DataTableData<string>
                {
                    recordsTotal = 0,
                    data = string.Empty,
                    recordsFiltered = 0
                };
            }

        }

        public bool CheckProductNameExists(string productName)
        {
            return _context.Products.Any(p => p.Name.ToUpper() == productName.ToUpper());
        }

        public bool AddProduct(string name, decimal price, int categoryId, DateTime date)
        {
            try
            {
                Product product = new Product();
                product.Name = name;
                product.Price = price;
                product.CategoryId = categoryId;
                product.Created = date;
                _context.Products.Add(product);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool UpdatedProduct(int id, string name, decimal price, int categoryId, DateTime date)
        {
            try
            {
                var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);
                if (existingProduct.Name != name)
                {
                    if (CheckProductNameExists(name))
                    {
                        return false;
                    }
                }
                existingProduct.Name = name;
                existingProduct.Price = price;
                existingProduct.CategoryId = categoryId;
                existingProduct.Created = date;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }
    }
}