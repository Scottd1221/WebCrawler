using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCrawler.Functions.Common.Constants
{
    public class ConfigurationStrings
    {
        ///// <summary>
        ///// The list is used to hold the url strings
        ///// </summary>
        //public static List<string> urlList = new List<string>();

        /// <summary>
        /// The url counter used to count how many urls are there in the list
        /// </summary>
        public static int UrlCounter = 1;

        /// <summary>
        /// The urlAndParentList conatins the url and its parent
        /// </summary>
        public static List<Dictionary<string, int>> UrlAndParentList = new List<Dictionary<string, int>>();

        /// <summary>
        /// The depth of the web going to dig
        /// </summary>
        public static int Depth = 0;

        /// <summary>
        /// The url administrator input
        /// </summary>
        public static string Url = string.Empty;
    }
}