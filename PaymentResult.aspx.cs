using System;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Specialized;
using Lyranetwork;
using System.Web.UI.WebControls;

public partial class PaymentResult : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        LanguageManager.Initialize(Request);

        base.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

        string certificate = "PRODUCTION".Equals(config.AppSettings.Settings["ctx_mode"].Value) ? // Choose certificate.
            config.AppSettings.Settings["key_prod"].Value :
            config.AppSettings.Settings["key_test"].Value;

        // If vads_hash is present, then the request came from IPN call.
        if (Request.Form.Get("vads_hash") != null)
        {
            //
            // The order processing will be done here. The return to shop code part should be used only for displaying the payment result to the buyer.
            // 
            // Le traitement de la commande se fera ici. La partie du code pour le retour à la boutique ne doit servir qu'à l'affichage du résultat pour l'acheteur.
            //
            
            // Check signature consistency.
            if (CheckAuthenticity(Request.Form, certificate))
            {
                // Use order ID to find the order to update.
                string orderId = Request.Params.Get("vads_order_id");

                // The signature is valid, we can continue processing.

                PaymentStatus status = LyraApi.GetPaymentStatus(Request.Params.Get("vads_trans_status"));
                if (PaymentStatus.ACCEPTED.Equals(status))
                {
                    // Payment accepted. 
                    // Insert your logic here to register order, manage stock, empty shopping cart, send e-mail confirmation and more.
                    Response.Write("OK-Accepted payment, order has been updated.");
                }
                else if (PaymentStatus.PENDING.Equals(status))
                {
                    // Payment is pending.
                    // Insert your logic here to make order in a pending status, empty shopping cart and send notification about order processing.
                    Response.Write("OK-Accepted payment, order has been updated.");
                }
                else
                {
                    // Payment cancelled by the buyer or payment failed. 
                    // Insert your logic here to cancel order if already created.
                    Response.Write("OK-Payment failure, order has been cancelled.");
                }
            }
            else
            {
                //
                // The computed signature is not the same as the received one. Potential risk of fraud. The received data should not be used to process order.
                //
                // La signature calculée ne correpond pas à la signature reçue. Risque potentiel de fraude. Les données reçues ne doivent pas être prises en compte pour le traitement de la commande.
                //

                // Specific error management here.
                Response.Write("KO-An error occurred while computing the signature.");
            }
        }
        else if (Request.Params.Get("vads_site_id") != null)
        {
            //
            // Return to shop case: We use Request.Params to get received parameters (either return mode is GET or POST).
            // Note : the siganture validity check is not mandatory on the return to shop because order processing is not done here.
            //
            // Cas du retour à la boutique: Nous utilisons Request.Params pour récupérer les paramètres reçus (si le mode de retour est GET ou POST).
            // Remarque: Il n'est pas obligatoire de calculer la signature sur le retour boutique, le traitement de la commande n'étant pas réalisé ici.
            //

            // In test mode, display response parameters.
            if ("TEST".Equals(Request.Params.Get("vads_ctx_mode")))
            {
                int i = 1;
                foreach (string key in Request.Params.AllKeys)
                {
                    if (key == null)
                    {
                        continue;
                    }

                    if (!key.StartsWith("vads_"))
                    {
                        continue;
                    }

                    Label field = (Label)this.FindControl("ctl00$Content$ResultData" + i);
                    field.Text += "[" + key + "=" + Request.Params.Get(key) + "]<br />";

                    if (i == 3)
                    {
                        i = 1;
                    } else
                    {
                        i++;
                    }
                }

                Signature.Text = "[signature=" + Request.Params.Get("signature") + "]";

                // In this example, we add this line to display the unhashed signature string in TEST mode.
                UnhashedSignature.Text = LyraApi.GetSignature(Request.Params, certificate, false);
            }

            // Check signature consistency.
            if (CheckAuthenticity(Request.Params, certificate))
            {
                //
                // Here we check payment result to know which message to show to the buyer.
                // Ici nous vérifions le résultat du paiement pour déterminer le message à afficher à l'acheteur.
                //

                PaymentStatus status = LyraApi.GetPaymentStatus(Request.Params.Get("vads_trans_status"));
                if (PaymentStatus.ACCEPTED.Equals(status))
                {
                    // Payment accepted. Insert your code here.
                    ResultMsg.Text = Resources.WebResources.SuccessMsg;
                }
                else if (PaymentStatus.PENDING.Equals(status))
                {
                    // Payment is pending. Insert your code here.
                    ResultMsg.Text = Resources.WebResources.PendingMsg;
                }
                else if (PaymentStatus.CANCELLED.Equals(status))
                {
                    // Payment cancelled by buyer. Insert your code here.
                    ResultMsg.Text = Resources.WebResources.CancelMsg;
                }
                else
                {
                    // Payment failed. Insert your code here.
                    ResultMsg.Text = Resources.WebResources.FailureMsg;
                }
            }
            else
            {
                // Signature error. Manage error here.
                ResultMsg.Text = Resources.WebResources.FatalErrorMsg;
            }
        }
        else
        {
            //
            // Client return without parameters (vads_return_mode = NONE). Insert your processing code here.
            //
            // Retour navigateur sans paramètres (vads_return_mode = NONE). Inserez votre code de traitement ici.
            //
        }
    }

    /// <summary>
    /// Check received signature validity.
    /// 
    /// Vérification de la signature reçue.
    /// </summary>
    /// <param name="values">Received data.</param>
    /// <param name="certificate">The secret key.</param>
    /// <returns>True if received signature is the same as computed one.</returns>

    private bool CheckAuthenticity(NameValueCollection values, string certificate)
    {
        // Compute the signature.
        string computedSign = LyraApi.GetSignature(values, certificate);

        // Check signature consistency.
        return String.Equals(values.Get("signature"), computedSign, System.StringComparison.InvariantCultureIgnoreCase);
    }
}