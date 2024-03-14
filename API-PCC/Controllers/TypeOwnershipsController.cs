using API_PCC.ApplicationModels;
using API_PCC.ApplicationModels.Common;
using API_PCC.Data;
using API_PCC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class TypeOwnershipsController : ControllerBase
    {
        private readonly PCC_DEVContext _context;

        public TypeOwnershipsController(PCC_DEVContext context)
        {
            _context = context;
        }

        public class TypeOwnershipSearchFilter
        {
            public string? typeOwnCode { get; set; }
            public string? typeOwnDesc { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
        }

        // POST: TypeOwnerships/list
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ATypeOwnership>>> list(TypeOwnershipSearchFilter searchFilter)
        {
            if (_context.ATypeOwnerships == null)
            {
                return Problem("Entity set 'PCC_DEVContext.Feeding Sytem' is null!");
            }

            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;


            var typeOwnershipList = _context.ATypeOwnerships.AsNoTracking();
            typeOwnershipList = typeOwnershipList.Where(typeOwnership => !typeOwnership.DeleteFlag);
            try
            {
                if (searchFilter.typeOwnCode != null && searchFilter.typeOwnCode != "")
                {
                    typeOwnershipList = typeOwnershipList.Where(typeOwnership => typeOwnership.TypeOwnCode.Contains(searchFilter.typeOwnCode));
                }

                if (searchFilter.typeOwnDesc != null && searchFilter.typeOwnDesc != "")
                {
                    typeOwnershipList = typeOwnershipList.Where(typeOwnership => typeOwnership.TypeOwnDesc.Contains(searchFilter.typeOwnDesc));
                }

                totalItems = typeOwnershipList.ToList().Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                items = typeOwnershipList.Skip((page - 1) * pagesize).Take(pagesize).ToList();

                var result = new List<TypeOwnershipPagedModel>();
                var item = new TypeOwnershipPagedModel();

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

        // GET: TypeOwnerships/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ATypeOwnership>> search(int id)
        {
            if (_context.ATypeOwnerships == null)
            {
                return Problem("Entity set 'PCC_DEVContext.AtypeOwnerships' is null!");
            }
            var aTypeOwnership = await _context.ATypeOwnerships.FindAsync(id);

            if (aTypeOwnership == null)
            {
                return Conflict("No records matched!");
            }

            return Ok(aTypeOwnership);
        }

        // PUT: TypeOwnerships/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, ATypeOwnership aTypeOwnership)
        {
            
            if (id != aTypeOwnership.Id)
            {
                return Problem("Entity set 'PCC_DEVContext.AtypeOwnerships' is null!");
            }

            var typeOwnership = _context.ATypeOwnerships.AsNoTracking().Where(typeOwnership => !typeOwnership.DeleteFlag && typeOwnership.Id == id).FirstOrDefault();

            if (typeOwnership == null)
            {
                return Conflict("No records matched!");
            }

            if (id != aTypeOwnership.Id)
            {
                return Conflict("Ids mismatched!");
            }

            bool hasDuplicateOnUpdate = (_context.ATypeOwnerships?.Any(typeOwnership => !typeOwnership.DeleteFlag && typeOwnership.TypeOwnCode == aTypeOwnership.TypeOwnCode && typeOwnership.TypeOwnDesc == aTypeOwnership.TypeOwnDesc && typeOwnership.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.Entry(aTypeOwnership).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }
        

        // POST: TypeOwnerships/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ATypeOwnership>> save(ATypeOwnership aTypeOwnership)
        {
            if (_context.ATypeOwnerships == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HtypeOwnerships' is null!");
            }

            bool hasDuplicateOnSave = (_context.ATypeOwnerships?.Any(typeOwnership => !typeOwnership.DeleteFlag && typeOwnership.TypeOwnCode == aTypeOwnership.TypeOwnCode && typeOwnership.TypeOwnDesc == aTypeOwnership.TypeOwnDesc)).GetValueOrDefault();

            if (hasDuplicateOnSave)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.ATypeOwnerships.Add(aTypeOwnership);
                await _context.SaveChangesAsync();

                return CreatedAtAction("save", new { id = aTypeOwnership.Id }, aTypeOwnership);
            }
            catch (Exception ex)
            {
             
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // POST: TypeOwnerships/delete/5
        [HttpPost]
        public async Task<IActionResult> delete(DeletionModel deletionModel)
        {
            if (_context.ATypeOwnerships == null)
            {
                return Problem("Entity set 'PCC_DEVContext.AtypeOwnerships' is null!");
            }

            var typeOwnership = await _context.ATypeOwnerships.FindAsync(deletionModel.id);
            if (typeOwnership == null || typeOwnership.DeleteFlag)
            {
                return Conflict("No records matched!");
            }

            try
            {
                typeOwnership.DeleteFlag = true;
                typeOwnership.DateDeleted = DateTime.Now;
                typeOwnership.DeletedBy = deletionModel.deletedBy;
                typeOwnership.DateRestored = null;
                typeOwnership.RestoredBy = "";
                _context.Entry(typeOwnership).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Deletion Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }


        // GET: typeOwnerships/view
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ATypeOwnership>>> view()
        {
            if (_context.ATypeOwnerships == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HtypeOwnerships' is null.");
            }
            return await _context.ATypeOwnerships.Where(typeOwnership => !typeOwnership.DeleteFlag).ToListAsync();
        }

        // POST: typeOwnerships/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {

            if (_context.ATypeOwnerships == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HtypeOwnerships' is null!");
            }

            var typeOwnership = await _context.ATypeOwnerships.FindAsync(restorationModel.id);
            if (typeOwnership == null || !typeOwnership.DeleteFlag)
            {
                return Conflict("No deleted records matched!");
            }

            try
            {
                typeOwnership.DeleteFlag = !typeOwnership.DeleteFlag;
                typeOwnership.DateDeleted = null;
                typeOwnership.DeletedBy = "";
                typeOwnership.DateRestored = DateTime.Now;
                typeOwnership.RestoredBy = restorationModel.restoredBy;

                _context.Entry(typeOwnership).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }
      
        private bool ATypeOwnershipExists(int id)
        {
            return (_context.ATypeOwnerships?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
