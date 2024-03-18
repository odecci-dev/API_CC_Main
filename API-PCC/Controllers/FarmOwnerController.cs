using API_PCC.ApplicationModels;
using API_PCC.Data;
using API_PCC.EntityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class FarmOwnerController : ControllerBase
    {
        private readonly PCC_DEVContext _context;

        public FarmOwnerController(PCC_DEVContext context)
        {
            _context = context;
        }

        // POST: farmOwners/list
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TblFarmOwner>>> list(FarmOwnerSearchFilterModel searchFilter)
        {
            if (_context.TblFarmOwners == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblFarmOwners' is null!");
            }

            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;


            var farmOwnerList = _context.TblFarmOwners.AsNoTracking();
            try
            {
                if (searchFilter.Name != null && searchFilter.Name != "")
                {
                    farmOwnerList = farmOwnerList.Where(farmOwner => farmOwner.Name.Contains(searchFilter.Name));
                }

                if (searchFilter.LastName != null && searchFilter.LastName != "")
                {
                    farmOwnerList = farmOwnerList.Where(farmOwner => farmOwner.LastName.Contains(searchFilter.LastName));
                }

                totalItems = farmOwnerList.ToList().Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                items = farmOwnerList.Skip((page - 1) * pagesize).Take(pagesize).ToList();

                var result = new List<FarmOwnerPagedModel>();
                var item = new FarmOwnerPagedModel();

                int pages = searchFilter.page == 0 ? 1 : searchFilter.page;
                item.CurrentPage = searchFilter.page == 0 ? "1" : searchFilter.page.ToString();
                int page_prev = pages - 1;

                double t_records = Math.Ceiling(Convert.ToDouble(totalItems) / Convert.ToDouble(pagesize));
                int page_next = searchFilter.page >= t_records ? 0 : pages + 1;
                item.NextPage = items.Count % pagesize >= 0 ? page_next.ToString() : "0";
                item.PrevPage = pages == 1 ? "0" : page_prev.ToString();
                item.TotalPage = t_records.ToString();
                item.PageSize = pagesize.ToString();
                item.TotalRecord = totalItems.ToString();
                item.items = items;
                result.Add(item);
                return Ok(result);
            }

            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
            }
        }

        // GET: farmOwners/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblFarmOwner>> search(int id)
        {
            if (_context.TblFarmOwners == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblFarmOwners' is null!");
            }
            var farmOwner = await _context.TblFarmOwners.FindAsync(id);

            if (farmOwner == null)
            {
                return Conflict("No records found!");
            }

            return farmOwner;
        }

        // PUT: farmOwners/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, TblFarmOwner TblFarmOwner)
        {
            if (_context.TblFarmOwners == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblFarmOwners' is null!");
            }

            var farmOwner = _context.TblFarmOwners.AsNoTracking().Where(farmOwner => farmOwner.Id == id).FirstOrDefault();

            if (farmOwner == null)
            {
                return Conflict("No records matched!");
            }

            if (id != TblFarmOwner.Id)
            {
                return Conflict("Ids mismatched!");
            }

            bool hasDuplicateOnUpdate = (_context.TblFarmOwners?.Any(farmOwner => farmOwner.Name == TblFarmOwner.Name && farmOwner.LastName == TblFarmOwner.LastName && farmOwner.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.Entry(TblFarmOwner).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
            }
        }

        // POST: farmOwners/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblFarmOwner>> save(TblFarmOwner TblFarmOwner)
        {
            if (_context.TblFarmOwners == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblFarmOwners' is null!");
            }

            bool hasDuplicateOnSave = (_context.TblFarmOwners?.Any(farmOwner => farmOwner.Name == TblFarmOwner.Name && farmOwner.LastName == TblFarmOwner.LastName)).GetValueOrDefault();

            if (hasDuplicateOnSave)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.TblFarmOwners.Add(TblFarmOwner);
                await _context.SaveChangesAsync();

                return CreatedAtAction("save", new { id = TblFarmOwner.Id }, TblFarmOwner);
            }
            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
            }
        }


    }
}
