using API_PCC.ApplicationModels.Common;
using API_PCC.Models;

namespace API_PCC.ApplicationModels
{
    public class HerdPagedModel : PaginationModel
    {
        public List<HBuffHerd> items { get; set; }
    }
}
