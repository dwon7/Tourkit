using Tourkit.Models;

namespace Tourkit.Interface
{
    public interface ICategoryService
    {
        DataTableData<string> getCategories(int pageIndex, int pageSize, string? keySearch);
        bool CheckCategoryNameExists(string categoryName);
        bool AddCategory(string name, DateTime date);
        bool UpdatedCategory(int id, string name, DateTime date);
        bool DeleteCategory(int id);
    }
}
