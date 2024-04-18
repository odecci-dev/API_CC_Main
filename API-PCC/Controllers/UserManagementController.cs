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
    public class UserManagementController : ControllerBase
    {
        private readonly PCC_DEVContext _context;
        DbManager db = new DbManager();

        public UserManagementController(PCC_DEVContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<UserPagedModel>>> List(CommonSearchFilterModel searchFilter)
        {
            try
            {
                DataTable queryResult = db.SelectDb_WithParamAndSorting(QueryBuilder.buildUserSearchQuery(searchFilter), null, populateSqlParameters(searchFilter));
                var result = buildUserPagedModel(searchFilter, queryResult);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<UserPagedModel>>> UserForApprovalList(CommonSearchFilterModel searchFilter)
        {
            try
            {
                DataTable queryResult = db.SelectDb_WithParamAndSorting(QueryBuilder.buildUserForApprovalSearchQuery(searchFilter), null, populateSqlParameters(searchFilter));
                var result = buildUserPagedModel(searchFilter, queryResult);
                return Ok(result);
            }

            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
            }
        }

        // GET: UserManagemetn/search/5
        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<UserResponseModel>>> search(string username)
        {
            try
            {
                DataTable queryResult = db.SelectDb_WithParamAndSorting(QueryBuilder.buildUserSearchQuery(), null, populateSqlParameters(username));
                if (queryResult.Rows.Count == 0)
                {
                    return Conflict("No records found!");
                }
                var userModels = convertDataRowToUserList(queryResult.AsEnumerable().ToList());
                List<UserResponseModel> userResponseModels = convertUserListToResponseModelList(userModels);

                return Ok(userResponseModels);
            }
            catch (Exception ex)
            {
                return Problem(ex.GetBaseException().ToString());
            }
        }

        // PUT: UserManagement/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, UserUpdateModel userUpdateModel)
        {
            DataTable userRecord = db.SelectDb_WithParamAndSorting(QueryBuilder.buildUserSearchQueryById(), null, populateSqlParameters(id));

            if (userRecord.Rows.Count == 0)
            {
                return Conflict("No records matched!");
            }

            DataTable userDuplicateCheck = db.SelectDb_WithParamAndSorting(QueryBuilder.buildUserDuplicateCheckUpdateQuery(), null, populateSqlParameters(id, userUpdateModel));

            // check for duplication
            if (userDuplicateCheck.Rows.Count > 0)
            {
                return Conflict("Entity already exists");
            }

            var userModel = convertDataRowToUser(userRecord.Rows[0]);

            try
            {
                populateUser(userModel, userUpdateModel);
                _context.Entry(userModel).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Update Successful!");
            }
            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
            }
        }

        private void populateUser(TblUsersModel userModel, UserUpdateModel userUpdateModel)
        {
            userModel.Username = userUpdateModel.Username;
            userModel.Password = Cryptography.Encrypt(userUpdateModel.Password);
            userModel.Fullname = userUpdateModel.Fullname;
            userModel.Fname = userUpdateModel.Fname;
            userModel.Lname = userUpdateModel.Lname;
            userModel.Mname = userUpdateModel.Mname;
            userModel.Email = userUpdateModel.Email;
            userModel.Gender = userUpdateModel.Gender;
            userModel.EmployeeId = userUpdateModel.EmployeeId;
            userModel.Active = userUpdateModel.Active;
            userModel.Cno = userUpdateModel.Cno;
            userModel.Address = userUpdateModel.Address;
            userModel.CenterId = userUpdateModel.CenterId;
            userModel.AgreementStatus = userUpdateModel.AgreementStatus;
    }

        // POST: UserManagement/delete/5
        [HttpPost]
        public async Task<IActionResult> delete(DeletionModel deletionModel)
        {
            DataTable userRecord = db.SelectDb_WithParamAndSorting(QueryBuilder.buildUserSearchQueryById(), null, populateSqlParameters(deletionModel.id));

            if (userRecord.Rows.Count == 0)
            {
                return Conflict("No records found!");
            }

            var userModel = convertDataRowToUser(userRecord.Rows[0]);

            try
            {
                userModel.DeleteFlag = true;
                userModel.DateDeleted = DateTime.Now;
                userModel.DeletedBy = deletionModel.deletedBy;
                userModel.DateRestored = null;
                userModel.RestoredBy = "";
                _context.Entry(userModel).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Deletion Successful!");
            }
            catch (Exception ex)
            {

                return Problem(ex.GetBaseException().ToString());
            }
        }

        // POST: UserManagemetn/restore/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> restore(RestorationModel restorationModel)
        {


            DataTable userRecord = db.SelectDb_WithParamAndSorting(QueryBuilder.buildUserDeletedSearchQueryById(), null, populateSqlParameters(restorationModel.id));

            if (userRecord.Rows.Count == 0)
            {
                return Conflict("No deleted records found!");
            }

            var userModel = convertDataRowToUser(userRecord.Rows[0]);

            try
            {
                userModel.DeleteFlag = !userModel.DeleteFlag;
                userModel.DateDeleted = null;
                userModel.DeletedBy = "";
                userModel.DateRestored = DateTime.Now;
                userModel.RestoredBy = restorationModel.restoredBy;

                _context.Entry(userModel).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Restoration Successful!");
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

        private SqlParameter[] populateSqlParameters(int id)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "Id",
                Value = id,
                SqlDbType = System.Data.SqlDbType.VarChar,
            });
            return sqlParameters.ToArray();
        }

        private SqlParameter[] populateSqlParameters(string username)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "Username",
                Value = username ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            });
            return sqlParameters.ToArray();
        }
        private SqlParameter[] populateSqlParameters(int id, UserUpdateModel userUpdateModel)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "Id",
                Value = id,
                SqlDbType = System.Data.SqlDbType.VarChar,
            });

            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "Username",
                Value = userUpdateModel.Username,
                SqlDbType = System.Data.SqlDbType.VarChar,
            });

            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "Fullname",
                Value = userUpdateModel.Fullname,
                SqlDbType = System.Data.SqlDbType.VarChar,
            });

            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "Fname",
                Value = userUpdateModel.Fname,
                SqlDbType = System.Data.SqlDbType.VarChar,
            });

            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "Lname",
                Value = userUpdateModel.Lname,
                SqlDbType = System.Data.SqlDbType.VarChar,
            });

            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "Mname",
                Value = userUpdateModel.Mname,
                SqlDbType = System.Data.SqlDbType.VarChar,
            });

            sqlParameters.Add(new SqlParameter
            {
                ParameterName = "Email",
                Value = userUpdateModel.Email,
                SqlDbType = System.Data.SqlDbType.VarChar,
            });

            return sqlParameters.ToArray();
        }

        private List<UserPagedModel> buildUserPagedModel(CommonSearchFilterModel searchFilter, DataTable dt)
        {
            int pagesize = searchFilter.pageSize == 0 ? 10 : searchFilter.pageSize;
            int page = searchFilter.page == 0 ? 1 : searchFilter.page;
            var items = (dynamic)null;

            int totalItems = dt.Rows.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
            items = dt.AsEnumerable().Skip((page - 1) * pagesize).Take(pagesize).ToList();


            var userModels = convertDataRowToUserList(items);
            List<UserResponseModel> userResponseModels = convertUserListToResponseModelList(userModels);

            var result = new List<UserPagedModel>();
            var item = new UserPagedModel();

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
            item.items = userResponseModels;
            result.Add(item);

            return result;
        }

        private List<TblUsersModel> convertDataRowToUserList(List<DataRow> dataRowList)
        {
            var userList = new List<TblUsersModel>();

            foreach (DataRow dataRow in dataRowList)
            {
                var user = DataRowToObject.ToObject<TblUsersModel>(dataRow);
                userList.Add(user);
            }

            return userList;
        }

        private TblUsersModel convertDataRowToUser(DataRow dataRow)
        {
            return DataRowToObject.ToObject<TblUsersModel>(dataRow);
        }

        private List<UserResponseModel> convertUserListToResponseModelList(List<TblUsersModel> userList)
        {
            var userResponseModels = new List<UserResponseModel>();

            foreach (TblUsersModel user in userList)
            {
                var userResponseModel = new UserResponseModel()
                {
                    Id= user.Id,
                    FilePath= user.FilePath,
                    Username = user.Username,
                    Fullname = user.Fullname,
                    Fname = user.Fname,
                    Lname = user.Lname,
                    Mname = user.Mname,
                    Email = user.Email,
                    Gender = user.Gender,
                    EmployeeId = user.EmployeeId,
                    Active = user.Active,
                    Cno = user.Cno,
                    Address = user.Address,
                    CenterId = user.CenterId,
                    AgreementStatus = user.AgreementStatus
                };
                userResponseModels.Add(userResponseModel);
            }

            return userResponseModels;
        }

    }
}
