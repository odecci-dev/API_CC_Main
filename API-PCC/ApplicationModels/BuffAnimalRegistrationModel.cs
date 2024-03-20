namespace API_PCC.ApplicationModels
{
    public class BuffAnimalRegistrationModel
    {
        public string AnimalId { get; set; }

        public string Name { get; set; }

        public string Rfid { get; set; }

        public string HerdCode { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Sex { get; set; }

        public string BuffaloType { get; set; }

        public string IdSystem { get; set; }

        public string PedigreeRecords { get; set; }

        public String Photo { get; set; }

        public string CountryBirth { get; set; }

        public string OriginAcquisition { get; set; }

        public DateTime DateAcquisition { get; set; }

        public string Marking { get; set; }

        // create Sire Object
        public Sire Sire { get; set; }

        // create Dam Object
        public Dam Dam { get; set; }   

        public string BreedCode { get; set; }

        public string BloodCode { get; set; }

        public string BirthTypeCode { get; set; }

        public string TypeOwnCode { get; set; }

        public string CreatedBy { get; set; }

    }
}
