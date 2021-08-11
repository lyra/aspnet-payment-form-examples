//
// Copyright © Lyra Network.
// This file is part of Lyra ASP.NET payment form example. See COPYING.md for license details.
//
// @author    Lyra Network <https://www.lyra.com>
// @copyright Lyra Network
// @license   http://www.gnu.org/licenses/gpl.html GNU General Public License (GPL v3)
//

using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Web.UI;

namespace Lyranetwork.Lyra
{
    public class PaymentUtils
    {
        public static readonly string VERSION = "1.1.0";
        public static readonly string SIGN_ALGO = "HMAC-SHA-256";

        private PaymentUtils()
        {
            // Do not instanciate this class.
        }

        /// <summary>
        /// Return current UTC date in format yyyyMMddHHmmss.
        ///
        /// Renvoi la date courante en UTC au format yyyyMMddHHmmss.
        /// </summary>
        /// <returns>A formatted transaction Date.</returns>
        public static String GetTransDate()
        {
            return DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Generate a unique transaction number in the day.
        ///
        /// Génère un numéro de transaction unique dans la journée.
        /// </summary>
        /// <returns>A generated transaction ID.</returns>
        public static string GetTransId()
        {
            DateTime date = DateTime.UtcNow; // Current UTC date time.
            double diff = date.TimeOfDay.TotalMilliseconds / 100; // Number of 1/10 of a second since midnight.
            return String.Format("{0:000000}", diff); // Convert to a string of 6 digits.
        }

        private static string Hash(string text, string key, Encoding encoding)
        {
            switch (SIGN_ALGO)
            {
                case "HMAC-SHA-256":
                    return HashHMACSHA256(text, key, encoding);
                case "SHA-1":
                    return HashSHA1(text, encoding);
                default:
                    throw new ArgumentException("Unsupported algorithm : " + SIGN_ALGO);
            }
        }

        /// <summary>
        /// Hash signature string using SHA1 algorithm.
        ///
        /// Hashage du texte de la signature en utilisant l'algorithme SHA1.
        /// </summary>
        /// <param name="text">Text to hash.</param>
        /// <param name="encoding">Original text encoding.</param>
        /// <returns>Hash of text using SHA1 algo.</returns>
        private static string HashSHA1(string text, Encoding encoding)
        {
            byte[] textBuffer = encoding.GetBytes(text);

            SHA1CryptoServiceProvider serviceProvider = new SHA1CryptoServiceProvider();
            return BitConverter.ToString(serviceProvider.ComputeHash(textBuffer)).Replace("-", "");
        }

        /// <summary>
        /// Hash signature string using HMAC-SHA256 algorithm.
        ///
        /// Hashage du texte de la signature en utilisant l'algorithme HMAC-SHA256.
        /// </summary>
        /// <param name="text">Text to hash.</param>
        /// <param name="key">Key to use by HMAC algorithm.</param>
        /// <param name="encoding">Original text encoding.</param>
        /// <returns>Hash of text using HMAC-SHA256 algo.</returns>
        private static string HashHMACSHA256(string text, string key, Encoding encoding)
        {
            byte[] textBuffer = encoding.GetBytes(text);
            byte[] keyBuffer = encoding.GetBytes(key);

            HMACSHA256 serviceProvider = new HMACSHA256(keyBuffer);
            return Convert.ToBase64String(serviceProvider.ComputeHash(textBuffer));
        }

        /// <summary>
        /// Compute signature.
        ///  - The use of SortedDictionary&lt;key, value&gt; allow sorting data alphabetically.
        ///  - To add field, use the method SortedDictionary.Add().
        ///  - Keys and values must be strings.
        ///  - The data sort order is case insensitive.
        ///
        /// Calcul de la signature.
        ///  - L'utilisation d'un SortedDictionary&lt;clé, valeur&gt; permet d'organiser les données par ordre alphabétique.
        ///  - L'ajout d'un champ se fait avec la méthode SortedDictionary.Add().
        ///  - Les clés et les valeurs stockées doivent être de type string.
        ///  - Le tri par ordre alphabétique ne tient pas compte de la casse.
        /// </summary>
        /// <param name="parameters">A list of (key, value) entries.</param>
        /// <param name="shakey">The secret key.</param>
        /// <returns>The computed signature</returns>
        public static string GetSignature(NameValueCollection parameters, string shakey)
        {
            return GetSignature(parameters, shakey, true);
        }

        public static string GetSignature(NameValueCollection parameters, string shakey, bool hashed)
        {
            var data = new SortedDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (string key in parameters.AllKeys)
            {
                // Check parameter name and save it only if it starts with "vads_".
                if ((key != null) && key.StartsWith("vads_"))
                {
                    data.Add(key, parameters.Get(key));
                }
            }

            return GetSignature(data, shakey, hashed);
        }

        public static string GetSignature(SortedDictionary<string, string> parameters, string shakey)
        {
            return GetSignature(parameters, shakey, true);
        }

        public static string GetSignature(SortedDictionary<string, string> parameters, string shakey, bool hashed)
        {
            // The sign var contains unhashed string. Display it if you have signature problems.
            string sign = "";
            foreach (var parameter in parameters)
            {
                sign += parameter.Value + "+";
            }

            if (!hashed)
            {
                return sign + "SECRETSHOPKEY";
            } else
            {
                sign += shakey;
                System.Diagnostics.Debug.WriteLine("|" + sign + "|");
                return Hash(sign, shakey, Encoding.UTF8);
            }
        }

        public static PaymentStatus GetPaymentStatus(string transStatus)
        {
            PaymentStatus status;
            switch (transStatus)
            {
                case "AUTHORISED":
                case "CAPTURED":
                case "CAPTURE_FAILED": // Capture will be redone.
                    status = PaymentStatus.ACCEPTED;
                    break;

                case "AUTHORISED_TO_VALIDATE":
                case "WAITING_AUTHORISATION_TO_VALIDATE":
                case "UNDER_VERIFICATION":
                case "INITIAL":
                    status = PaymentStatus.PENDING;
                    break;

                case "ABANDONED":
                    status = PaymentStatus.CANCELLED;
                    break;

                default:
                    status = PaymentStatus.FAILED;
                    break;
            }

            return status;
        }

        public static string GetPaymentForm(string url, SortedDictionary<string, string> parameters)
        {
            // Set a name for the form.
            string formId = "LyraPaymentForm";

            // Build the form using the specified data to be posted.
            StringBuilder formBuilder = new StringBuilder();

            formBuilder.Append("\n<form id=\"" + formId + "\" name=\"" + formId + "\" action=\"" + url + "\" method=\"POST\">");

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                formBuilder.Append("\n<input type=\"hidden\" name=\"" + parameter.Key + "\" value=\"" + HttpContext.Current.Server.HtmlEncode(parameter.Value) + "\">");
            }

            formBuilder.Append("\n</form>");
            formBuilder.Append("\n\n");

            // Build the JavaScript that will auto submit form.
            formBuilder.Append("\n<script type=\"text/javascript\">");
            formBuilder.Append("\n    var payForm = document." + formId + ";");
            formBuilder.Append("\n    payForm.submit();");
            formBuilder.Append("\n</script>");

            // Return the payment form code as string.
            return formBuilder.ToString();
        }
    }
}
