using System;
using System.Collections.Generic;

namespace API_PCC.Models
{
    public partial class TblCenterModel
    {
        public int Id { get; set; }
        public string? CenterCode { get; set; }
        public string? CenterDesc { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime DateUpdated { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string? RestoredBy { get; set; }
        public DateTime? DateRestored { get; set; }
        public bool DeleteFlag { get; set; }
        public int Status { get; set; }
    }
}
