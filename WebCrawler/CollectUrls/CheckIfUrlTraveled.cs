//------------------------------------------------------------------
// <copyright file="CheckIfUrlTraveled.cs" company="Microsoft">
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
namespace WebCrawler.CollectUrls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebCrawler.Constants;

    public class CheckIfUrlTraveled
    {
        /// <summary>
        /// Check if a specified url is traveled by the Great Crawler
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns> 
        public static bool CheckIfSpecifiedUrlTraveled(string url)
        {
            foreach (Dictionary<string, int> dictionary in FieldStrings.urlAndParentList)
            {
                if (dictionary.Keys.Contains(url))
                {
                    return true;

                }
            }
            return false;
        }

        /// <summary>
        /// Check if a page is traveled, because there are different links point to the same page.
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static bool CheckIfSpecifiedPageTraved(string htmlString)
        {
            string content = null;
            if (content == htmlString)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}