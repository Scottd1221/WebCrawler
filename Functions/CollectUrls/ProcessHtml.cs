//------------------------------------------------------------------
// <copyright file="CollectUrls.cs" company="GreatWarrior">
//     Copyright (c) GreatWarrior Corporation.  All rights reserved.
// </copyright>
//
// <author email="bigyhm@live.com" />
//
// <summary>
// CollectUrls
// </summary>
//
// <remarks>
// 
// </remarks>
//
// <disclaimer/>
//------------------------------------------------------------------

namespace WebCrawler.Functions.CollectUrls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using WebCrawler.Functions.Common.Constants;
    using WebCrawler.DataOperations.DBOperations;

    public class ProcessHtml
    {
        /// <summary>
        /// Travel urls in the html.
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static string TravelUrlsInHtml(int depth)
        {
            // Holds the url link found by the FindUrls.FindLink(htmlString, ref curLocation);
            string link = null;

            // Holds the html string of an url
            string htmlString = null;

            // Holds the current location in response
            int curLocationOfHtml = 0;

            // Holds the location of the url list
            int curLocation = 0;

            // Holds the depth of the crawler
            int depthCrawled = 0;

            // Holds the depth of the url
            int depthUrl = 0;

            // Holds the response the the web request
            HttpWebResponse response = null;

            do
            {
                try
                {
                    // Travel the urls
                    if (ConfigurationStrings.UrlAndParentList[depthCrawled].Count > curLocation)
                    {
                        Console.WriteLine("Linking to " + ConfigurationStrings.UrlAndParentList[depthCrawled].Keys.ToArray()[curLocation]);

                        // Create the WebRequest to the specified URL
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationStrings.UrlAndParentList[depthCrawled].Keys.ToArray()[curLocation]);

                        // Send the request and return the response
                        response = (HttpWebResponse)request.GetResponse();

                        // From the response, obtain an input stream
                        Stream instream = response.GetResponseStream();

                        // Wrap the input stream in a StreamReader
                        StreamReader streamReader = new StreamReader(instream);

                        // Read the entire page
                        htmlString = streamReader.ReadToEnd();

                        // Set the cur location of html to be zero
                        curLocationOfHtml = 0;
                    }

                    // Set the location of the cur
                    if (depthCrawled < depth - 1 && curLocation == 0)
                    {
                        ConfigurationStrings.UrlAndParentList.Add(new Dictionary<string, int>());
                        depthUrl++;
                    }
                    if (ConfigurationStrings.UrlAndParentList[depthCrawled].Count > curLocation + 1)
                    {
                        curLocation++;
                    }
                    else
                    {
                        curLocation = 0;
                        depthCrawled++;
                    }
                    // Collect the urls in the html
                    while (depthCrawled < depth - 1)
                    {
                        // Find the next URL to link to
                        link = FindLink(htmlString, ref curLocationOfHtml);

                        if (link != null && !CheckIfSpecifiedUrlTraveled(link) && !CheckIfSpecifiedPageTraved(htmlString))
                        {
                            // Add the link to the more deepper list
                            ConfigurationStrings.UrlAndParentList[depthUrl].Add(link, depthCrawled);
                            ConfigurationStrings.UrlCounter++;

                            // Add the url and the html to DB
                            DBOperations.AddHtml(DBStrings.SQLServerConnectionStrings, link, htmlString, depthCrawled);
                        }
                        else if (link != null && link.Length > 0)
                        {
                            if (CheckIfSpecifiedUrlTraveled(link))
                            {
                                Console.WriteLine(string.Format("{0}: The link traveled.", link));
                            }
                            else if (CheckIfSpecifiedPageTraved(htmlString))
                            {
                                Console.WriteLine(string.Format("{0}: The page traveled", link));
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    response.Close();
                }
                catch (WebException exc)
                {
                    Console.WriteLine("Network Error: " + exc.Message + "\nStatus code: " + exc.Status);
                    curLocation++;
                }
                catch (ProtocolViolationException exc)
                {
                    Console.WriteLine("Protocol Error: " + exc.Message);
                    curLocation++;
                }
                catch (UriFormatException exc)
                {
                    Console.WriteLine("URI Format Error: " + exc.Message);
                }
                catch (NotSupportedException exc)
                {
                    Console.WriteLine("Unknown Protocol: " + exc.Message);
                    curLocation++;
                }
                catch (IOException exc)
                {
                    Console.WriteLine("I/O Error: " + exc.Message);
                    curLocation++;
                }
            }
            while (depthCrawled < depth);
            return null;
        }

        /// <summary>
        /// Find the link in the html string
        /// </summary>
        /// <param name="htmlString">The html string</param>
        /// <param name="startLocation">The start location of an url in the html</param>
        /// <returns>The url got</returns>
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

        /// <summary>
        /// Check if a specified url is traveled by the Great Crawler
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns> 
        public static bool CheckIfSpecifiedUrlTraveled(string url)
        {
            foreach (Dictionary<string, int> dictionary in ConfigurationStrings.UrlAndParentList)
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