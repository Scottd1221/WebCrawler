//------------------------------------------------------------------
// <copyright file="FindUrls.cs" company="Microsoft">
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
    using System.Web;

    public class FindUrls
    {
        // Find a link in a content string.
        public static string FindLink(string htmlString, ref int startLocation)
        {
            int locationOfHttp;
            int linkStartLocation, linkEndLocation;

            string url = null;
            string lowCaseString = htmlString.ToLower();

            // Find the location of the href = http
            locationOfHttp = lowCaseString.IndexOf("href=\"http", startLocation);
            if (locationOfHttp != -1)
            {
                linkStartLocation = htmlString.IndexOf('"', locationOfHttp) + 1;
                linkEndLocation = htmlString.IndexOf('"', linkStartLocation);
                url = htmlString.Substring(linkStartLocation, (linkEndLocation - linkStartLocation));
                startLocation = linkEndLocation;
            }
            return url;
        }
    }
}