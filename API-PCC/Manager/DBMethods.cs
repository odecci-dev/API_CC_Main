using API_PCC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace API_PCC.Manager
{
    public class DBMethods
    {
        string sql = "";
        string Stats = "";
        string Mess = "";
        string JWT = "";
        DbManager db = new DbManager();
        #region Models

        public class StaturReturns
        {
            public string? Status { get; set; }
            public string? Message { get; set; }
            public string? JwtToken { get; set; }
        }

        #endregion

        public StaturReturns GetUserLogIn(string username,string password)
        {
            var result = new List<TblUsersModel>();
           
            if(username.Length != 0 || password.Length !=0)
            {
                    var param = new IDataParameter[]
                    {
                    new SqlParameter("@Username",username),
                    new SqlParameter("@ApplicationId",Cryptography.Encrypt(password))
                    };
                    DataTable dt = db.SelectDb_SP("GetUserLogIn", param).Tables[0];
                if (dt.Rows.Count != 0)
                {
                    string user_statId = dt.Rows[0]["StatusId"].ToString();
                    string user_activeId = dt.Rows[0]["ActiveStatusId"].ToString();
                    if(user_activeId == "1")
                    {
                        //active
                        switch (user_statId)
                        {
                            case "3":
                                //VERIFIED
                                Stats = "Error";
                                Mess = "Your account is under screening. Please contact admin to check the status.";
                                JWT = "";
                                break;
                            case "4":
                                //UNVERIFIED
                                Stats = "Error";
                                Mess = "Your account is unverified. Please contact your administrator.";
                                JWT = "";
                                break;
                            case "5":
                                //APPROVED
                                Stats = "Error";
                                Mess = "Your account is under screening or Your account has been disapproved . Please contact admin to check the status.";
                                JWT = "";
                                break;
                            case "6":
                                //REGISTERED
                                Stats = "Error";
                                Mess = "Succesfully Log In";
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
            StaturReturns strings = new StaturReturns
            {
                Status = "",
                Message = ""
            };
            return strings;
        }

    }
}

