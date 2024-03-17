using API_PCC.ApplicationModels;

namespace API_PCC.DtoModels
{
    public class BuffHerdRegistrationModel
    {
        public string HerdName { get; set; }
        public string HerdCode { get; set; }
        public int HerdSize { get; set; }
        public string BBuffCode { get; set; }
        public string FCode { get; set; }
        public string HTypeCode { get; set; }
        public string FeedCode { get; set; }
        public string FarmManager { get; set; }
        public string FarmAddress { get; set; }
        public Owner Owner { get; set; }
        public string OrganizationName { get; set; }
        public string Center { get; set; }
        public string CreatedBy { get; set; }

    }
}
