using System;
using System.Data;
using System.Data.SqlClient;

class SQLDB
    {
        //Declare Global SQL Connection
        public SqlConnection DBConnection;

        #region Database Functions
        /// <summary>
        /// BuildConnectionString - To build a connection string using integrated security (Windows Authentication)
        /// </summary>
        /// <param name="psServerName">string - the name of the SQL server instance to connect to</param>
        /// <param name="psDatabaseName">string - the name of the SQL database to connect to</param>
        /// <returns>string - the complete SQL database connection string</returns>
        public string BuildConnectionString(string psServerName, string psDatabaseName)
        {
            string sConnectionString = null;

            try
            {
                sConnectionString = "Server=" + psServerName + "; Database=" + psDatabaseName + "; Integrated Security=SSPI;";
                return sConnectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// BuildConnectionString - To build a connection string using SQL Authentication
        /// </summary>
        /// <param name="psServerName">string - the name of the SQL server instance to connect to</param>
        /// <param name="psDatabaseName">string - the name of the SQL database to connect to</param>
        /// <param name="psUserName">string - the SQL username to connect with</param>
        /// <param name="psPassword">string - the SQL user's password</param>
        /// <returns>string - the complete SQL database connection string</returns>
        public string BuildConnectionString(string psServerName, string psDatabaseName, string psUserName, string psPassword)
        {
            string sConnectionString = null;

            try
            {
                sConnectionString = "Server=" + psServerName + "; Database=" + psDatabaseName + "; User ID=" + psUserName + "; Password=" + psPassword.Trim();
                return sConnectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// OpenContextDBConnection - To open the SQL context connection passed from SQL Server
        /// when a CLR package is used for a stored procedure/trigger/etc
        /// </summary>
        /// <returns>SqlConnection - the opened SQL database connection</returns>
        public bool OpenContextDBConnection()
        {
            try
            {
                DBConnection = new SqlConnection("context connection=true");
                //DBConnection.ConnectionString = sConnectionString;
                DBConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// OpenDBConnection - To open a SQL database connection
        /// </summary>
        /// <param name="sConnectionString">string - the complete database connection string using either Windows or SQL Authentication</param>
        /// <returns>SqlConnection - the opened SQL database connection</returns>
        public bool OpenDBConnection(string sConnectionString)
        {
            try
            {
                DBConnection = new SqlConnection();
                DBConnection.ConnectionString = sConnectionString;
                DBConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// CloseDBConnection - To close a SQL database connection
        /// </summary>
        /// <returns>boolean</returns>
        public bool CloseDBConnection()
        {
            try
            {
                DBConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// OpenDataSet - To open a SQL dataset for the given command object
        /// </summary>
        /// <param name="adoCommand">SqlCommand - the SQL command object for which to open the dataset</param>
        /// <returns>DataSet - the opened SQL dataset</returns>
        public DataSet OpenDataSet(SqlCommand adoCommand)
        {
            DataSet adoRS = new DataSet();
            SqlDataAdapter adoDataAdapter = new SqlDataAdapter();
            
            try
            {
                if (DBConnection.State == ConnectionState.Open)
                {
                    adoDataAdapter.SelectCommand = adoCommand;
                    adoDataAdapter.Fill(adoRS);

                    if ((adoRS != null))
                    {
                        return adoRS;
                    }
                    else 
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Insert - to insert records into a SQL database as per the given command object
        /// </summary>
        /// <param name="adoCommand">SqlCommand - the SQL command object containing records to be inserted</param>
        /// <returns>int - the number of records inserted</returns>
        public int Insert(SqlCommand adoCommand)
        {
            int nRowsEffected = 0;

            try
            {
                nRowsEffected = adoCommand.ExecuteNonQuery();
                //if (nRowsEffected != 0)
                //{
                    return nRowsEffected;
                //}
            }
            catch (Exception ex)
            {
                // IMSMessage(modMessages.eMessageDesc.edException, "Insert", modMessages.eMessageType.etError, ex.Message)
                throw ex;
            }
        }
        /// <summary>
        /// Insert - to insert records into a SQL database as per the given command object
        /// </summary>
        /// <param name="adoCommand">SqlCommand - the SQL command object containing records to be inserted</param>
        /// <param name="sReturnParameter">string - name or value of parameter to be returned</param>
        /// <returns>int - the number of records inserted else the requested return parameter</returns>
        public int Insert(SqlCommand adoCommand, string sReturnParameter)
        {
            int nRowsEffected = 0;
            int nReturnID = 0;

            try
            {
                if (!string.IsNullOrEmpty(sReturnParameter))
                {
                    nRowsEffected = adoCommand.ExecuteNonQuery();

                    if (nRowsEffected != 0)
                    {
                        nReturnID = int.Parse(adoCommand.Parameters[sReturnParameter].Value.ToString());
                        return nReturnID;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    nRowsEffected = adoCommand.ExecuteNonQuery();
                    return nRowsEffected;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public string Insert(SqlCommand adoCommand, string sReturnParameter, SqlDbType nDBType)
        //{
        //    int nRowsEffected = 0;
        //    string sReturnID = null;

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(sReturnParameter))
        //        {
        //            nRowsEffected = adoCommand.ExecuteNonQuery();
        //            if (nRowsEffected != 0)
        //            {
        //                sReturnID = adoCommand.Parameters[sReturnParameter].Value.ToString();
        //                return sReturnID;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            nRowsEffected = adoCommand.ExecuteNonQuery();
        //            return nRowsEffected.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        /// <summary>
        /// Update - to update records in a SQL database as per the given command object
        /// </summary>
        /// <param name="adoCommand">SqlCommand - the SQL command object for records to be updated</param>
        /// <returns>int - the number of records updated</returns>
        public int Update(SqlCommand adoCommand)
        {
            int nRowsEffected = 0;

            try
            {
                nRowsEffected = adoCommand.ExecuteNonQuery();

                //if (nRowsEffected != 0)
                //{
                    return nRowsEffected;
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Delete - to delete records in a SQL database as per the given command object
        /// </summary>
        /// <param name="adoCommand">SqlCommand - the SQL command object for records to be deleted</param>
        /// <returns>int - the number of records deleted</returns>
        public int Delete(SqlCommand adoCommand)
        {

            int nRowsEffected = 0;

            try
            {
                nRowsEffected = adoCommand.ExecuteNonQuery();

                //if (nRowsEffected != 0)
                //{
                    return nRowsEffected;
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
