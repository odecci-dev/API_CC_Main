using API_PCC.ApplicationModels;
using API_PCC.ApplicationModels.Common;
using API_PCC.Data;
using API_PCC.DtoModels;
using API_PCC.EntityModels;
using API_PCC.Manager;
using API_PCC.Models;
using API_PCC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Data;

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
        [HttpGet("{herdCode}")]
        public async Task<ActionResult<HBuffHerd>> view(String herdCode)
        {
            DataTable dt = db.SelectDb(QueryBuilder.buildHerdSearchQuery(herdCode)).Tables[0];

            if (dt.Rows.Count == 0)
            {
                return Conflict("No records found!");
            }

            return Ok(DataRowToObject.ToObject<HBuffHerd>(dt.Rows[0]));
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

            DataTable buffHerdDataTable = db.SelectDb(QueryBuilder.buildHerdSelectQueryById(id)).Tables[0];

            if (buffHerdDataTable.Rows.Count == 0)
            {
                return Conflict("No records matched!");
            }

            DataTable buffHerdDuplicateCheck = db.SelectDb(QueryBuilder.buildHerdSelectDuplicateQueryByIdHerdNameHerdCode(id, registrationModel.HerdName, registrationModel.HerdCode)).Tables[0];

            // check for duplication
            if (buffHerdDuplicateCheck.Rows.Count > 0)
            {
                return Conflict("Entity already exists");
            }

            DataTable farmOwnerRecordsCheck = db.SelectDb(QueryBuilder.buildFarmOwnerSearchQueryByFirstNameAndLastName(registrationModel.Owner.FirstName, registrationModel.Owner.LastName)).Tables[0];

            if (farmOwnerRecordsCheck.Rows.Count == 0)
            {
                // Should we create a new farmer here as well?
                return Conflict("Farm owner does not exists");
            }

            var farmOwner = convertDataRowToFarmOwnerEntity(farmOwnerRecordsCheck.Rows[0]);
            var buffHerd = populateHerdModel(buffHerdDataTable.Rows[0]);

            try
            {
                buffHerd = populateBuffHerd(buffHerd, registrationModel);
                buffHerd.Owner = farmOwner.Id;
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

            try
            {
                DataTable duplicateCheck = db.SelectDb(QueryBuilder.buildHerdCheckDuplicateQuery(registrationModel.HerdName, registrationModel.HerdCode)).Tables[0];

                if (duplicateCheck.Rows.Count > 0)
                {
                    return Conflict("Herd already exists");
                }

                var BuffHerdModel = buildBuffHerd(registrationModel);

                DataTable farmOwnerRecordsCheck = db.SelectDb(QueryBuilder.buildFarmOwnerSearchQueryByFirstNameAndLastName(registrationModel.Owner.FirstName, registrationModel.Owner.LastName)).Tables[0];
            
                if (farmOwnerRecordsCheck.Rows.Count == 0)
                {
                    // Create new Farm Owner Record
                    string user_insert = $@"INSERT INTO [dbo].[tbl_FarmOwner]
                                                ([FirstName]
                                                ,[LastName]
                                                ,[Address]
                                                ,[TelephoneNumber]
                                                ,[MobileNumber]
                                                ,[Email])
                                            VALUES
                                                ('" + registrationModel.Owner.FirstName + "'" +
                                                ",'" + registrationModel.Owner.LastName + "'," +
                                                "'" + registrationModel.Owner.Address + "'," +
                                                "'" + registrationModel.Owner.TelNo + "'," +
                                                "'" + registrationModel.Owner.MNo + "'," +
                                                "'" + registrationModel.Owner.Email + "')";
                    string test = db.DB_WithParam(user_insert);

                    DataTable farmOwnerRecord = db.SelectDb(QueryBuilder.buildFarmOwnerSearchQueryByFirstNameAndLastName(registrationModel.Owner.FirstName, registrationModel.Owner.LastName)).Tables[0];

                    var farmOwner = convertDataRowToFarmOwnerEntity(farmOwnerRecord.Rows[0]);
                    BuffHerdModel.Owner = farmOwner.Id;
                } else
                {
                    var farmOwner = convertDataRowToFarmOwnerEntity(farmOwnerRecordsCheck.Rows[0]);
                    BuffHerdModel.Owner = farmOwner.Id;
                }

                BuffHerdModel.CreatedBy = registrationModel.CreatedBy;
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

        private TblFarmOwner convertDataRowToFarmOwnerEntity(DataRow dataRow)
        {
            var farmOwner = DataRowToObject.ToObject<TblFarmOwner>(dataRow);

            return farmOwner;
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

            DataTable dtForDuplicateCheck = db.SelectDb(QueryBuilder.buildHerdCheckDuplicateQuery(herdModel.HerdName, herdModel.HerdCode)).Tables[0];

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
            if (updateModel.BreedTypeCode != null && updateModel.BreedTypeCode != "")
            {
                buffHerd.BreedTypeCode = updateModel.BreedTypeCode;
            }
            if (updateModel.FarmAffilCode != null && updateModel.FarmAffilCode != "")
            {
                buffHerd.FarmAffilCode = updateModel.FarmAffilCode;
            }
            if (updateModel.HerdClassCode != null && updateModel.HerdClassCode != "")
            {
                buffHerd.HerdClassCode = updateModel.HerdClassCode;
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
            if (updateModel.OrganizationName != null && updateModel.OrganizationName != "")
            {
                buffHerd.OrganizationName = updateModel.OrganizationName;
            }
            return buffHerd;
        }


        private HBuffHerd buildBuffHerd(BuffHerdBaseModel registrationModel)
        {
            var BuffHerdModel = new HBuffHerd()
            {
                HerdName = registrationModel.HerdName,
                HerdCode = registrationModel.HerdCode,
                HerdSize = registrationModel.HerdSize,
                BreedTypeCode = registrationModel.BreedTypeCode,
                FarmAffilCode = registrationModel.FarmAffilCode,
                HerdClassCode = registrationModel.HerdClassCode,
                FeedCode = registrationModel.FeedCode,
                FarmManager = registrationModel.FarmManager,
                FarmAddress = registrationModel.FarmAddress,
                OrganizationName = registrationModel.OrganizationName,
                Center = registrationModel.Center
            };

            return BuffHerdModel;
        }

    }

}
