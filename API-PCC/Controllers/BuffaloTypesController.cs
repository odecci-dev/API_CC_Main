using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_PCC.Data;
using API_PCC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol.Core.Types;
using static API_PCC.Controllers.HerdClassificationController;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class BuffaloTypesController : ControllerBase
    {
        private readonly PCC_DEVContext _context;

        public BuffaloTypesController(PCC_DEVContext context)
        {
            _context = context;
        }

        public class PaginationModel
        {
            public string? CurrentPage { get; set; }
            public string? NextPage { get; set; }
            public string? PrevPage { get; set; }
            public string? TotalPage { get; set; }
            public string? PageSize { get; set; }
            public string? TotalRecord { get; set; }
            public List<HBuffaloType> items { get; set; }
        }
        public class BuffaloTypeSearchFilter
        {
            public string? bBuffCode { get; set; }
            public string? bBuffDesc { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
        }

        // POST: BuffaloTypes/list
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HBuffaloType>>> list(BuffaloTypeSearchFilter searchFilter)
        {
            if (_context.HBuffaloTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HBuffaloTypes' is null!");
            }

            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;


            var buffaloTypeList = _context.HBuffaloTypes.AsNoTracking();
            buffaloTypeList = buffaloTypeList.Where(buffaloType => !buffaloType.DeleteFlag);
            try
            {
                if (searchFilter.bBuffCode != null && searchFilter.bBuffCode != "")
                {
                    buffaloTypeList = buffaloTypeList.Where(buffaloType => buffaloType.BBuffCode.Contains(searchFilter.bBuffCode));
                }

                if (searchFilter.bBuffDesc != null && searchFilter.bBuffDesc != "")
                {
                    buffaloTypeList = buffaloTypeList.Where(buffaloType => buffaloType.BBuffDesc.Contains(searchFilter.bBuffDesc));
                }

                totalItems = buffaloTypeList.ToList().Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                items = buffaloTypeList.Skip((page - 1) * pagesize).Take(pagesize).ToList();

                var result = new List<PaginationModel>();
                var item = new PaginationModel();

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
                return Ok(items);
            }

            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // GET: BuffaloTypes/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HBuffaloType>> search(int id)
        {
            if (_context.HBuffaloTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HBuffaloTypes' is null!");
            }
            var hbuffaloType = await _context.HBuffaloTypes.FindAsync(id);

            if (hbuffaloType == null || hbuffaloType.DeleteFlag)
            {
                return Conflict("No records found!");
            }

            return hbuffaloType;
        }

        // PUT: BuffaloTypes/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, HBuffaloType hBuffaloType)
        {
            if (_context.HBuffaloTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.Feeding System' is null!");
            }

            var buffaloType = _context.HBuffaloTypes.AsNoTracking().Where(buffaloType => !buffaloType.DeleteFlag && buffaloType.Id == id).FirstOrDefault();

            if (buffaloType == null)
            {
                return Conflict("No records matched!");
            }

            if (id != hBuffaloType.Id)
            {
                return Conflict("Ids mismatched!");
            }

            bool hasDuplicateOnUpdate = (_context.HBuffaloTypes?.Any(buffaloType => !buffaloType.DeleteFlag && buffaloType.BBuffCode == hBuffaloType.BBuffCode && buffaloType.BBuffDesc == hBuffaloType.BBuffDesc && buffaloType.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.Entry(hBuffaloType).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // POST: BuffaloTypes/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HBuffaloType>> save(HBuffaloType hBuffaloType)
        {
            if (_context.HBuffaloTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HBuffaloTypes' is null!");
            }

            bool hasDuplicateOnSave = (_context.HBuffaloTypes?.Any(buffaloType => !buffaloType.DeleteFlag && buffaloType.BBuffCode == hBuffaloType.BBuffCode && buffaloType.BBuffDesc == hBuffaloType.BBuffDesc)).GetValueOrDefault();

            if (hasDuplicateOnSave)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.HBuffaloTypes.Add(hBuffaloType);
                await _context.SaveChangesAsync();

                return CreatedAtAction("save", new { id = hBuffaloType.Id }, hBuffaloType);
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // POST: buffaloTypes/delete/5
        [HttpPost]
        public async Task<IActionResult> delete(DeletionModel deletionModel)
        {
            if (_context.HBuffaloTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HBuffaloTypes' is null!");
            }
            var hbuffaloType = await _context.HBuffaloTypes.FindAsync(deletionModel.id);
            if (hbuffaloType == null || hbuffaloType.DeleteFlag)
            {
                return Conflict("No records matched!");
            }

            bool feedCodeExistsInBuffHerd = _context.HBuffHerds.Any(buffHerd => !buffHerd.DeleteFlag && buffHerd.BBuffCode == hbuffaloType.BBuffCode);

            if (feedCodeExistsInBuffHerd)
            {
                return Conflict("Used by other table!");
            }

            try
            {
                hbuffaloType.DeleteFlag = true;
                hbuffaloType.DateDeleted = DateTime.Now;
                hbuffaloType.DeletedBy = deletionModel.deletedBy;
                hbuffaloType.DateRestored = null;
                hbuffaloType.RestoredBy = "";
                _context.Entry(hbuffaloType).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Deletion Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }


        // GET: buffaloTypes/view
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HBuffaloType>>> view()
        {
            if (_context.HBuffaloTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HbuffaloType' is null.");
            }
            return await _context.HBuffaloTypes.Where(buffaloType => !buffaloType.DeleteFlag).ToListAsync();
        }

        // POST: buffaloTypes/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {

            if (_context.HBuffaloTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HbuffaloType' is null.");
            }

            var buffaloType = await _context.HBuffaloTypes.FindAsync(restorationModel.id);

            if (buffaloType == null || !buffaloType.DeleteFlag)
            {
                return Conflict("No deleted records matched!");
            }

            try
            {
                buffaloType.DeleteFlag = !buffaloType.DeleteFlag;
                buffaloType.DateDeleted = null;
                buffaloType.DeletedBy = "";
                buffaloType.DateRestored = DateTime.Now;
                buffaloType.RestoredBy = restorationModel.restoredBy;

                _context.Entry(buffaloType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        private bool HBuffaloTypeExists(int id)
        {
            return (_context.HBuffaloTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
