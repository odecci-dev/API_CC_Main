using API_PCC.ApplicationModels.Common;
using API_PCC.Models;

namespace API_PCC.ApplicationModels
{
    public class FarmerAffiliationPagedModel : PaginationModel
    {
        public List<HFarmerAffiliation> items { get; set; }

    }
}
