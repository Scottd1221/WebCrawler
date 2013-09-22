//
//
namespace WebCrawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using WebCrawler.Functions.Common.Constants;
    using WebCrawler.Functions.CollectUrls;

    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Click the Start Crawler, Crawler save a job to Db and then redirect to the JobStatus page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StartCrawlerClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxUrl.Text.ToString()) || string.IsNullOrWhiteSpace(TextBoxDepth.Text.ToString()))
            {
                // Show error, the input text is null
            }
            else
            {
                try
                {
                    ConfigurationStrings.Depth = Convert.ToInt32(TextBoxDepth.Text);
                    ConfigurationStrings.Url = TextBoxUrl.Text.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                    // Throw exception in Message box, tried to convert the depth, failed.
                }
                ConfigurationStrings.UrlAndParentList.Add(new Dictionary<string, int> { { ConfigurationStrings.Url, 0 } });

                //SaveJobToDB(ConfigurationStrings.Url, ConfigurationStrings.Depth);
                ProcessHtml.TravelUrlsInHtml(ConfigurationStrings.Depth);

                Response.Redirect("~/JobStatus.aspx", true);

                //FieldStrings.urlAndParentList.Add(new Dictionary<string, int> { { TextBoxUrl.Text.ToString(), 0 } });
                //TravleUrls.TravleUrlsInHtml(Convert.ToInt32(TextBoxDepth.Text));
            }
        }
    }
}