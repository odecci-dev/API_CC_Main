using API_PCC.ApplicationModels.Common;
using API_PCC.DtoModels;
using API_PCC.Models;

namespace API_PCC.ApplicationModels
{
    public class HerdPagedModel : PaginationModel
    {
        public List<BuffHerdBaseModel> items { get; set; }
    }
}
