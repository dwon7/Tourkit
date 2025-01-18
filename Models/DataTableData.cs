namespace Tourkit.Models
{
    public class DataTableData<T>
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public T data { get; set; }
    }

}
