﻿using API_PCC.ApplicationModels;
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
    public class HerdClassificationController : ControllerBase
    {

        private readonly PCC_DEVContext _context;
        DbManager db = new DbManager();

        public HerdClassificationController(PCC_DEVContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<HerdClassificationPagedModel>>> List(CommonSearchFilterModel searchFilter)
        {
            sanitizeInput(searchFilter);

            try
            {
                DataTable queryResult = db.SelectDb_WithParamAndSorting(QueryBuilder.buildHerdClassificationSearchQuery(searchFilter), null, populateSqlParameters(searchFilter));
                var result = buildHerdClassificationPagedModel(searchFilter, queryResult);
                return Ok(result);
            }

            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        private SqlParameter[] populateSqlParameters(CommonSearchFilterModel searchFilter)
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

        private void sanitizeInput(CommonSearchFilterModel searchFilter)
        {
            searchFilter.searchParam = StringSanitizer.sanitizeString(searchFilter.searchParam);
        }

        private List<HerdClassificationPagedModel> buildHerdClassificationPagedModel(CommonSearchFilterModel searchFilter, DataTable dt)
        {
            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;

            int totalItems = dt.Rows.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
            items = dt.AsEnumerable().Skip((page - 1) * pagesize).Take(pagesize).ToList();


            var herdClassificationModels = convertDataRowToHerdClassificationList(items);
            List<HerdClassificationResponseModel> herdClassificationResponseModels = convertHerdClassificationToResponseModelList(herdClassificationModels);

            var result = new List<HerdClassificationPagedModel>();
            var item = new HerdClassificationPagedModel();

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
            item.items = herdClassificationResponseModels;
            result.Add(item);

            return result;
        }

        private List<HHerdClassification> convertDataRowToHerdClassificationList(List<DataRow> dataRowList)
        {
            var herdClassificationList = new List<HHerdClassification>();

            foreach (DataRow dataRow in dataRowList)
            {
                var herdClassificationModel = DataRowToObject.ToObject<HHerdClassification>(dataRow);
                herdClassificationList.Add(herdClassificationModel);
            }

            return herdClassificationList;
        }

        private List<HerdClassificationResponseModel> convertHerdClassificationToResponseModelList(List<HHerdClassification> hHerdClassificationList)
        {
            var herdClassificationResponseModels = new List<HerdClassificationResponseModel>();

            foreach (HHerdClassification herdClassification in hHerdClassificationList)
            {
                var herdClassificationResponseModel = new HerdClassificationResponseModel()
                {
                    herdClassCode = herdClassification.HerdClassCode,
                    herdClassDesc = herdClassification.HerdClassDesc
                };
                herdClassificationResponseModels.Add(herdClassificationResponseModel);
            }

            return herdClassificationResponseModels;
        }


        // GET: HerdClassification/search/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HHerdClassification>> search(int id)
        {
            if (_context.HHerdClassifications == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HerdClassification' is null!");
            }
            var HHerdClassification = await _context.HHerdClassifications.FindAsync(id);

            if (HHerdClassification == null || HHerdClassification.DeleteFlag)
            {
                return Conflict("No records found!");
            }

            return HHerdClassification;
        }

        // PUT: HerdClassification/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, HHerdClassification HHerdClassification)
        {
            if (_context.HHerdClassifications == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HerdClassification' is null!");
            }

            var herdType = _context.HHerdClassifications.AsNoTracking().Where(herdType => !herdType.DeleteFlag && herdType.Id == id).FirstOrDefault();

            if (herdType == null)
            {
                return Conflict("No records matched!");
            }

            if (id != HHerdClassification.Id)
            {
                return Conflict("Ids mismatched!");
            }

            bool hasDuplicateOnUpdate = (_context.HHerdClassifications.Any(hs => !hs.DeleteFlag && hs.HerdClassCode == HHerdClassification.HerdClassCode && hs.HerdClassDesc == HHerdClassification.HerdClassDesc && hs.Id != id));

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                _context.Entry(HHerdClassification).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }

        }

        // POST: HerdClassification/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HHerdClassification>> save(HHerdClassification HHerdClassification)
        {
          if (_context.HHerdClassifications == null)
          {
            return Problem("Entity set 'PCC_DEVContext.HerdClassification' is null!");
          }

            bool hasDuplicateOnSave = (_context.HHerdClassifications?.Any(hs => !hs.DeleteFlag && hs.HerdClassCode == hs.HerdClassCode 
                                        && hs.HerdClassDesc == hs.HerdClassDesc)).GetValueOrDefault();

            // check for duplication
          if (hasDuplicateOnSave)
          {
              return Conflict("Entity already exists");
          }
          try
          {
                _context.HHerdClassifications.Add(HHerdClassification);
                await _context.SaveChangesAsync();
                return CreatedAtAction("save", new { id = HHerdClassification.Id }, HHerdClassification);
          }
          catch (Exception ex) 
          { 
                
                return Problem(ex.GetBaseException().ToString());
          }
        }

        // POST: HerdClassification/delete/5
        [HttpPost]
        public async Task<IActionResult> delete(DeletionModel deletionModel)
        {
            if (_context.HHerdClassifications == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HerdClassification' is null!");
            }
            var HHerdClassification = await _context.HHerdClassifications.FindAsync(deletionModel.id);
            if (HHerdClassification == null || HHerdClassification.DeleteFlag)
            {
                return Conflict("No records found!");
            }

            bool typeCodeExistsInBuffHerd = _context.HBuffHerds.Any(buffHerd => !buffHerd.DeleteFlag && buffHerd.HerdClassDesc == HHerdClassification.HerdClassCode);

            if (typeCodeExistsInBuffHerd)
            {
                return Conflict("Used by other table!");
            }

            try
            {
                HHerdClassification.DeleteFlag = true;
                HHerdClassification.DateDeleted = DateTime.Now;
                HHerdClassification.DeletedBy = deletionModel.deletedBy;
                HHerdClassification.DateRestored = null;
                HHerdClassification.RestoredBy = "";
                _context.Entry(HHerdClassification).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Deletion Successful!");
            }
            catch(Exception ex)
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // GET: HerdClassification/view
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HHerdClassification>>> view()
        {
            if (_context.HHerdClassifications == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HerdClassification' is null!");
            }
            return await _context.HHerdClassifications.Where(HerdClassification => !HerdClassification.DeleteFlag).ToListAsync();
        }

        // POST: HerdClassification/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {

            if(_context.HHerdClassifications == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HerdClassification' is null!");
            }

            var HHerdClassification = await _context.HHerdClassifications.FindAsync(restorationModel.id);
            if (HHerdClassification == null || !HHerdClassification.DeleteFlag)
            {
                return Conflict("No deleted records matched!");
            }

            try
            {
                HHerdClassification.DeleteFlag = !HHerdClassification.DeleteFlag;
                HHerdClassification.DateDeleted = null;
                HHerdClassification.DeletedBy = "";
                HHerdClassification.DateRestored = DateTime.Now;
                HHerdClassification.RestoredBy = restorationModel.restoredBy;

                _context.Entry(HHerdClassification).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
            }
            catch (Exception ex) 
            {
                
                return Problem(ex.GetBaseException().ToString());
            }
        }

        private bool HHerdClassificationExists(int id)
        {
            return (_context.HHerdClassifications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
