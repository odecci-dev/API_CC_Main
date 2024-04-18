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
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Core.Types;
using System.Data;
using System.Data.SqlClient;

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
        public async Task<ActionResult<IEnumerable<HerdPagedModel>>> search(BuffHerdSearchFilterModel searchFilter)
        {
            sanitizeInput(searchFilter);
            validateDate(searchFilter);
            if (!searchFilter.sortBy.Field.IsNullOrEmpty())
            {
                if (searchFilter.sortBy.Field.ToLower().Equals("cowlevel"))
                {
                    searchFilter.sortBy.Field = "HERD_SIZE";
                }
                else
                {
                    SortRequestToColumnNameConverter.convert(searchFilter.sortBy);
                }

            }
            try
            {
                DataTable queryResult = db.SelectDb_WithParamAndSorting(QueryBuilder.buildHerdSearchQuery(searchFilter), searchFilter.sortBy, populateSqlParameters(searchFilter));
                var result = buildHerdPagedModel(searchFilter, queryResult);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // GET: BuffHerds/view/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuffHerdListResponseModel>>> view()
        {
            try
            {
                DataTable dt = db.SelectDb_WithParamAndSorting(QueryBuilder.buildHerdSearchAll(), null, new SqlParameter[]{ });

                if (dt.Rows.Count == 0)
                {
                    return Conflict("No records found!");
                }
                var buffHerdResponseModel= convertDataRowToHerdModel(dt.Rows[0]);
                var buffHerdResponseModels = convertBuffHerdToResponseModel(buffHerdResponseModel);

                return Ok(buffHerdResponseModels);
            }
            catch (Exception ex)
            {
                return Problem(ex.GetBaseException().ToString());
            }
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

            var buffHerd = convertDataRowToHerdModel(buffHerdDataTable.Rows[0]);
            DataTable buffHerdDuplicateCheck = db.SelectDb(QueryBuilder.buildHerdSelectDuplicateQueryByIdHerdNameHerdCode(id, registrationModel.HerdName, registrationModel.HerdCode)).Tables[0];

            // check for duplication
            if (buffHerdDuplicateCheck.Rows.Count > 0)
            {
                return Conflict("Entity already exists");
            }

            DataTable farmOwnerRecordsCheck = db.SelectDb(QueryBuilder.buildFarmOwnerSearchQueryById(buffHerd.Owner)).Tables[0];

            if (farmOwnerRecordsCheck.Rows.Count == 0)
            {
                return Conflict("Farm owner does not exists");
            }

            string farmOwner_update = $@"UPDATE [dbo].[tbl_FarmOwner] SET 
                                             [FirstName] = '" + registrationModel.Owner.FirstName + "'" +
                                            ",[LastName] = '" + registrationModel.Owner.LastName + "'" +
                                            ",[Address] = '" + registrationModel.Owner.Address + "'" +
                                            ",[TelephoneNumber] = '" + registrationModel.Owner.TelNo + "'" +
                                            ",[MobileNumber] = '" + registrationModel.Owner.MNo + "'" +
                                            ",[Email] = '" + registrationModel.Owner.Email + "'" +
                                            " WHERE id = " + buffHerd.Owner;
            string result = db.DB_WithParam(farmOwner_update);

            var farmOwner = convertDataRowToFarmOwnerEntity(farmOwnerRecordsCheck.Rows[0]);

            try
            {
                buffHerd = populateBuffHerd(buffHerd, registrationModel);
                buffHerd.Owner = farmOwner.Id;
                buffHerd.DateUpdated = DateTime.Now;
                buffHerd.UpdatedBy = registrationModel.UpdatedBy;

                _context.Entry(buffHerd).State = EntityState.Modified;
                 _context.SaveChanges();

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
            
            var herdModel = convertDataRowToHerdModel(dt.Rows[0]);

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

            var herdModels = convertDataRowListToHerdModelList(items);
            List<BuffHerdListResponseModel> buffHerdBaseModels = convertBuffHerdToResponseModelList(herdModels);

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
            item.items = buffHerdBaseModels;
            result.Add(item);

            return result;
        }

        private List<HBuffHerd> convertDataRowListToHerdModelList(List<DataRow> dataRowList)
        {
            var herdModelList = new List<HBuffHerd>();

            foreach (DataRow dataRow in dataRowList)
            {
                var herdModel = DataRowToObject.ToObject<HBuffHerd>(dataRow);
                herdModelList.Add(herdModel);
            }

            return herdModelList;
        }

        private HBuffHerd convertDataRowToHerdModel(DataRow dataRow)
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
            if (updateModel.HerdClassDesc != null && updateModel.HerdClassDesc != "")
            {
                buffHerd.HerdClassDesc = updateModel.HerdClassDesc;
            }
            if (updateModel.FeedingSystemCode != null && updateModel.FeedingSystemCode != "")
            {
                buffHerd.FeedingSystemCode = updateModel.FeedingSystemCode;
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
                HerdClassDesc = registrationModel.HerdClassDesc,
                FeedingSystemCode = registrationModel.FeedingSystemCode,
                FarmManager = registrationModel.FarmManager,
                FarmAddress = registrationModel.FarmAddress,
                OrganizationName = registrationModel.OrganizationName,
                Center = registrationModel.Center,
                Photo = registrationModel.Photo
            };

            return BuffHerdModel;
        }

        private List<BuffHerdListResponseModel> convertBuffHerdToResponseModelList(List<HBuffHerd> buffHerdList)
        {
            var buffHerdResponseModels = new List<BuffHerdListResponseModel>();
            foreach (HBuffHerd buffHerd in buffHerdList)
            {
                var buffHerdResponseModel = new BuffHerdListResponseModel()
                {
                    HerdName = buffHerd.HerdName,
                    HerdClassification = buffHerd.HerdClassDesc,
                    CowLevel = buffHerd.HerdSize.ToString(),
                    FarmManager = buffHerd.FarmManager,
                    HerdCode = buffHerd.HerdCode,
                    Photo = buffHerd.Photo,
                    DateOfApplication = buffHerd.DateCreated.ToString("yyyy-MM-dd")
                };
                buffHerdResponseModels.Add(buffHerdResponseModel);
            }
           
            return buffHerdResponseModels;
        }

        private BuffHerdListResponseModel convertBuffHerdToResponseModel(HBuffHerd buffHerd)
        {
            var buffHerdResponseModel = new BuffHerdListResponseModel()
            {
                HerdName = buffHerd.HerdName,
                HerdClassification = buffHerd.HerdClassDesc,
                CowLevel = buffHerd.HerdSize.ToString(),
                FarmManager = buffHerd.FarmManager,
                HerdCode = buffHerd.HerdCode,
                Photo = buffHerd.Photo,
                DateOfApplication = buffHerd.DateCreated.ToString("yyyy-MM-dd")
            };
            return buffHerdResponseModel;
        }

        private Owner populateOwner(HBuffHerd buffHerd)
        {
            DataTable dt = db.SelectDb(QueryBuilder.buildFarmOwnerSearchQueryById(buffHerd.Owner)).Tables[0];
            if (dt.Rows.Count == 0)
            {
                throw new Exception("Owner Record Not Found!");
            }
            var farmOwnerEntity = convertDataRowToFarmOwnerEntity(dt.Rows[0]);

            var owner = new Owner()
            {
                FirstName = farmOwnerEntity.FirstName,
                LastName = farmOwnerEntity.LastName,
                Address = farmOwnerEntity.Address,
                Email = farmOwnerEntity.Email,
                MNo = farmOwnerEntity.MobileNumber,
                TelNo = farmOwnerEntity.TelephoneNumber
            };


            return owner;
        }

        private SqlParameter[] populateSqlParameters(BuffHerdSearchFilterModel searchFilter)
        {

            var sqlParameters = new List<SqlParameter>();

            if (searchFilter.searchValue != null && searchFilter.searchValue != "")
            {
                sqlParameters.Add(new SqlParameter
                {
                    ParameterName = "SearchParam",
                    Value = searchFilter.searchValue ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                });
            }

            if (searchFilter.filterBy != null)
            {
                if (searchFilter.filterBy.BreedTypeCode != null && searchFilter.filterBy.BreedTypeCode != "")
                {
                    sqlParameters.Add(new SqlParameter
                    {
                        ParameterName = "BreedTypeCode",
                        Value = searchFilter.filterBy.BreedTypeCode ?? Convert.DBNull,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                    });
                }

                if (searchFilter.filterBy.HerdClassDesc != null && searchFilter.filterBy.HerdClassDesc != "")
                {
                    sqlParameters.Add(new SqlParameter
                    {
                        ParameterName = "HerdClassDesc",
                        Value = searchFilter.filterBy.HerdClassDesc ?? Convert.DBNull,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                    });
                }

                if (searchFilter.filterBy.feedingSystemCode != null && searchFilter.filterBy.feedingSystemCode != "")
                {
                    sqlParameters.Add(new SqlParameter
                    {
                        ParameterName = "FeedingSystemCode",
                        Value = searchFilter.filterBy.feedingSystemCode ?? Convert.DBNull,
                        SqlDbType = System.Data.SqlDbType.VarChar,
                    });
                }
            }

            if (searchFilter.dateFrom != null && searchFilter.dateFrom != "")
            {
                sqlParameters.Add(new SqlParameter
                {
                    ParameterName = "DateFrom",
                    Value = searchFilter.dateFrom == "" ? Convert.DBNull : searchFilter.dateFrom,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                });
            }

            if (searchFilter.dateTo != "")
            {
                sqlParameters.Add(new SqlParameter
                {
                    ParameterName = "DateTo",
                    Value = searchFilter.dateTo == "" ? Convert.DBNull : searchFilter.dateTo,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                });
            }

            return sqlParameters.ToArray();
        }

        private SqlParameter[] populateSqlParameters(String herdCode)
        {

            var sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "HerdCode",
                Value = herdCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            });

            return sqlParameters.ToArray();
        }

        private void sanitizeInput(BuffHerdSearchFilterModel searchFilter)
        {
            searchFilter.searchValue = StringSanitizer.sanitizeString(searchFilter.searchValue);
            searchFilter.dateFrom = StringSanitizer.sanitizeString(searchFilter.dateFrom);
            searchFilter.dateTo = StringSanitizer.sanitizeString(searchFilter.dateTo);
            searchFilter.filterBy.feedingSystemCode = StringSanitizer.sanitizeString(searchFilter.filterBy.feedingSystemCode);
            searchFilter.filterBy.BreedTypeCode = StringSanitizer.sanitizeString(searchFilter.filterBy.BreedTypeCode);
            searchFilter.filterBy.HerdClassDesc = StringSanitizer.sanitizeString(searchFilter.filterBy.HerdClassDesc);
            searchFilter.sortBy.Field = StringSanitizer.sanitizeString(searchFilter.sortBy.Field);
            searchFilter.sortBy.Sort = StringSanitizer.sanitizeString(searchFilter.sortBy.Sort);
        }

        private void validateDate(BuffHerdSearchFilterModel searchFilter)
        {
            if (!searchFilter.dateFrom.IsNullOrEmpty())
            {
                if (!DateTime.TryParse(searchFilter.dateFrom, out DateTime dateTimeFrom))
                {
                    throw new System.FormatException("Date From is not a valid Date!");
                }
            }

            if (!searchFilter.dateTo.IsNullOrEmpty())
            {
                if (!DateTime.TryParse(searchFilter.dateTo, out DateTime dateTimeTo))
                {
                    throw new System.FormatException("Date To is not a valid Date!");
                }
            }
        }

    }

}
