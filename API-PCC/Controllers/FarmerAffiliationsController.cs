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
using System.Data.SqlClient;


namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class FarmerAffiliationsController : ControllerBase
    {
        private readonly PCC_DEVContext _context;
        DbManager db = new DbManager();

        public FarmerAffiliationsController(PCC_DEVContext context)
        {
            _context = context;
        }

        // POST: FarmerAffiliations/list
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HFarmerAffiliation>>> list(FarmerAffiliationSearchFilterModel searchFilter)
        {
            sanitizeInput(searchFilter);
            try
            {
                DataTable queryResult = db.SelectDb_WithParamAndSorting(QueryBuilder.buildFarmerAffiliationSelectQuery(searchFilter), null, populateSqlParameters(searchFilter));
                var result = buildFarmerAffiliationPagedModel(searchFilter, queryResult);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.GetBaseException().ToString());
            }
        }

        private SqlParameter[] populateSqlParameters(FarmerAffiliationSearchFilterModel searchFilter)
        {

            var sqlParameters = new List<SqlParameter>();

            if (searchFilter.searchParam != null && searchFilter.searchParam != "")
            {
                sqlParameters.Add(new SqlParameter
                {
                    ParameterName = "SearchParam",
                    Value = searchFilter.searchParam ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                });
            }

            return sqlParameters.ToArray();
        }

        private void sanitizeInput(FarmerAffiliationSearchFilterModel searchFilter)
        {
            searchFilter.searchParam = StringSanitizer.sanitizeString(searchFilter.searchParam);
        }

        private List<FarmerAffiliationPagedModel> buildFarmerAffiliationPagedModel(FarmerAffiliationSearchFilterModel searchFilter, DataTable dt)
        {

            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;

            int totalItems = dt.Rows.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
            items = dt.AsEnumerable().Skip((page - 1) * pagesize).Take(pagesize).ToList();

            var farmerAffiliationModels = convertDataRowListToFarmerAffiliationlist(items);
            List<FarmerAffiliationResponseModel> famerAffiliationResponseModels = convertFarmerAffiliationToResponseModelList(farmerAffiliationModels);

            var result = new List<FarmerAffiliationPagedModel>();
            var item = new FarmerAffiliationPagedModel();

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
            item.items = famerAffiliationResponseModels;
            result.Add(item);

            return result;
        }

        private List<HFarmerAffiliation> convertDataRowListToFarmerAffiliationlist(List<DataRow> dataRowList)
        {
            var farmerAffiliationList = new List<HFarmerAffiliation>();

            foreach (DataRow dataRow in dataRowList)
            {
                var farmerAffiliationModel = DataRowToObject.ToObject<HFarmerAffiliation>(dataRow);
                farmerAffiliationList.Add(farmerAffiliationModel);
            }

            return farmerAffiliationList;
        }


        private List<FarmerAffiliationResponseModel> convertFarmerAffiliationToResponseModelList(List<HFarmerAffiliation> farmerAffiliationList)
        {
            var farmerAffiliationResponseModels = new List<FarmerAffiliationResponseModel>(); 

            foreach (HFarmerAffiliation farmerAffiliation in farmerAffiliationList)
            {
                var farmerAffiliationResponseModel = new FarmerAffiliationResponseModel()
                {
                    farmerAffiliationCode = farmerAffiliation.FCode,
                    farmerAffiliationName = farmerAffiliation.FDesc
                };
                farmerAffiliationResponseModels.Add(farmerAffiliationResponseModel);
            }

            return farmerAffiliationResponseModels;
        }

        // GET: FarmerAffiliations/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HFarmerAffiliation>> search(int id)
        {
            if (_context.HFarmerAffiliations == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HFarmerAffiliations' is null!");
            }
            var farmerAffiliation = await _context.HFarmerAffiliations.FindAsync(id);

            if (farmerAffiliation == null || farmerAffiliation.DeleteFlag)
            {
                return Conflict("No records found!");
            }

            return farmerAffiliation;
        }

        // PUT: FarmerAffiliations/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, HFarmerAffiliation hFarmerAffiliation)
        {
            if (_context.HFarmerAffiliations == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HFarmerAffiliations' is null!");
            }

            var farmerAffiliation = _context.HFarmerAffiliations.AsNoTracking().Where(farmerAffiliation => !farmerAffiliation.DeleteFlag && farmerAffiliation.Id == id).FirstOrDefault();

            if (farmerAffiliation == null)
            {
                return Conflict("No records matched!");
            }

            if (id != hFarmerAffiliation.Id)
            {
                return Conflict("Ids mismatched!");
            }

            bool hasDuplicateOnUpdate = (_context.HFarmerAffiliations?.Any(farmerAffiliation => !farmerAffiliation.DeleteFlag && farmerAffiliation.FCode == hFarmerAffiliation.FCode && farmerAffiliation.FDesc == hFarmerAffiliation.FDesc && farmerAffiliation.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.Entry(hFarmerAffiliation).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // POST: FarmerAffiliations/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HFarmerAffiliation>> save(HFarmerAffiliation hFarmerAffiliation)
        {
            if (_context.HFarmerAffiliations == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HFarmerAffiliations' is null!");
            }

            bool hasDuplicateOnSave = (_context.HFarmerAffiliations?.Any(farmerAffiliation => !farmerAffiliation.DeleteFlag && farmerAffiliation.FCode == hFarmerAffiliation.FCode && farmerAffiliation.FDesc == hFarmerAffiliation.FDesc)).GetValueOrDefault();

            if (hasDuplicateOnSave)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.HFarmerAffiliations.Add(hFarmerAffiliation);
                await _context.SaveChangesAsync();

                return CreatedAtAction("save", new { id = hFarmerAffiliation.Id }, hFarmerAffiliation);
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // POST: FarmerAffiliations/delete/5
        [HttpPost]
        public async Task<IActionResult> delete(DeletionModel deletionModel)
        {
            if (_context.HFarmerAffiliations == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HFarmerAffiliations' is null!");
            }

            var farmerAffiliation = await _context.HFarmerAffiliations.FindAsync(deletionModel.id);
            if (farmerAffiliation == null || farmerAffiliation.DeleteFlag)
            {
                return Conflict("No records matched!");
            }

            try
            {
                farmerAffiliation.DeleteFlag = true;
                farmerAffiliation.DateDeleted = DateTime.Now;
                farmerAffiliation.DeletedBy = deletionModel.deletedBy;
                farmerAffiliation.DateRestored = null;
                farmerAffiliation.RestoredBy = "";
                _context.Entry(farmerAffiliation).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Deletion Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // GET: farmerAffiliations/view
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HFarmerAffiliation>>> view()
        {
            if (_context.HFarmerAffiliations == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HFarmerAffiliations' is null.");
            }
            return await _context.HFarmerAffiliations.Where(farmerAffiliation => !farmerAffiliation.DeleteFlag).ToListAsync();
        }

        // POST: farmerAffiliations/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {

            if (_context.HFarmerAffiliations == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HFarmerAffiliations' is null!");
            }

            var farmerAffiliation = await _context.HFarmerAffiliations.FindAsync(restorationModel.id);
            if (farmerAffiliation == null || !farmerAffiliation.DeleteFlag)
            {
                return Conflict("No deleted records matched!");
            }

            try
            {
                farmerAffiliation.DeleteFlag = !farmerAffiliation.DeleteFlag;
                farmerAffiliation.DateDeleted = null;
                farmerAffiliation.DeletedBy = "";
                farmerAffiliation.DateRestored = DateTime.Now;
                farmerAffiliation.RestoredBy = restorationModel.restoredBy;

                _context.Entry(farmerAffiliation).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        private bool HFarmerAffiliationExists(int id)
        {
            return (_context.HFarmerAffiliations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
