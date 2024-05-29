using API_PCC.Data;
using API_PCC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;
using System.Text;
using static API_PCC.Controllers.BloodCompsController;
using static API_PCC.Controllers.BuffAnimalsController;
using static API_PCC.Controllers.UserController;
namespace API_PCC.Manager
{
    public class DBMethods
    {
        string sql = "";
        string Stats = "";
        string Mess = "";
        string JWT = "";
        DbManager db = new DbManager();
        private readonly PCC_DEVContext _context;
        #region Models

        public partial class RegistrationModel
        {
            public string Username { get; set; }

            public string Password { get; set; }

            public string Fname { get; set; }

            public string? Lname { get; set; }

            public string? Mname { get; set; }

            public string Email { get; set; }

            public string Gender { get; set; }

            public string? EmployeeId { get; set; }

            public string Jwtoken { get; set; }

            public string? FilePath { get; set; }

            public int? Active { get; set; }

            public string? Cno { get; set; }

            public string? Address { get; set; }

            public int? Status { get; set; }
            public string? CreatedBy { get; set; }

            public int? CenterId { get; set; }

            public bool? AgreementStatus { get; set; }
            public string? UserType { get; set; }
        }

        #endregion
        public List<animalresult> getanimallist()
        {



            string sql = $@"SELECT        A_Buff_Animal.id, A_Buff_Animal.Animal_ID_Number, A_Buff_Animal.Animal_Name, A_Buff_Animal.Photo, A_Buff_Animal.Herd_Code, A_Buff_Animal.RFID_Number, A_Buff_Animal.Date_of_Birth, A_Buff_Animal.Sex, 
                         A_Buff_Animal.Birth_Type, A_Buff_Animal.Country_Of_Birth, A_Buff_Animal.Origin_Of_Acquisition, A_Buff_Animal.Date_Of_Acquisition, A_Buff_Animal.Marking, A_Buff_Animal.Type_Of_Ownership, A_Buff_Animal.Delete_Flag, 
                         A_Buff_Animal.Status, A_Buff_Animal.Created_By, A_Buff_Animal.Created_Date, A_Buff_Animal.Updated_By, A_Buff_Animal.Update_Date, A_Buff_Animal.Date_Deleted, A_Buff_Animal.Deleted_By, 
                         A_Buff_Animal.Date_Restored, A_Buff_Animal.Restored_By, A_Buff_Animal.BreedRegistryNumber, A_Breed.Breed_Code, A_Blood_Comp.Blood_Code, tbl_StatusModel.Status AS StatusName
FROM            A_Buff_Animal INNER JOIN
                         A_Breed ON A_Buff_Animal.Breed_Code = A_Breed.Breed_Code INNER JOIN
                         A_Blood_Comp ON A_Buff_Animal.Blood_Code = A_Blood_Comp.Blood_Code INNER JOIN
                         tbl_StatusModel ON A_Buff_Animal.Status = tbl_StatusModel.id
WHERE        (A_Buff_Animal.Delete_Flag <> 1)";
            var result = new List<animalresult>();
            DataTable table = db.SelectDb(sql).Tables[0];

            foreach (DataRow dr in table.Rows)
            {
                var item = new animalresult();
                item.Id = dr["Id"].ToString();
                item.Animal_ID_Number = dr["Animal_ID_Number"].ToString();
                item.Animal_Name = dr["Animal_Name"].ToString();
                item.Photo = dr["Photo"].ToString();
                item.Herd_Code = dr["Herd_Code"].ToString();
                item.RFID_Number = dr["RFID_Number"].ToString();
                item.Date_of_Birth = dr["Date_of_Birth"].ToString();
                item.Sex = dr["Sex"].ToString();
                item.Birth_Type = dr["Birth_Type"].ToString();
                item.Country_Of_Birth = dr["Country_Of_Birth"].ToString();
                item.Origin_Of_Acquisition = dr["Origin_Of_Acquisition"].ToString();
                item.Date_Of_Acquisition = dr["Date_Of_Acquisition"].ToString();
                item.Marking = dr["Marking"].ToString();
                item.Type_Of_Ownership = dr["Type_Of_Ownership"].ToString();
                item.Delete_Flag = dr["Delete_Flag"].ToString();
                item.BreedRegistryNumber = dr["BreedRegistryNumber"].ToString();
                item.Breed_Code = dr["Breed_Code"].ToString();
                item.Blood_Code = dr["Blood_Code"].ToString();
                item.Restored_By = dr["Restored_By"].ToString();
                item.Date_Restored = dr["Date_Restored"].ToString();
                item.Date_Deleted = dr["Date_Deleted"].ToString();
                item.Update_Date = dr["Update_Date"].ToString();
                item.Updated_By = dr["Updated_By"].ToString();
                item.Created_Date = dr["Created_Date"].ToString();
                item.Created_By = dr["Created_By"].ToString();
                item.Deleted_By = dr["Deleted_By"].ToString();
                item.Delete_Flag = dr["Delete_Flag"].ToString();
                item.Status = dr["Status"].ToString();
                item.StatusName = dr["StatusName"].ToString();
               

                result.Add(item);
            }

            return result;
        }
        public List<TblCenterModel> getcenterlist()
        {



            string sql = $@"SELECT        tbl_CenterModel.id, tbl_CenterModel.CenterCode, tbl_CenterModel.CenterName, tbl_CenterModel.Center_desc, tbl_CenterModel.Address, 
                            tbl_CenterModel.ContactPerson, tbl_CenterModel.MobileNumber, 
                         tbl_CenterModel.TelNumber, tbl_CenterModel.Email, tbl_CenterModel.Created_By, 
                         tbl_CenterModel.Date_Created, tbl_CenterModel.Updated_By, tbl_CenterModel.Date_Updated, 
                         tbl_CenterModel.Deleted_By, tbl_CenterModel.Date_Deleted, tbl_CenterModel.Restored_By,
                         tbl_CenterModel.Date_Restored, tbl_CenterModel.Delete_Flag, tbl_CenterModel.Status, tbl_StatusModel.Status AS StatusId
                         FROM            tbl_CenterModel INNER JOIN
                         tbl_StatusModel ON tbl_CenterModel.Status = tbl_StatusModel.id";
            var result = new List<TblCenterModel>();
            DataTable table = db.SelectDb(sql).Tables[0];

            foreach (DataRow dr in table.Rows)
            {
                var datedeleted = dr["Date_Deleted"].ToString() == "" ? "1990-01-01" : Convert.ToDateTime(dr["Date_Deleted"].ToString()).ToString("yyyy-MM-dd");
                var daterestored = dr["Date_Restored"].ToString() == "" ? "1990-01-01" : Convert.ToDateTime(dr["Date_Restored"].ToString()).ToString("yyyy-MM-dd");
                var item = new TblCenterModel();
                item.Id = int.Parse(dr["id"].ToString());
                item.CenterCode = dr["CenterCode"].ToString();
                item.CenterName = dr["CenterName"].ToString();
                item.CenterDesc = dr["Center_desc"].ToString();
                item.Address = dr["Address"].ToString();
                item.ContactPerson = dr["ContactPerson"].ToString();
                item.MobileNumber = dr["MobileNumber"].ToString();
                item.TelNumber = dr["TelNumber"].ToString();
                item.Email = dr["Email"].ToString();
                item.CreatedBy = dr["Created_By"].ToString();
                item.DeletedBy = dr["Deleted_By"].ToString();
                item.DateDeleted = DateTime.Parse(datedeleted);
                item.RestoredBy = dr["Restored_By"].ToString();
                item.DateRestored =DateTime.Parse(daterestored);
                item.DeleteFlag = bool.Parse(dr["Delete_Flag"].ToString());
                item.StatusName = dr["StatusId"].ToString();
                item.StatusId = int.Parse(dr["Status"].ToString());
                

                result.Add(item);
            }

            return result;
        }
        public StatusReturns GetUserLogIn(string username, string password, string? ipaddress, string? location)
        {
            var result = new List<TblUsersModel>();
            bool compr_user = false;
            if (username.Length != 0 || password.Length != 0)
            {
                var param = new IDataParameter[]
                {
                    new SqlParameter("@Username",username),
                    new SqlParameter("@Password",Cryptography.Encrypt(password))
                };
                DataTable dt = db.SelectDb_SP("GetUserLogIn", param).Tables[0];
                if (dt.Rows.Count != 0)
                {
                    string user_statId = dt.Rows[0]["StatusId"].ToString();
                    string user_activeId = dt.Rows[0]["ActiveStatusId"].ToString();
                    if (user_activeId == "1")
                    {
                        //active
                        switch (user_statId)
                        {
                            case "3":
                                //VERIFIED
                                Stats = "Error";
                                Mess = "Your account is under screening. Please contact administrator.";
                                JWT = "";
                                break;
                            case "4":
                                //UNVERIFIED
                                Stats = "Error";
                                Mess = "Your account is unverified. Please contact administrator.";
                                JWT = "";
                                break;
                            case "5":
                                //APPROVED
                           

                                compr_user = String.Equals(dt.Rows[0]["Username"].ToString().Trim(), username, StringComparison.Ordinal);

                                if (compr_user)
                                {
                                    string pass = Cryptography.Decrypt(dt.Rows[0]["password"].ToString().Trim());
                                    if ((pass).Equals(password))
                                    {
                                        StringBuilder str_build = new StringBuilder();
                                        Random random = new Random();
                                        int length = 8;
                                        char letter;

                                        for (int i = 0; i < length; i++)
                                        {
                                            double flt = random.NextDouble();
                                            int shift = Convert.ToInt32(Math.Floor(25 * flt));
                                            letter = Convert.ToChar(shift + 2);
                                            str_build.Append(letter);
                                        }
                                        //gv.AudittrailLogIn("Successfully", "Log In Form", dt.Rows[0]["EmployeeID"].ToString(), 7);
                                        var token = Cryptography.Encrypt(str_build.ToString());
                                        string strtokenresult = token;
                                        string[] charsToRemove = new string[] { "/", ",", ".", ";", "'", "=", "+" };
                                        foreach (var c in charsToRemove)
                                        {
                                            strtokenresult = strtokenresult.Replace(c, string.Empty);
                                        }

                                        string query = $@"update tbl_UsersModel set JWToken='" + string.Concat(strtokenresult.TakeLast(15)) + "' where id = '" + dt.Rows[0]["id"].ToString() + "'";
                                        db.DB_WithParam(query);

                                        Stats = "Ok";
                                        Mess = "Successfully Log In";
                                        JWT = string.Concat(strtokenresult.TakeLast(15));
                                    }
                                    else
                                    {
                                        string sql = $@"select * from tbl_Attempts where UserId ='" + dt.Rows[0]["Id"].ToString() + "'";
                                        DataTable user_dt = db.SelectDb(sql).Tables[0];
                                        if (user_dt.Rows.Count != 0)
                                        {
                                            //update
                                            int attemp_count = int.Parse(user_dt.Rows[0]["AttemptCount"].ToString());
                                            if (attemp_count > 5)
                                            {
                                                string update_attempts = $@"update tbl_Attempts set AttemptCount ='" + attemp_count + 1 + "'  where id ='" + dt.Rows[0]["Id"].ToString() + "'";
                                                db.DB_WithParam(update_attempts);
                                            }
                                            else
                                            {
                                                Stats = "Error";
                                                Mess = "User LogIn Attempts Exceeded. Please contact admin";
                                                JWT = "";
                                            }


                                        }
                                        else
                                        {
                                            string OTPInsert = $@"INSERT INTO [dbo].[tbl_Attempts]
                                                               ([UserId]
                                                               ,[AttemptCount]
                                                               ,[IPAddress]
                                                               ,[Location])
                                                                VALUES
                                                               ('" + dt.Rows[0]["Id"].ToString() + "'" +
                                                               ",'1'," +
                                                               "'" + ipaddress + "'," +
                                                               "'" + location + "')";
                                            db.DB_WithParam(OTPInsert);
                                            Stats = "Error";
                                            Mess = "Invalid Log In";
                                            JWT = "";
                                            //insert

                                        }
                                        //update login attempts

                                    }
                                }
                                break;
                            case "6":
                                //REGISTERED
                                Stats = "Error";
                                Mess = "Your account is for approval. Please contact administrator.";
                                JWT = "";

                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        //inactive
                        Stats = "Error";
                        Mess = "User is InActive. Please contact your administrator. ";
                        JWT = "";

                    }

                }
                else
                {
                    Stats = "Error";
                    Mess = "Invalid LogIn";
                    JWT = "";
                }


            }
            else
            {


            }
            StatusReturns results = new StatusReturns
            {
                Status = Stats,
                Message = Mess,
                JwtToken = JWT
            };
            return results;
        }

        public List<ABloodComp_2> getBlodyList()
        {



            string sql = $@"select Id, Blood_Code, Blood_Desc, Status,Date_Created from A_Blood_Comp where Delete_Flag ='False'";
            var result = new List<ABloodComp_2>();
            DataTable table = db.SelectDb(sql).Tables[0];

            foreach (DataRow dr in table.Rows)
            {
                var item = new ABloodComp_2();
                item.Id = int.Parse(dr["id"].ToString());
                item.BloodCode = dr["Blood_Code"].ToString();
                item.BloodDesc = dr["Blood_Desc"].ToString();
                item.Status = dr["Status"].ToString();
                item.DateCreated = dr["Date_Created"].ToString();

                result.Add(item);
            }

            return result;
        }

    }
}

