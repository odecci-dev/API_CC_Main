using API_PCC.Data;
using API_PCC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static API_PCC.Controllers.HerdClassificationController;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class HBuffHerdsController : ControllerBase
    {
        private readonly PCC_DEVContext _context;

        public HBuffHerdsController(PCC_DEVContext context)
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
            public List<HBuffHerd> items { get; set; }
        }

        public class BuffHerdSearchFilter
        {
            public string? herdCode { get; set; }
            public string? herdName { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
        }

        // POST: BuffHerds/list
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HBuffHerd>>> list(BuffHerdSearchFilter searchFilter)
        {
          if (_context.HBuffHerds == null)
          {
              return NotFound();
            }

            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;


            var hBuffHerdList = _context.HBuffHerds.AsNoTracking();
            hBuffHerdList = hBuffHerdList.Where(buffHerd => !buffHerd.DeleteFlag);
            try
            {
                if (searchFilter.herdCode != null && searchFilter.herdCode != "")
                {
                    hBuffHerdList = hBuffHerdList.Where(buffHerd => buffHerd.HerdCode.Contains(searchFilter.herdCode));
                }

                if (searchFilter.herdName != null && searchFilter.herdName != "")
                {
                    hBuffHerdList = hBuffHerdList.Where(buffHerd => buffHerd.HerdName.Contains(searchFilter.herdName));
                }

                totalItems = hBuffHerdList.ToList().Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                items = hBuffHerdList.Skip((page - 1) * pagesize).Take(pagesize).ToList();

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

        // GET: BuffHerds/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HBuffHerd>> search(int id)
        {
            if (_context.HBuffHerds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.BuffHerd' is null!");
            }
            var hBuffHerd = await _context.HBuffHerds.FindAsync(id);

            if (hBuffHerd == null || hBuffHerd.DeleteFlag)
            {
                return Conflict("No records found!");
            }

            return Ok(hBuffHerd);

        }

        // PUT: BuffHerds/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, HBuffHerd hBuffHerd)
        {
            if (id != hBuffHerd.Id)
            {
                return BadRequest();
            }

            var buffHerd = _context.HBuffHerds.AsNoTracking().Where(buffHerd => !buffHerd.DeleteFlag && buffHerd.Id == id).FirstOrDefault();

            if (buffHerd == null)
            {
                return Conflict("No records matched!");
            }

            if (id != hBuffHerd.Id)
            {
                return Conflict("Ids mismatched!");
            }

            bool hasDuplicateOnUpdate = (_context.HBuffHerds?.Any(buffHerd => !buffHerd.DeleteFlag && buffHerd.HerdName == hBuffHerd.HerdName && buffHerd.HerdCode == hBuffHerd.HerdCode && buffHerd.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.Entry(buffHerd).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // POST: BuffHerds/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HBuffHerd>> save(HBuffHerd hBuffHerd)
        {
          if (_context.HBuffHerds == null)
          {
              return Problem("Entity set 'PCC_DEVContext.HBuffHerds'  is null.");
            }
            bool hasDuplicateOnSave = (_context.HBuffHerds?.Any(buffHerd => !buffHerd.DeleteFlag && buffHerd.HerdCode == hBuffHerd.HerdCode && buffHerd.HerdName == hBuffHerd.HerdName)).GetValueOrDefault();


            if (hasDuplicateOnSave)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.HBuffHerds.Add(hBuffHerd);
                await _context.SaveChangesAsync();

                return CreatedAtAction("save", new { id = hBuffHerd.Id }, hBuffHerd);
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // DELETE: BuffHerds/delete/5
        [HttpPost]
        public async Task<IActionResult> delete(DeletionModel deletionModel)
        {
            if (_context.HBuffHerds == null)
            {
                return NotFound();
            }
            var hBuffHerd = await _context.HBuffHerds.FindAsync(deletionModel.id);
            if (hBuffHerd == null || hBuffHerd.DeleteFlag)
            {
                return Conflict("No records matched!");
            }

            try
            {
                hBuffHerd.DeleteFlag = true;
                hBuffHerd.DateDeleted = DateTime.Now;
                hBuffHerd.DeletedBy = deletionModel.deletedBy;
                hBuffHerd.DateRestored = null;
                hBuffHerd.RestoredBy = "";
                _context.Entry(hBuffHerd).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Deletion Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // POST: BuffHerds/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {

            if (_context.HBuffHerds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.BuffHerd' is null!");
            }

            var hBuffHerd = await _context.HBuffHerds.FindAsync(restorationModel.id);
            if (hBuffHerd == null || !hBuffHerd.DeleteFlag)
            {
                return Conflict("No deleted records matched!");
            }

            try
            {
                hBuffHerd.DeleteFlag = !hBuffHerd.DeleteFlag;
                hBuffHerd.DateDeleted = null;
                hBuffHerd.DeletedBy = "";
                hBuffHerd.DateRestored = DateTime.Now;
                hBuffHerd.RestoredBy = restorationModel.restoredBy;

                _context.Entry(hBuffHerd).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        private bool HBuffHerdExists(int id)
        {
            return (_context.HBuffHerds?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
