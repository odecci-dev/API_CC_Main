using API_PCC.ApplicationModels;
using API_PCC.ApplicationModels.Common;
using API_PCC.Data;
using API_PCC.Manager;
using API_PCC.Models;
using API_PCC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class BreedsController : ControllerBase
    {
        private readonly PCC_DEVContext _context;
        DbManager db = new DbManager();

        public BreedsController(PCC_DEVContext context)
        {
            _context = context;
        }

        // POST: Breeds/list
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ABreed>>> list(CommonSearchFilterModel searchFilter)
        {
            try
            {
                DataTable queryResult = db.SelectDb_WithParamAndSorting(QueryBuilder.buildBreedSearchQuery(searchFilter), null, populateSearchParamSqlParameters.populateSqlParameters(searchFilter));
                var result = buildHerdClassificationPagedModel(searchFilter, queryResult);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.GetBaseException().ToString());
            }
        }

        private void sanitizeInput(CommonSearchFilterModel searchFilter)
        {
            searchFilter.searchParam = StringSanitizer.sanitizeString(searchFilter.searchParam);
        }

        private List<BreedsPagedModel> buildHerdClassificationPagedModel(CommonSearchFilterModel searchFilter, DataTable dt)
        {
            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;

            int totalItems = dt.Rows.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
            items = dt.AsEnumerable().Skip((page - 1) * pagesize).Take(pagesize).ToList();


            var breedModels = convertDataRowListToBreedList(items);
            List<BreedResponseModel> breedResponseModels = convertBreedListToResponseModelList(breedModels);

            var result = new List<BreedsPagedModel>();
            var item = new BreedsPagedModel();

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
            item.items = breedResponseModels;
            result.Add(item);

            return result;
        }

        private List<ABreed> convertDataRowListToBreedList(List<DataRow> dataRowList)
        {
            var herdClassificationList = new List<ABreed>();

            foreach (DataRow dataRow in dataRowList)
            {
                var herdClassificationModel = DataRowToObject.ToObject<ABreed>(dataRow);
                herdClassificationList.Add(herdClassificationModel);
            }

            return herdClassificationList;
        }

        private List<BreedResponseModel> convertBreedListToResponseModelList(List<ABreed> breedList)
        {
            var breedResponseModels = new List<BreedResponseModel>();

            foreach (ABreed breed in breedList)
            {
                var breedResponseModel = new BreedResponseModel()
                {
                    breedCode = breed.BreedCode,
                    breedDesc = breed.BreedDesc
                };
                breedResponseModels.Add(breedResponseModel);
            }
            return breedResponseModels;
        }

        // GET: Breeds/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ABreed>> search(int id)
        {
            if (_context.ABreeds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.Abreeds' is null!");
            }
            var aBreed = await _context.ABreeds.FindAsync(id);

            if (aBreed == null || aBreed.DeleteFlag)
            {
                return Conflict("No records found!");
            }
            return Ok(aBreed);
        }

        // PUT: Breeds/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, ABreed aBreed)
        {
            if (id != aBreed.Id)
            {
                return BadRequest();
            }

            var breed = _context.ABreeds.AsNoTracking().Where(breed => !breed.DeleteFlag && breed.Id == id).FirstOrDefault();

            if (breed == null)
            {
                return Conflict("No records matched!");
            }

            if (id != aBreed.Id)
            {
                return Conflict("Ids mismatched!");
            }

            bool hasDuplicateOnUpdate = (_context.ABreeds?.Any(breed => !breed.DeleteFlag && breed.BreedCode == aBreed.BreedCode && breed.BreedDesc == aBreed.BreedDesc && breed.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.Entry(aBreed).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // POST: Breeds/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ABreed>> save(ABreed aBreed)
        {
            if (_context.ABreeds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.ABreed'  is null.");
            }
            bool hasDuplicateOnSave = (_context.ABreeds?.Any(breed => !breed.DeleteFlag && breed.BreedCode == aBreed.BreedCode && breed.BreedDesc == aBreed.BreedDesc)).GetValueOrDefault();

            if (hasDuplicateOnSave)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.ABreeds.Add(aBreed);
                await _context.SaveChangesAsync();

                return CreatedAtAction("save", new { id = aBreed.Id }, aBreed);
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // POST: Breeds/delete/5
        [HttpPost]
        public async Task<IActionResult> delete(DeletionModel deletionModel)
        {
            if (_context.ABreeds == null)
            {
                return NotFound();
            }
            var aBreed = await _context.ABreeds.FindAsync(deletionModel.id);
            if (aBreed == null || aBreed.DeleteFlag)
            {
                return Conflict("No records matched!");
            }

            try
            {
                aBreed.DeleteFlag = true;
                aBreed.DateDeleted = DateTime.Now;
                aBreed.DeletedBy = deletionModel.deletedBy;
                aBreed.DateRestored = null;
                aBreed.RestoredBy = "";
                _context.Entry(aBreed).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Deletion Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // GET: Breeds/view
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ABreed>>> view()
        {
            if (_context.ABreeds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.ABreeds' is null.");
            }
            return await _context.ABreeds.Where(breed => !breed.DeleteFlag).ToListAsync();
        }

        // POST: Breeds/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {

            if (_context.ABreeds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.Abreeds' is null!");
            }

            var aBreed = await _context.ABreeds.FindAsync(restorationModel.id);
            if (aBreed == null || !aBreed.DeleteFlag)
            {
                return Conflict("No deleted records matched!");
            }

            try
            {
                aBreed.DeleteFlag = !aBreed.DeleteFlag;
                aBreed.DateDeleted = null;
                aBreed.DeletedBy = "";
                aBreed.DateRestored = DateTime.Now;
                aBreed.RestoredBy = restorationModel.restoredBy;

                _context.Entry(aBreed).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        private bool ABreedExists(int id)
        {
            return (_context.ABreeds?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
