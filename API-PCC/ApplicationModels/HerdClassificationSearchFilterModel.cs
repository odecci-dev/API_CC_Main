namespace API_PCC.ApplicationModels
{
    public class HerdClassificationSearchFilterModel
    {
        public string? typeCode { get; set; }
        public string? typeDesc { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
