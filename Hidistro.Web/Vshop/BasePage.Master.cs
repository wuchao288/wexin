using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.Web.Vshop
{
    public partial class BasePage : System.Web.UI.MasterPage
    {
        private const string titleKey = "Hishop.Title.Value";

        public static void AddSiteNameTitle(string title)
        {
            AddTitle(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", new object[] { title, SettingsManager.GetMasterSettings(true).SiteName }));
        }

        public static void AddSiteNameTitle()
        {
            AddTitle(string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { SettingsManager.GetMasterSettings(true).SiteName }));
        }

        public static void AddTitle(string title)
        {
            if (HttpContext.Current == null)
            {
                throw new ArgumentNullException("context");
            }

            
            HttpContext.Current.Items["Hishop.Title.Value"] = title;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}