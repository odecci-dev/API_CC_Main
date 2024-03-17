using API_PCC.ApplicationModels;
using API_PCC.ApplicationModels.Common;
using API_PCC.Data;
using API_PCC.DtoModels;
using API_PCC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static API_PCC.Manager.DBMethods;
using System;
using System.Data;
using API_PCC.Manager;
using API_PCC.Constants;
using API_PCC.Utils;
using System.Collections.Generic;
using NuGet.Protocol.Core.Types;
using Org.BouncyCastle.Utilities;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class HBuffHerdsController : ControllerBase
    {
        DbManager db = new DbManager();

        private readonly PCC_DEVContext _context;

        public HBuffHerdsController(PCC_DEVContext context)
        {
            _context = context;
        }

        // POST: BuffHerds/search
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HBuffHerd>>> search(BuffHerdSearchFilterModel searchFilter)
        {
            try
            {
                DataTable dt = db.SelectDb(QueryBuilder.buildHerdSearchQuery(searchFilter.searchValue)).Tables[0];
                var result = buildHerdPagedModel(searchFilter, dt);
                return Ok(result);
            }

            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
            }
        }

        // GET: BuffHerds/view/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HBuffHerd>> view(int id)
        {
            var hBuffHerd = await _context.HBuffHerds.FindAsync(id);

            if (hBuffHerd == null || hBuffHerd.DeleteFlag)
            {
                return Conflict("No records found!");
            }

            return Ok(hBuffHerd);
        }
       
        // GET: BuffHerds/archive
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HBuffHerd>>> archive(BuffHerdSearchFilterModel searchFilter)
        {
            DataTable dt = db.SelectDb(QueryBuilder.buildHerdArchiveQuery()).Tables[0];

            var result = buildHerdPagedModel(searchFilter, dt);
            return Ok(result);
        }

        // PUT: BuffHerds/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, BuffHerdUpdateModel registrationModel)
        {
            if (_context.HBuffHerds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HBuffHerds' is null!");
            }

            var buffHerd = _context.HBuffHerds.AsNoTracking().Where(buffHerd => !buffHerd.DeleteFlag && buffHerd.Id == id).FirstOrDefault();

            if (buffHerd == null)
            {
                return Conflict("No records matched!");
            }

            bool hasDuplicateOnUpdate = (_context.HBuffHerds?.Any(buffHerd => !buffHerd.DeleteFlag && buffHerd.HerdName == registrationModel.HerdName && buffHerd.HerdCode == registrationModel.HerdCode && buffHerd.Id != id)).GetValueOrDefault();

            // check for duplication
            if (hasDuplicateOnUpdate)
            {
                return Conflict("Entity already exists");
            }

            try
            {
                buffHerd = populateBuffHerd(buffHerd, registrationModel);
                buffHerd.HerdSize = registrationModel.HerdSize;
                buffHerd.DateUpdated = DateTime.Now;
                buffHerd.UpdatedBy = registrationModel.UpdatedBy;

                _context.Entry(buffHerd).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
            }
        }

        // POST: BuffHerds/save
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HBuffHerd>> save(BuffHerdRegistrationModel registrationModel)
        {
            if (_context.HBuffHerds == null)
            {
                return Problem("Entity set 'PCC_DEVContext.HBuffHerds'  is null.");
            }

            bool hasDuplicateOnSave = (_context.HBuffHerds?.Any(buffHerd => !buffHerd.DeleteFlag && (buffHerd.HerdCode == registrationModel.HerdCode || buffHerd.HerdName == registrationModel.HerdName))).GetValueOrDefault();

            if (hasDuplicateOnSave)
            {
                return Conflict("Herd already exists");
            }

            try
            {
                var BuffHerdModel = buildBuffHerd(registrationModel);
                BuffHerdModel.DateCreated = DateTime.Now;

                _context.HBuffHerds.Add(BuffHerdModel);
                await _context.SaveChangesAsync();

                return Ok("Herd successfully registered!");
            }
            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
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

                return Problem(ex.GetBaseException().ToString());
            }
        }

        // POST: BuffHerds/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {
            DataTable dt = db.SelectDb(QueryBuilder.buildHerdSelectForRestoreQuery(restorationModel.id)).Tables[0];

            if (dt.Rows.Count == 0)
            {
                return Conflict("No deleted records matched!");
            }
            
            var herdModel = populateHerdModel(dt.Rows[0]);

            DataTable dtForDuplicateCheck = db.SelectDb(QueryBuilder.buildHerdCheckDuplicateForRestoreQuery(herdModel.HerdName, herdModel.HerdCode)).Tables[0];

            if (dtForDuplicateCheck.Rows.Count > 0)
            {
                return Conflict("Entity already exists!!");
            }

            try
            {
                herdModel.DeleteFlag = !herdModel.DeleteFlag;
                herdModel.DateDeleted = null;
                herdModel.DeletedBy = "";
                herdModel.DateRestored = DateTime.Now;
                herdModel.RestoredBy = restorationModel.restoredBy;

                _context.Entry(herdModel).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
            }
            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
            }
        }

        private List<HerdPagedModel> buildHerdPagedModel(BuffHerdSearchFilterModel searchFilter, DataTable dt)
        {

            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;

            int totalItems = dt.Rows.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
            items = dt.AsEnumerable().Skip((page - 1) * pagesize).Take(pagesize).ToList();

            var herdModel = populateHerdModel(items);

            var result = new List<HerdPagedModel>();
            var item = new HerdPagedModel();

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
            item.items = herdModel;
            result.Add(item);

            return result;
        }


        private List<HBuffHerd> populateHerdModel(List<DataRow> dataRowList)
        {
            var herdModelList = new List<HBuffHerd>();

            foreach (DataRow dataRow in dataRowList)
            {
                var herdModel = DataRowToObject.ToObject<HBuffHerd>(dataRow);
                herdModelList.Add(herdModel);
            }

            return herdModelList;
        }

        private HBuffHerd populateHerdModel(DataRow dataRow)
        {
            return DataRowToObject.ToObject<HBuffHerd>(dataRow);
        }

        private HBuffHerd populateBuffHerd(HBuffHerd buffHerd, BuffHerdUpdateModel updateModel)
        {

            if (updateModel.HerdName != null && updateModel.HerdName != "")
            {
                buffHerd.HerdName = updateModel.HerdName;
            }
            if (updateModel.HerdCode != null && updateModel.HerdCode != "")
            {
                buffHerd.HerdCode = updateModel.HerdCode;
            }
            if (updateModel.BBuffCode != null && updateModel.BBuffCode != "")
            {
                buffHerd.BBuffCode = updateModel.BBuffCode;
            }
            if (updateModel.FCode != null && updateModel.FCode != "")
            {
                buffHerd.FCode = updateModel.FCode;
            }
            if (updateModel.HTypeCode != null && updateModel.HTypeCode != "")
            {
                buffHerd.HTypeCode = updateModel.HTypeCode;
            }
            if (updateModel.FeedCode != null && updateModel.FeedCode != "")
            {
                buffHerd.FeedCode = updateModel.FeedCode;
            }
            if (updateModel.FarmManager != null && updateModel.FarmManager != "")
            {
                buffHerd.FarmManager = updateModel.FarmManager;
            }
            if (updateModel.FarmAddress != null && updateModel.FarmAddress != "")
            {
                buffHerd.FarmAddress = updateModel.FarmAddress;
            }
            if (updateModel.Owner.Fname != null && updateModel.Owner.Fname != "")
            {
                buffHerd.Owner = updateModel.Owner.Fname + updateModel.Owner.Lname ;
            }
            if (updateModel.Owner.Address != null && updateModel.Owner.Address != "")
            {
                buffHerd.Address = updateModel.Owner.Address;
            }
            if (updateModel.Owner.TelNo != null && updateModel.Owner.TelNo != "")
            {
                buffHerd.TelNo = updateModel.Owner.TelNo;
            }
            if (updateModel.Owner.MNo != null && updateModel.Owner.MNo != "")
            {
                buffHerd.MNo = updateModel.Owner.MNo;
            }
            if (updateModel.Owner.Email != null && updateModel.Owner.Email != "")
            {
                buffHerd.Email = updateModel.Owner.Email;
            }
            if (updateModel.OrganizationName != null && updateModel.OrganizationName != "")
            {
                buffHerd.OrganizationName = updateModel.OrganizationName;
            }
            return buffHerd;
        }


        private HBuffHerd buildBuffHerd(BuffHerdRegistrationModel registrationModel)
        {
            var BuffHerdModel = new HBuffHerd()
            {
                HerdName = registrationModel.HerdName,
                HerdCode = registrationModel.HerdCode,
                HerdSize = registrationModel.HerdSize,
                BBuffCode = registrationModel.BBuffCode,
                FCode = registrationModel.FCode,
                HTypeCode = registrationModel.HTypeCode,
                FeedCode = registrationModel.FeedCode,
                FarmManager = registrationModel.FarmManager,
                FarmAddress = registrationModel.FarmAddress,
                Owner = registrationModel.Owner.Fname + " "+ registrationModel.Owner.Lname,
                Address = registrationModel.Owner.Address,
                TelNo = registrationModel.Owner.TelNo,
                MNo = registrationModel.Owner.MNo,
                Email = registrationModel.Owner.Email,
                OrganizationName = registrationModel.OrganizationName,
                CreatedBy = registrationModel.CreatedBy,
                Center = registrationModel.Center
            };

            return BuffHerdModel;
        }

    }

}
