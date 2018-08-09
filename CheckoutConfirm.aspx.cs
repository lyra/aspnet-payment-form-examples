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
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;
using Lyranetwork.Lyra;
using System.Text;

public partial class CheckoutConfirm : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        LanguageManager.Initialize(Request);

        base.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            return;
        }

        if (Page.PreviousPage == null || !Page.PreviousPage.IsValid)
        {
            // No previous page, return to home page.
            Response.Redirect("~/Checkout.aspx", true);
            return;
        }

        // Retrieve data entered on Checkout.aspx page to display an order review.

        DropDownList list = (DropDownList)Page.PreviousPage.FindControl("ctl00$Content$Civility");
        if (list != null)
        {
            Civility.Text = list.SelectedItem.Value;
        }

        list = (DropDownList)Page.PreviousPage.FindControl("ctl00$Content$Country");
        if (list != null)
        {
            Country.Text = list.SelectedItem.Value;
        }

        TextBox textBox = (TextBox)Page.PreviousPage.FindControl("ctl00$Content$FirstName");
        if (textBox != null)
        {
            FirstName.Text = textBox.Text;
        }

        textBox = (TextBox)Page.PreviousPage.FindControl("ctl00$Content$LastName");
        if (textBox != null)
        {
            LastName.Text = textBox.Text;
        }

        textBox = (TextBox)Page.PreviousPage.FindControl("ctl00$Content$Address");
        if (textBox != null)
        {
            Address.Text = textBox.Text;
        }

        textBox = (TextBox)Page.PreviousPage.FindControl("ctl00$Content$ZipCode");
        if (textBox != null)
        {
            ZipCode.Text = textBox.Text;
        }

        textBox = (TextBox)Page.PreviousPage.FindControl("ctl00$Content$City");
        if (textBox != null)
        {
            City.Text = textBox.Text;
        }

        textBox = (TextBox)Page.PreviousPage.FindControl("ctl00$Content$Phone");
        if (textBox != null)
        {
            Phone.Text = textBox.Text;
        }

        textBox = (TextBox)Page.PreviousPage.FindControl("ctl00$Content$Email");
        if (textBox != null)
        {
            Email.Text = textBox.Text;
        }

        textBox = (TextBox)Page.PreviousPage.FindControl("ctl00$Content$OrderNumber");
        if (textBox != null)
        {
            OrderNumber.Text = textBox.Text;
        }

        textBox = (TextBox)Page.PreviousPage.FindControl("ctl00$Content$Amount");
        if (textBox != null)
        {
            Amount.Text = textBox.Text;
        }
    }

    protected void PayButton_Click(object sender, EventArgs e)
    {
        //
        // Configuration settings such as shop ID and certificates are stored in Web.config file. These settings will be available throught the config object below.
        //
        // Les paramètres de configuration tels que l'identifiant de la boutique et les certificats sont stockés dans le fichier Web.config. Ces paramètres seront accessibles au travers de l'objet config ci-dessous.
        //
        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

        //
        // Prepare form data to be posted to payment gateway :
        // - The use of SortedDictionary<key, value> allow ordering data alphabetically to compute signature.
        // - To add field, use the method SortedDictionary.Add().
        // - Keys and values must be strings.
        // - The data sort order is case insensitive.
        //
        // Préparation des données du formulaire à poster à la plateforme de paiement :
        // - L'utilisation d'un SortedDictionary<clé, valeur> permet d'organiser les données par ordre alphabétique en vue de calculer la signature.
        // - L'ajout d'un champ se fait avec la méthode SortedDictionary.Add().
        // - Les clés et les valeurs stockées doivent être de type string.
        // - Le tri par ordre alhpabétique ne tient pas compte de la casse.
        //

        var data = new SortedDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        //
        // Mandatory parameters.
        //

        data.Add("vads_site_id", config.AppSettings.Settings["shop_id"].Value); // Store identifier.
        
        string certificate = "PRODUCTION".Equals(config.AppSettings.Settings["ctx_mode"].Value) ? // Choose certificate.
            config.AppSettings.Settings["key_prod"].Value : 
            config.AppSettings.Settings["key_test"].Value;

        // Operating parameters.

        data.Add("vads_version", "V2"); // Payment form version. V2 is the only possible value.
        data.Add("vads_contrib", "ASP.NET_Form_Sample_v" + PaymentUtils.VERSION);
        data.Add("vads_ctx_mode", config.AppSettings.Settings["ctx_mode"].Value); // Context mode. 

        data.Add("vads_trans_date", PaymentUtils.GetTransDate()); // Generate UTC payment date in the format expected by the payment gateway : yyyyMMddHHmmss.
        data.Add("vads_page_action", "PAYMENT"); // This field define the action executed by the payment gateway. See gateway documentation for more information.
        data.Add("vads_action_mode", "INTERACTIVE"); // This allow to define the bank data acquisition mode : INTERACTIVE | SILENT.
                                                            
        // Payment information

        data.Add("vads_currency", "978"); //  Currency code in ISO-4217 standard.
        data.Add("vads_payment_config", "SINGLE"); // Payment type : SINGLE | MULTI | MULTI_EXT. For more information about advanced payment types, please see gateway documentation.

        // The amount to pay must be expressed in the smallest monetary unit (in cents for euro).
        var amount = decimal.Parse(Amount.Text.Replace(".", ",")); // Conversion to decimal and replacement of '.' by ','.
        amount = Convert.ToInt32(amount * 100); // Conversion to cents then to integer to remove the decimal part.
        data.Add("vads_amount", Convert.ToString(amount)); // Set amount as string.
       
        data.Add("vads_trans_id", PaymentUtils.GetTransId()); // Method generating transaction ID based on 1/10 of a second since midnight.

        //             
        // Optional parameters.
        //

        // Payment configuration.

        data.Add("vads_validation_mode", config.AppSettings.Settings["validation_mode"].Value);
        data.Add("vads_capture_delay", config.AppSettings.Settings["capture_delay"].Value);
        data.Add("vads_payment_cards", config.AppSettings.Settings["payment_cards"].Value);
        
        // Payment page customization.

        data.Add("vads_language", (string)Session["language"]);
        // data.Add("vads_available_languages", ""); 
        // data.Add("vads_shop_name", "");
        // data.Add("vads_shop_url", "");   
       
        // Return to shop

        data.Add("vads_return_mode", "GET"); // GET | POST.
        data.Add("vads_url_return", ReturnURL());
        // data.Add("vads_url_success", "");
        // data.Add("vads_url_refused", "");
        // data.Add("vads_url_cancel", "");
        // data.Add("vads_redirect_success_timeout", ""); // Time in seconds (0-300) before the buyer is automatically redirected to your website after a successful payment.
        // data.Add("vads_redirect_success_message", ""); // Message displayed on the payment page prior to redirection after a successful payment.
        // data.Add("vads_redirect_error_timeout", ""); // Time in seconds (0-300) before the buyer is automatically redirected to your website after a declined payment.
        // data.Add("vads_redirect_error_message", ""); // Message displayed on the payment page prior to redirection after a declined payment.

        // Information about customer.

        // data.Add("vads_cust_id", ""); 
        // data.Add("vads_cust_status", ""); // PRIVATE | COMPANY.
        if (!String.IsNullOrEmpty(Civility.Text)) { data.Add("vads_cust_title", Civility.Text); }
        // data.Add("vads_cust_name", "");
        if (!String.IsNullOrEmpty(FirstName.Text)) { data.Add("vads_cust_first_name", FirstName.Text); }
        if (!String.IsNullOrEmpty(LastName.Text)) { data.Add("vads_cust_last_name", LastName.Text); }
        if (!String.IsNullOrEmpty(Address.Text)) { data.Add("vads_cust_address", Address.Text); }
        // data.Add("vads_cust_number_address", ""); 
        // data.Add("vads_cust_district", "");
        if (!String.IsNullOrEmpty(ZipCode.Text)) { data.Add("vads_cust_zip", ZipCode.Text); }
        if (!String.IsNullOrEmpty(City.Text)) { data.Add("vads_cust_city", City.Text); }
        // data.Add("vads_cust_state", "");
        // data.Add("vads_cust_country", "");
        if (!String.IsNullOrEmpty(Phone.Text)) { data.Add("vads_cust_phone", Phone.Text); }
        // data.Add("vads_cust_cell_phone", "");
        if (!String.IsNullOrEmpty(Email.Text)) { data.Add("vads_cust_email", Email.Text); }

        // Delivery method

        // data.Add("vads_ship_to_status", ""); // PRIVATE | COMPANY.
        // data.Add("vads_ship_to_type", ""); // RECLAIM_IN_SHOP | RELAY_POINT | RECLAIM_IN_STATION | PACKAGE_DELIVERY_COMPANY | ETICKET.
        // data.Add("vads_ship_to_delivery_company_name", ""); // ex:UPS, La Poste, etc.
        // data.Add("vads_shipping_amount", "");
        // data.Add("vads_tax_amount", "");
        // data.Add("vads_insurance_amount", "");

        // Delivery address.

        // data.Add("vads_ship_to_name", "");
        // data.Add("vads_ship_to_first_name", "");
        // data.Add("vads_ship_to_last_name", "");
        // data.Add("vads_ship_to_address_number", "");
        // data.Add("vads_ship_to_street", "");
        // data.Add("vads_ship_to_street2", "");
        // data.Add("vads_ship_to_district", "");
        // data.Add("vads_ship_to_zip", "");
        // data.Add("vads_ship_to_city", "");
        // data.Add("vads_ship_to_state", "");
        // data.Add("vads_ship_to_country", "");
        // data.Add("vads_ship_to_phone_num", "");

        // Order information.

        if (!String.IsNullOrEmpty(OrderNumber.Text)) { data.Add("vads_order_id", OrderNumber.Text); }  // Retrieve order information entered in previous page.
        // data.Add("vads_order_info", "");
        // data.Add("vads_order_info2", "");
        // data.Add("vads_order_info3", "");                
        
        // Compute signature.
        data.Add("signature", PaymentUtils.GetSignature(data, certificate));

        // Build payment form and redirect to payment gateway.
        string payForm = PaymentUtils.GetPaymentForm(config.AppSettings.Settings["gateway_url"].Value, data);
        CheckoutConfirmForm.Parent.Controls.Add(new LiteralControl(payForm));
    }

    private string ReturnURL()
    {
        StringBuilder url = new StringBuilder();
        url.Append(Request.Url.Scheme + "://");
        url.Append(Request.Url.Authority);

        string path = "/PaymentResult.aspx";
        if (!String.IsNullOrEmpty(Request.ApplicationPath) && !"/".Equals(Request.ApplicationPath))
        {
            path = Request.ApplicationPath + path;
        }

        path = path.Replace("//", "/");
        url.Append(path);

        return url.ToString();
    }
}