using GaragesManagement.Models;
using GaragesManagement.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GaragesManagement.Database
{
    public class AccountDAO
    {
        private static readonly Lazy<AccountDAO> _instance = new Lazy<AccountDAO>(() => new AccountDAO());

        public static AccountDAO Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private string connectionString = Config.GaragesConnectionString;

        public Account Login(string username, string password, out int responseStatus)
        {
            DBHelper db = null;
            try
            {
                db = new DBHelper(connectionString);
                List<SqlParameter> parsList = new List<SqlParameter>();
                parsList.Add(new SqlParameter("@_Username", username));
                parsList.Add(new SqlParameter("@_Password", password));

                SqlParameter response = new SqlParameter("@_ResponseStatus", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                parsList.Add(response);

                Account acc = db.GetInstanceSP<Account>("Login", parsList.ToArray());
                responseStatus = Convert.ToInt32(response.Value);
                return acc;

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (db != null)
                {
                    db.Close();
                }
            }

            responseStatus = -99;
            return null;
        }
    }
}