namespace API_PCC.ApplicationModels
{
    public class BuffHerdSearchFilterModel
    {
        public string? searchValue { get; set; }
        public FilterByModel? filterBy { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public SortByModel sortBy { get; set; }
        
    }
}
