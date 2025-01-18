using Tourkit.Interface;
using Tourkit.Models;

namespace Tourkit.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlStored _sqlStored;
        private readonly ApplicationDbContext _context;
        public CategoryService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public DataTableData<string> getCategories(int pageIndex, int pageSize, string? keySearch)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                int totalCount = 0;


                if (!string.IsNullOrEmpty(connectionString))
                {

                    string strCategory = SqlStored.GetCategories(connectionString, pageIndex, pageSize, out totalCount, keySearch);

                    return new DataTableData<string>
                    {
                        recordsTotal = totalCount,
                        data = strCategory,
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

        public bool CheckCategoryNameExists(string categoryName)
        {
            return _context.Categories.Any(c => c.Name.ToUpper() == categoryName.ToUpper());
        }

        public bool AddCategory(string name, DateTime date)
        {
            try
            {
                Category category = new Category();
                category.Name = name;
                category.Created = date;
                _context.Categories.Add(category);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool UpdatedCategory(int id, string name, DateTime date)
        {
            try
            {
                var existingCategory = _context.Categories.FirstOrDefault(p => p.Id == id);
                if (existingCategory.Name != name)
                {
                    if (CheckCategoryNameExists(name))
                    {
                        return false;
                    }
                }
                existingCategory.Name = name;
                existingCategory.Created = date;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(p => p.Id == id);
            if (category == null)
            {
                return false;
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }
    }
}
