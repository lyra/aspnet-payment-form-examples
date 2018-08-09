 //
 // Copyright (C) 2012 - 2018 Lyra Network.
 // This file is part of Lyra ASP.NET payment form sample.
 // See COPYING.md for license details.
 //
 // @author    Lyra Network <contact@lyra-network.com>
 // @copyright 2012 - 2018 Lyra Network
 // @license   http://www.gnu.org/licenses/gpl.html GNU General Public License (GPL v3)
 //

using System;
using System.Configuration;
using System.Web.Configuration;
using System.Threading;
using System.Globalization;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string lang = null;

        if (this.IsSupported(Request.QueryString["lang"]))
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

        Console.Out.WriteLine("language is : " + lang);

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
    }

    private bool IsSupported(string lang)
    {
        string[] supported = { "fr", "en" };
        return Array.Exists(supported, e => e == lang);
    }
}