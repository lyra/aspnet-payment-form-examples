using Lyranetwork;
using System;
using System.Configuration;
using System.Web.Configuration;

public partial class Checkout : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        LanguageManager.Initialize(Request);

        base.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        if ("12345678".Equals(config.AppSettings.Settings["shop_id"].Value))
        {
            ErrorMessage.Text = Resources.WebResources.ConfigErrorMessage;
            ErrorMessage.Visible = true;
        }
    }
}