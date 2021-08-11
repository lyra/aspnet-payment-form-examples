﻿ //
 // Copyright © Lyra Network.
 // This file is part of Lyra ASP.NET payment form example. See COPYING.md for license details.
 //
 // @author    Lyra Network <https://www.lyra.com>
 // @copyright Lyra Network
 // @license   http://www.gnu.org/licenses/gpl.html GNU General Public License (GPL v3)
 //

using System;
using System.Configuration;
using System.Web.Configuration;
using Lyranetwork.Lyra;

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
