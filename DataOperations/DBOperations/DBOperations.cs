//------------------------------------------------------------------
// <copyright file="DBOperations.cs" company="GreatWarrior">
//     Copyright (c) GreatWarrior Corporation.  All rights reserved.
// </copyright>
//
// <author email="bigyhm@live.com" />
//
// <summary>
// DBOperations
// </summary>
//
// <remarks>
// 
// </remarks>
//
// <disclaimer/>
//------------------------------------------------------------------

namespace WebCrawler.DataOperations.DBOperations
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Microsoft.SqlServer.Management.Smo;
    using Microsoft.SqlServer.Management.Common;
    using System.Data;
    using WebCrawler.DataOperations.Common.Constants;

    public class DBOperations
    {
        /// <summary>
        /// Add the url, html and the depth of the web crawler into the DB
        /// </summary>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="url">The url of the html</param>
        /// <param name="html">The html string</param>
        /// <param name="depth">The depth of the url in the web server</param>
        public static void AddHtml(string connectionString, string url, string html, int depth)
        {
            SqlParameter[] sqlParms = new SqlParameter[5];

            sqlParms[0] = new SqlParameter(@"ID", SqlDbType.UniqueIdentifier);
            sqlParms[0].Value = Guid.NewGuid();

            sqlParms[1] = new SqlParameter(@"Url", SqlDbType.VarChar, 50);
            sqlParms[1].Value = url;

            sqlParms[2] = new SqlParameter(@"Html", SqlDbType.VarChar);
            sqlParms[2].Value = html;

            sqlParms[3] = new SqlParameter(@"Keywords", SqlDbType.VarChar, 50);
            sqlParms[3].Value = string.Empty;

            sqlParms[4] = new SqlParameter(@"Depth", SqlDbType.Int);
            sqlParms[4].Value = depth;

            DBOperationsHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, CommandText.AddHtml, sqlParms);

        }

    }
}