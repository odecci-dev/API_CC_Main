using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_PCC.Data;
using API_PCC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using static API_PCC.Controllers.FeedingSystemsController;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class HerdTypesController : ControllerBase
    {
        public class PaginationModel
        {
            public string? CurrentPage { get; set; }
            public string? NextPage { get; set; }
            public string? PrevPage { get; set; }
            public string? TotalPage { get; set; }
            public string? PageSize { get; set; }
            public string? TotalRecord { get; set; }
            public List<HHerdType> items { get; set; }
        }

        public class HerdTypesSearchFilter
        {
            public string? typeCode { get; set; }
            public string? typeDesc { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
        }

        public class RestorationModel
        {
            public int id { get; set; }
            public string? restoredBy { get; set; }
        }

        private readonly PCC_DEVContext _context;

        public HerdTypesController(PCC_DEVContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> List(HerdTypesSearchFilter searchFilter)
        {
            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;

            var herdTypesList = _context.HHerdTypes.AsNoTracking();
            try
            {
                if (searchFilter.typeCode != null && searchFilter.typeCode != "")
                {
                    herdTypesList = herdTypesList.Where(feedingSystem => feedingSystem.HTypeCode.Contains(searchFilter.typeCode));
                }

                if (searchFilter.typeDesc != null && searchFilter.typeDesc != "")
                {
                    herdTypesList = herdTypesList.Where(feedingSystem => feedingSystem.HTypeDesc.Contains(searchFilter.typeDesc));

                }

                totalItems = herdTypesList.ToList().Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                items = herdTypesList.Skip((page - 1) * pagesize).Take(pagesize).ToList();

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

            catch (Exception)
            {
                return BadRequest("ERROR");
            }
        }

        // GET: HerdTypes/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HHerdType>> search(int id)
        {
          if (_context.HHerdTypes == null)
          {
              return NotFound();
          }
            var hHerdType = await _context.HHerdTypes.FindAsync(id);

            if (hHerdType == null)
            {
                return NotFound();
            }

            return hHerdType;
        }

        // PUT: HerdTypes/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, HHerdType hHerdType)
        {
            if (id != hHerdType.Id)
            {
                return BadRequest();
            }

            bool hasDuplicateOnUpdate = (_context.HHerdTypes?.Any(ht => ht.HTypeDesc == hHerdType.HTypeDesc && ht.HTypeCode == hHerdType.HTypeCode && ht.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            _context.Entry(hHerdType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HHerdTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: HerdTypes/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HHerdType>> save(HHerdType hHerdType)
        {
          if (_context.HHerdTypes == null)
          {
              return Problem("Entity set 'PCC_DEVContext.HHerdTypes'  is null.");
          }

          bool hasDuplicateOnSave = (_context.HHerdTypes?.Any(ht => ht.HTypeDesc == hHerdType.HTypeDesc && ht.HTypeCode == hHerdType.HTypeCode)).GetValueOrDefault();

            // check for duplication
          if (hasDuplicateOnSave)
          {
              return Conflict("Entity already exists");
          }

          _context.HHerdTypes.Add(hHerdType);
          await _context.SaveChangesAsync();

           return CreatedAtAction("save", new { id = hHerdType.Id }, hHerdType);
        }

        // DELETE: HerdTypes/delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            if (_context.HHerdTypes == null)
            {
                return NotFound();
            }
            var hHerdType = await _context.HHerdTypes.FindAsync(id);
            if (hHerdType == null)
            {
                return NotFound();
            }

            bool typeCodeExistsInBuffHerd = _context.HBuffHerds.Any(buffHerd => buffHerd.HTypeCode == hHerdType.HTypeCode);

            if (typeCodeExistsInBuffHerd)
            {
                return Conflict("Used by other table!");
            }

            _context.HHerdTypes.Remove(hHerdType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: FeedingSystems/view
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HHerdType>>> view()
        {
            if (_context.HHerdTypes == null)
            {
                return NotFound();
            }
            return await _context.HHerdTypes.ToListAsync();
        }

        // POST: HerdTypes/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {
            var hHerdType = await _context.HHerdTypes.FindAsync(restorationModel.id);

            if(hHerdType == null)
            {
                return NotFound();
            }


            hHerdType.DeleteFlag = !hHerdType.DeleteFlag;
            hHerdType.DateDelete = null;
            hHerdType.DeletedBy = "";
            hHerdType.DateRestored = DateTime.Now;
            hHerdType.RestoredBy = restorationModel.restoredBy;

            _context.Entry(hHerdType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool HHerdTypeExists(int id)
        {
            return (_context.HHerdTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }


    }
}
