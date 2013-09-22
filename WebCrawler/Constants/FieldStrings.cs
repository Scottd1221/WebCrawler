using System;
//------------------------------------------------------------------
// <copyright file="FieldStrings.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <author email="v-hayin@microsoft.com" />
//
// <summary>
// Main class
// 
// </summary>
//
// <remarks>
// </remarks>
//
// <disclaimer/>
//------------------------------------------------------------------
namespace WebCrawler.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FieldStrings
    {
        ///// <summary>
        ///// The list is used to hold the url strings
        ///// </summary>
        //public static List<string> urlList = new List<string>();

        /// <summary>
        /// The url counter used to count how many urls are there in the list
        /// </summary>
        public static int urlCounter = 1;

        /// <summary>
        /// The urAndParentList conatins the url and its parent
        /// </summary>
        public static List<Dictionary<string, int>> urlAndParentList = new List<Dictionary<string, int>>();
    }
}
