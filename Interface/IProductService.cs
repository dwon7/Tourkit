using Tourkit.Models;

namespace Tourkit.Interface
{
    public interface IProductService
    {
        DataTableData<string> getProducts(int pageIndex, int pageSize, string? keySearch, int category);
        bool CheckProductNameExists(string productName);
        bool AddProduct(string name, decimal price, int categoryId, DateTime date);
        bool UpdatedProduct(int id, string name, decimal price, int categoryId, DateTime date);
        bool DeleteProduct(int id);
    }
}
