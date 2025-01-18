using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourkit.Interface;
using Tourkit.Models;
using Tourkit.Services;

namespace Tourkit.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public DataTableData<string> GetProducts(int pageIndex, int pageSize, string? keySearch, int? category)
        {
            var result = _productService.getProducts(pageIndex, pageSize, keySearch, category ?? 0);
            return result;
        }

        
        [HttpPost]
        [Route("addNew")]
        public IActionResult AddProduct(string name,decimal price, int categoryId, DateTime date)
        {
            if ( string.IsNullOrEmpty(name))
            {
                return BadRequest("Product name is required.");
            }

            var productExists = _productService.CheckProductNameExists(name);

            if (productExists)
            {
                return Conflict("Tên sản phẩm đã tồn tại. Vui lòng nhập tên khác!");
            }

            var result = _productService.AddProduct( name,  price,  categoryId, date);

            return StatusCode(201, "Success!");
        }


        [HttpPut]
        [Route("update")]
        public IActionResult UpdateProduct(int id,string name, decimal price, int categoryId, DateTime date)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Product name is required.");
            }


            var result = _productService.UpdatedProduct(id, name, price, categoryId, date);
            if(result == true)
            {
                return StatusCode(201, "Success!");
            }
            else
            {
                return StatusCode(409, "Tên sản phẩm đã tồn tại. Vui lòng nhập tên khác!");
            }
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteProduct(int id)
        {
            var result = _productService.DeleteProduct(id);
            if (result == true)
            {
                return StatusCode(201, "Success!");
            }
            else
            {
                return StatusCode(404, "Not Found.");
            }
        }
    }
}
