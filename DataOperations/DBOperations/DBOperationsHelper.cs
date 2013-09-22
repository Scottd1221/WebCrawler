//------------------------------------------------------------------
// <copyright file="DBOperationsHelper.cs" company="GreatWarrior">
//     Copyright (c) GreatWarrior Corporation.  All rights reserved.
// </copyright>
//
// <author email="bigyhm@live.com" />
//
// <summary>
// DBOperationsHelper
// </summary>
//
// <remarks>
// The DBOperationsHelper is intended to encapsulate high performance, scalable best practices for common use of SqlClient.
// </remarks>
//
// <disclaimer/>
//------------------------------------------------------------------

namespace WebCrawler.DataOperations.DBOperations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// The DBOperationsHelper is intended to encapsulate high performance, scalable best practices for common use of SqlClient.
    /// </summary>
    public class DBOperationsHelper
    {
        /// <summary>
        /// Hash table used to store the cached paramaters
        /// </summary>
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) aganist the database specified in the connection string using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">
        /// A valid connection string for SqlConnection
        /// </param>
        /// <param name="cmdType">
        /// The commandType (Stored procedure, text, etc.)
        /// </param>
        /// <param name="cmdText">
        /// The stored procedure name or T-SQL command
        /// </param>
        /// <param name="commandParameters">
        /// An array of SqlParameters used to execute the command
        /// </param>
        /// <returns>
        /// An int representing the number of rows affected by the command
        /// </returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand sqlCmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(sqlCmd, conn, null, cmdType, cmdText, cmdParms);
                int val = sqlCmd.ExecuteNonQuery();
                sqlCmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="sqlConn">
        /// An existing database connection
        /// </param>
        /// <param name="cmdType">
        /// The CommandType (stored procedure, text, etc.)
        /// </param>
        /// <param name="cmdText">
        /// The stored procedure name or T-SQL command
        /// </param>
        /// <param name="cmdParms">
        /// An array of SqlParameters used to execute the command
        /// </param>
        /// <returns>
        /// An int representing the number of rows affected by the command
        /// </returns>
        public static int ExecuteNonQuery(SqlConnection sqlConn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand sqlCmd = new SqlCommand();
            PrepareCommand(sqlCmd, sqlConn, null, cmdType, cmdText, cmdParms);

            int val = sqlCmd.ExecuteNonQuery();
            sqlCmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a Sqlcommand (that returns no resultset) using an existing SQL Transaction using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24))
        /// </remarks>
        /// <param name="sqlTrans">
        /// An existing sql transaction
        /// </param>
        /// <param name="cmdType">
        /// The CommandType(stored procedure, text, etc.)
        /// </param>
        /// <param name="cmdText">
        /// The stored procedure name of T-SQL command
        /// </param>
        /// <param name="cmdParms">
        /// An array of SqlParameters used to execute the command
        /// </param>
        /// <returns>
        /// An int representing the number of rows affected by the command
        /// </returns>
        public static int ExecuteNonQuery(SqlTransaction sqlTrans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand sqlCmd = new SqlCommand();
            PrepareCommand(sqlCmd, sqlTrans.Connection, sqlTrans, cmdType, cmdText, cmdParms);

            int val = sqlCmd.ExecuteNonQuery();
            sqlCmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        /// SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connString">
        /// A valid connection string for a SqlConnection
        /// </param>
        /// <param name="cmdType">
        /// The connection string for a SqlConnection
        /// </param>
        /// <param name="cmdText">
        /// The stored procedure name or T-SQL command
        /// </param>
        /// <param name="cmdParms">
        /// An array of the SqlParameters used to execute the command
        /// </param>
        /// <returns>
        /// A SqlDataReader containing the results
        /// </returns>
        public static SqlDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConn = new SqlConnection(connString);

            // We use a try/catch here because if the method throws an exception, we want to close the connection throw error.
            // Because there is no datareader will exist, hence the commandBehaviour.CloseConnection will not work.
            try
            {
                PrepareCommand(sqlCmd, sqlConn, null, cmdType, cmdText, cmdParms);
                SqlDataReader sqlDataReader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
                sqlCmd.Parameters.Clear();
                return sqlDataReader;
            }
            catch
            {
                sqlConn.Close();
                throw;
            }
        }

        /// <summary>
        /// Adds or refreshes rows in the System.Data.DataSet
        /// </summary>
        /// <param name="connString">
        /// A valid connection string for a SqlConnection
        /// </param>
        /// <param name="cmdType">
        /// The connection string for a SqlConnection
        /// </param>
        /// <param name="cmdText">
        /// The stored procedure name or T-SQL command
        /// </param>
        /// <param name="cmdParms">
        /// An array of the SqlParameters used to execute the command
        /// </param>
        /// <returns>
        /// A Dataset containing the results
        /// </returns>
        public static DataSet ExecuteDataSet(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            DataSet dataSet = new DataSet();
            SqlConnection sqlConn = new SqlConnection(connString);
            SqlCommand sqlCmd = new SqlCommand();

            try
            {
                PrepareCommand(sqlCmd, sqlConn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCmd);
                dataAdapter.Fill(dataSet);

                return dataSet;
            }
            catch
            {
                throw;
            }
            finally
            {
                sqlConn.Close();
            }
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the 
        /// result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="connString">
        /// A valid connection string for a SqlConnection
        /// </param>
        /// <param name="cmdType">
        /// The connection string for a SqlConnection
        /// </param>
        /// <param name="cmdText">
        /// The stored procedure name or T-SQL command
        /// </param>
        /// <param name="cmdParms">
        /// An array of the SqlParameters used to execute the command
        /// </param>
        /// <returns>
        /// Returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
        /// </returns>
        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand sqlCmd = new SqlCommand();

            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                PrepareCommand(sqlCmd, sqlConn, null, cmdType, cmdText, cmdParms);
                object val = sqlCmd.ExecuteScalar();
                sqlCmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the 
        /// result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="sqlConn">
        /// An existing database connection
        /// </param>
        /// <param name="cmdType">
        /// The CommandType (stored procedure, text, etc.)
        /// </param>
        /// <param name="cmdText">
        /// The stored procedure name or T-SQL command
        /// </param>
        /// <param name="cmdParms">
        /// An array of SqlParameters used to execute the command
        /// </param>
        /// <returns>
        /// Returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
        /// </returns>
        public static object ExecuteScalar(SqlConnection sqlConn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand sqlCmd = new SqlCommand();
            PrepareCommand(sqlCmd, sqlConn, null, cmdType, cmdText, cmdParms);
            object val = sqlCmd.ExecuteScalar();
            sqlCmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Add the parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">An array of SqlParameters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] cmdParms)
        {
            parmCache[cacheKey] = cmdParms;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">Key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCacheParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
            {
                return null; 
            }

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
            {
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();
            }

            return clonedParms;
        }

        /// <summary>
        /// Prepare a command for the execution
        /// </summary>
        /// <param name="sqlCmd">
        /// SqlCommand object
        /// </param>
        /// <param name="sqlConn">
        /// SqlConnection object
        /// </param>
        /// <param name="sqlTrans">
        /// SqlTransaction object
        /// </param>
        /// <param name="cmdType">
        /// Cmd type e.g. stored procedure or text
        /// </param>
        /// <param name="cmdText">
        /// Cmd text, e.g. Select * from Products
        /// </param>
        /// <param name="cmdParms">
        /// SqlParameters to use the command
        /// </param>
        private static void PrepareCommand(SqlCommand sqlCmd, SqlConnection sqlConn, SqlTransaction sqlTrans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }

            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandText = cmdText;

            if (sqlTrans != null)
            {
                sqlCmd.Transaction = sqlTrans;
            }

            sqlCmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter sqlParm in cmdParms)
                {
                    sqlCmd.Parameters.Add(sqlParm);
                }
            }
        }
    }
}