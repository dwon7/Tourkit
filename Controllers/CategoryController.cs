using Microsoft.AspNetCore.Mvc;
using Tourkit.Interface;
using Tourkit.Models;

namespace Tourkit.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public DataTableData<string> GetCategories(int pageIndex, int pageSize, string? keySearch)
        {
            var result = _categoryService.getCategories(pageIndex, pageSize, keySearch);
            return result;
        }


        [HttpPost]
        [Route("addNew")]
        public IActionResult AddCategory(string name, DateTime date)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Category name is required.");
            }

            var categoryExists = _categoryService.CheckCategoryNameExists(name);

            if (categoryExists)
            {
                return Conflict("Tên loại sản phẩm đã tồn tại. Vui lòng nhập tên khác!");
            }

            var result = _categoryService.AddCategory(name, date);

            return StatusCode(201, "Success!");
        }


        [HttpPut]
        [Route("update")]
        public IActionResult UpdateCategory(int id, string name, DateTime date)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Category name is required.");
            }


            var result = _categoryService.UpdatedCategory(id, name, date);
            if (result == true)
            {
                return StatusCode(201, "Success!");
            }
            else
            {
                return StatusCode(409, "Tên loại sản phẩm đã tồn tại. Vui lòng nhập tên khác!");
            }
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteCategory(int id)
        {
            var result = _categoryService.DeleteCategory(id);
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
