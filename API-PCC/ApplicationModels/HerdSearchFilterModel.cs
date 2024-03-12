namespace API_PCC.ApplicationModels
{
    public class BuffHerdSearchFilterModel
    {
        public string? herdCode { get; set; }
        public string? herdName { get; set; }
        public string? ownerName { get; set; }
        public string? farmManager { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
