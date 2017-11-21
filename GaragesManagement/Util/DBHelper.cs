using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GaragesManagement.Util
{
    public class DBHelper
    {
        private string _cnnString = "";

        public string cnnString
        {
            get
            {
                return _cnnString;
            }
            set
            {
                _cnnString = value;
            }
        }

        public DBHelper()
        {
          
        }

        public DBHelper(string connStr)
        {
            _cnnString = connStr;
        }

        private SqlConnection _ConnectionToDB;

        public SqlConnection ConnectionToDB
        {
            get
            {
                return _ConnectionToDB;
            }
            set
            {
                _ConnectionToDB = value;
            }
        }

        public void Open()
        {
            if (_cnnString == "")
            {
                throw new Exception("Connection String can not null");
            }
            _ConnectionToDB = OpenConnection();
        }

        public void Close()
        {
            CloseConnection(_ConnectionToDB);
        }

        public SqlConnection OpenConnection(string connectionString)
        {
            try
            {
                _cnnString = connectionString;
                return OpenConnection();
            }
            catch (SqlException myException)
            {
                throw myException;
            }
        }

        public SqlConnection OpenConnection()
        {
            if (_cnnString == "")
            {
                throw new Exception("Connection String can not null");
            }

            SqlConnection mySqlConnection;

            try
            {
                mySqlConnection = new SqlConnection(_cnnString);
                mySqlConnection.Open();
                return mySqlConnection;
            }
            catch (Exception)
            {
                mySqlConnection = new SqlConnection(_cnnString);
                mySqlConnection.Open();
                return mySqlConnection;                
            }
        }

        public void CloseConnection(SqlConnection mySqlConnection)
        {
            try
            {
                if (mySqlConnection != null)
                {
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                }
            }
            catch (SqlException myException)
            {
                throw myException;// (new Exception(myException.Message));
            }
        }

        #region ExecuteNonQuery
        
        public int ExecuteNonQuery(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return ExecuteNonQuery(sqlCommand);
        }

        #endregion

        #region ExecuteNonQuerySP

        public int ExecuteNonQuerySP(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return ExecuteNonQuery(sqlCommand);
        }

        public int ExecuteNonQuerySP(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return ExecuteNonQuery(sqlCommand, Parameters);
        }

        #endregion

        #region[GetInstance]

        public T GetInstance<T>(SqlCommand sqlCommand)
        {
            try
            {
                T temp = default(T);

                sqlCommand.Connection = _ConnectionToDB ?? OpenConnection();
                var dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    var fCount = dr.FieldCount;
                    var m_Type = typeof(T);
                    var l_Property = m_Type.GetProperties();
                    object obj;
                    var m_List = new List<T>();
                    string pName;

                    obj = Activator.CreateInstance(m_Type);
                    for (var i = 0; i < fCount; i++)
                    {
                        pName = dr.GetName(i);
                        if (l_Property.Where(a => a.Name == pName).Select(a => a.Name).Count() <= 0)
                        {
                            continue;
                        }
                        if (dr[i] != DBNull.Value)
                        {
                            m_Type.GetProperty(pName).SetValue(obj, dr[i], null);
                        }
                        else
                        {
                            m_Type.GetProperty(pName).SetValue(obj, null, null);
                        }
                    }
                    dr.Close();
                    return (T)obj;
                }
                else
                {
                    return temp;
                }
            }
            catch (SqlException myException)
            {
                throw myException; //(new Exception(myException.Message));
            }
            finally
            {
                CloseConnection(sqlCommand.Connection);
            }
        }

        public T GetInstance<T>(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return GetInstance<T>(sqlCommand);
        }

        public T GetInstance<T>(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return GetInstance<T>(sqlCommand);
        }

        public T GetInstance<T>(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            sqlCommand.Parameters.AddRange(Parameters);
            return GetInstance<T>(sqlCommand);
        }

        #endregion

        #region[GetInstanceSP]

        public T GetInstanceSP<T>(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetInstance<T>(sqlCommand);
        }

        public T GetInstanceSP<T>(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetInstance<T>(sqlCommand, Parameters);
        }

        #endregion
    }
}