namespace Tourkit.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
