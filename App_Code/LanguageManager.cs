using System;
using System.Configuration;
using System.Web.Configuration;
using System.Threading;
using System.Globalization;
using System.Web;
using System.Web.SessionState;

namespace Lyranetwork
{
    public class LanguageManager
    {
        private LanguageManager()
        {
            // Do not instanciate this class
        }

        public static void Initialize(HttpRequest Request)
        {
            // Current session object
            HttpSessionState Session = HttpContext.Current.Session;

            string lang = null;

            if (IsSupported(Request.QueryString["lang"]))
            {
                lang = Request.QueryString["lang"];
            }
            else if (Session["language"] != null)
            {
                lang = (string)Session["language"];
            }
            else
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                lang = config.AppSettings.Settings["default_language"].Value;
            }

            Session.Add("language", lang);

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
        }

        private static bool IsSupported(string lang)
        {
            string[] supported = { "fr", "en" };
            return Array.Exists(supported, e => e == lang);
        }
    }
}