using System;
using System.Collections.Generic;

namespace API_PCC.Models
{
    public partial class UserTbl
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Fname { get; set; } = null!;
        public string Mname { get; set; } = null!;
        public string Lname { get; set; } = null!;
        public string Cno { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Status { get; set; }
        public string? RememberToken { get; set; }
        public string? CenterId { get; set; }
        public string? ApprovedBy { get; set; }
        public string? DateApproved { get; set; }
    }
}
