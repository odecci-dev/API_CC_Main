using API_PCC.Data;
using API_PCC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static API_PCC.Controllers.FeedingSystemsController;
using static API_PCC.Controllers.HerdClassificationController;
using PaginationModel = API_PCC.Controllers.FeedingSystemsController.PaginationModel;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class BuffAnimalsController : ControllerBase
    {
        private readonly PCC_DEVContext _context;

        public BuffAnimalsController(PCC_DEVContext context)
        {
            _context = context;
        }
        public class BuffAnimalSearchFilter
        {
            public string? AnimalId { get; set; }
            public string? Name { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
        }

        public class PaginationModel
        {
            public string? CurrentPage { get; set; }
            public string? NextPage { get; set; }
            public string? PrevPage { get; set; }
            public string? TotalPage { get; set; }
            public string? PageSize { get; set; }
            public string? TotalRecord { get; set; }
            public List<ABuffAnimal> items { get; set; }
        }

        // POST: BuffAnimals/list
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ABuffAnimal>>> list(BuffAnimalSearchFilter searchFilter)
        {
            if (_context.ABuffAnimals == null)
            {
                return Problem("Entity set 'PCC_DEVContext.BuffAnimal' is null!");
            }
            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;


            var buffAnimalList = _context.ABuffAnimals.AsNoTracking();
            buffAnimalList = buffAnimalList.Where(buffAnimal => !buffAnimal.DeleteFlag);
            try
            {
                if (searchFilter.AnimalId != null && searchFilter.AnimalId != "")
                {
                    buffAnimalList = buffAnimalList.Where(buffAnimal => buffAnimal.AnimalId.Contains(searchFilter.AnimalId));
                }

                if (searchFilter.Name != null && searchFilter.Name != "")
                {
                    buffAnimalList = buffAnimalList.Where(buffAnimal => buffAnimal.Name.Contains(searchFilter.Name));
                }

                totalItems = buffAnimalList.ToList().Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                items = buffAnimalList.Skip((page - 1) * pagesize).Take(pagesize).ToList();

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
                return Ok(result);
            }

            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // GET: BuffAnimals/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ABuffAnimal>> search(int id)
        {
            if (_context.ABuffAnimals == null)
            {
                return Problem("Entity set 'PCC_DEVContext.BuffAnimal' is null!");
            }
            var aBuffAnimal = await _context.ABuffAnimals.FindAsync(id);

            if (aBuffAnimal == null || aBuffAnimal.DeleteFlag)
            {
                return Conflict("No records found!");
            }
            return Ok(aBuffAnimal);
        }

        // PUT: BuffAnimals/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, ABuffAnimal aBuffAnimal)
        {
            if (id != aBuffAnimal.Id)
            {
                return BadRequest();
            }

            var buffAnimal = _context.ABuffAnimals.AsNoTracking().Where(buffAnimal => !buffAnimal.DeleteFlag && buffAnimal.Id == id).FirstOrDefault();

            if (buffAnimal == null)
            {
                return Conflict("No records matched!");
            }

            if (id != aBuffAnimal.Id)
            {
                return Conflict("Ids mismatched!");
            }

            bool hasDuplicateOnUpdate = (_context.ABuffAnimals?.Any(buffAnimal => !buffAnimal.DeleteFlag && buffAnimal.AnimalId == aBuffAnimal.AnimalId && buffAnimal.Name == aBuffAnimal.Name && buffAnimal.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.Entry(aBuffAnimal).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // POST: BuffAnimals/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ABuffAnimal>> save(ABuffAnimal aBuffAnimal)
        {
            if (_context.ABuffAnimals == null)
            {
                return Problem("Entity set 'PCC_DEVContext.ABuffAnimals'  is null.");
            }
            bool hasDuplicateOnSave = (_context.ABuffAnimals?.Any(buffAnimal => !buffAnimal.DeleteFlag && buffAnimal.AnimalId == aBuffAnimal.AnimalId && buffAnimal.Name == aBuffAnimal.Name)).GetValueOrDefault();


            if (hasDuplicateOnSave)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.ABuffAnimals.Add(aBuffAnimal);
                await _context.SaveChangesAsync();

                return CreatedAtAction("save", new { id = aBuffAnimal.Id }, aBuffAnimal);
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // POST: BuffAnimals/delete/5
        [HttpPost]
        public async Task<IActionResult> delete(DeletionModel deletionModel)
        {
            if (_context.ABuffAnimals == null)
            {
                return NotFound();
            }
            var aBuffAnimal = await _context.ABuffAnimals.FindAsync(deletionModel.id);
            if (aBuffAnimal == null || aBuffAnimal.DeleteFlag)
            {
                return Conflict("No records matched!");
            }

            try
            {
                aBuffAnimal.DeleteFlag = true;
                aBuffAnimal.DateDeleted = DateTime.Now;
                aBuffAnimal.DeletedBy = deletionModel.deletedBy;
                aBuffAnimal.DateRestored = null;
                aBuffAnimal.RestoredBy = "";
                _context.Entry(aBuffAnimal).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Deletion Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        // GET: FeedingSystems/view
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ABuffAnimal>>> view()
        {
            if (_context.ABuffAnimals == null)
            {
                return Problem("Entity set 'PCC_DEVContext.ABuffAnimals' is null.");
            }
            return await _context.ABuffAnimals.Where(buffAnimal => !buffAnimal.DeleteFlag).ToListAsync();
        }


        // POST: BuffAnimals/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {

            if (_context.ABuffAnimals == null)
            {
                return Problem("Entity set 'PCC_DEVContext.BuffAnimal' is null!");
            }

            var aBuffAnimal = await _context.ABuffAnimals.FindAsync(restorationModel.id);
            if (aBuffAnimal == null || !aBuffAnimal.DeleteFlag)
            {
                return Conflict("No deleted records matched!");
            }

            try
            {
                aBuffAnimal.DeleteFlag = !aBuffAnimal.DeleteFlag;
                aBuffAnimal.DateDeleted = null;
                aBuffAnimal.DeletedBy = "";
                aBuffAnimal.DateRestored = DateTime.Now;
                aBuffAnimal.RestoredBy = restorationModel.restoredBy;

                _context.Entry(aBuffAnimal).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

            private bool ABuffAnimalExists(int id)
        {
            return (_context.ABuffAnimals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
