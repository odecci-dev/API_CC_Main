
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_PCC.Data;
using API_PCC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using static API_PCC.Controllers.HerdClassificationController;
namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class BirthTypesController : ControllerBase
    {
        private readonly PCC_DEVContext _context;

        public class PaginationModel
        {
            public string? CurrentPage { get; set; }
            public string? NextPage { get; set; }
            public string? PrevPage { get; set; }
            public string? TotalPage { get; set; }
            public string? PageSize { get; set; }
            public string? TotalRecord { get; set; }
            public List<ABirthType> items { get; set; }
        }

        public class BirthTypesSearchFilter
        {
            public string? BirthTypeCode { get; set; }
            public string? BirthTypeDesc { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
        }

        public BirthTypesController(PCC_DEVContext context)
        {
            _context = context;
        }

        // POST: BirthTypes/list
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ABirthType>>> list(BirthTypesSearchFilter searchFilter)
        {
          if (_context.ABirthTypes == null)
          {
                return Problem("Entity set 'PCC_DEVContext.BirthTypes' is null!");
            }
            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;


            var birthTypesList = _context.ABirthTypes.AsNoTracking();
            birthTypesList = birthTypesList.Where(birthType => !birthType.DeleteFlag);
            try
            {
                if (searchFilter.BirthTypeCode != null && searchFilter.BirthTypeCode != "")
                {
                    birthTypesList = birthTypesList.Where(birthType => birthType.BirthTypeCode.Contains(searchFilter.BirthTypeCode));
                }

                if (searchFilter.BirthTypeDesc != null && searchFilter.BirthTypeDesc != "")
                {
                    birthTypesList = birthTypesList.Where(birthType => birthType.BirthTypeDesc.Contains(searchFilter.BirthTypeDesc));
                }

                totalItems = birthTypesList.ToList().Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                items = birthTypesList.Skip((page - 1) * pagesize).Take(pagesize).ToList();

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

        // GET: BirthTypes/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ABirthType>> search(int id)
        {
            if (_context.ABirthTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.BirthTypes' is null!");
            }
            var birthType = await _context.ABirthTypes.FindAsync(id);

            if (birthType == null || birthType.DeleteFlag)
            {
                return Conflict("No records found!");
            }
            return Ok(birthType);
        }

        // PUT: BirthTypes/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, ABirthType aBirthType)
        {
            if (id != aBirthType.Id)
            {
                return BadRequest();
            }

            var birthType = _context.ABirthTypes.AsNoTracking().Where(birthType => !birthType.DeleteFlag && birthType.Id == id).FirstOrDefault();

            if (birthType == null)
            {
                return Conflict("No records matched!");
            }

            if (id != birthType.Id)
            {
                return Conflict("Ids mismatched!");
            }

            bool hasDuplicateOnUpdate = (_context.ABirthTypes?.Any(birthType => !birthType.DeleteFlag && birthType.BirthTypeCode == aBirthType.BirthTypeCode && birthType.BirthTypeDesc == aBirthType.BirthTypeDesc && birthType.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.Entry(birthType).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // POST: BirthTypes/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ABirthType>> save(ABirthType aBirthType)
        {
            if (_context.ABirthTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.ABirthTypes'  is null.");
            }
            bool hasDuplicateOnSave = (_context.ABirthTypes?.Any(birthType => !birthType.DeleteFlag && birthType.BirthTypeCode == aBirthType.BirthTypeCode && birthType.BirthTypeDesc == aBirthType.BirthTypeDesc)).GetValueOrDefault();


            if (hasDuplicateOnSave)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.ABirthTypes.Add(aBirthType);
                await _context.SaveChangesAsync();

                return CreatedAtAction("save", new { id = aBirthType.Id }, aBirthType);
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // POST: BirthTypes/delete/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> delete(DeletionModel deletionModel)
        {

            if (_context.ABirthTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.BirthTypes' is null!");
            }

            var birthType = await _context.ABirthTypes.FindAsync(deletionModel.id);
            if (birthType == null || !birthType.DeleteFlag)
            {
                return Conflict("No deleted records matched!");
            }

            try
            {
                birthType.DeleteFlag = true;
                birthType.DateDeleted = DateTime.Now;
                birthType.DeletedBy = deletionModel.deletedBy;
                birthType.DateRestored = null;
                birthType.RestoredBy = "";
                _context.Entry(birthType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Deletion Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // POST: BirthTypes/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {

            if (_context.ABirthTypes == null)
            {
                return Problem("Entity set 'PCC_DEVContext.BirthTypes' is null!");
            }

            var birthType = await _context.ABirthTypes.FindAsync(restorationModel.id);
            if (birthType == null || !birthType.DeleteFlag)
            {
                return Conflict("No deleted records matched!");
            }

            try
            {
                birthType.DeleteFlag = !birthType.DeleteFlag;
                birthType.DateDeleted = null;
                birthType.DeletedBy = "";
                birthType.DateRestored = DateTime.Now;
                birthType.RestoredBy = restorationModel.restoredBy;

                _context.Entry(birthType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        private bool ABirthTypeExists(int id)
        {
            return (_context.ABirthTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
