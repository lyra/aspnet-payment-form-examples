# Lyra ASP.NET payment form example

## Introduction

The code presented here is an example of the implementation of the Lyra payment gateway form API in ASP.NET technology. It aims to ease its use and learning.

## Contents

* Checkout.aspx (and its .cs file): an HTML form to enter main order data.
* CheckoutConfirm.aspx (and its .cs file): an order preview page that will send data to the payment gateway.
* PaymentResult.aspx (and its .cs file): the page to be called by IPN callback and where the buyer will return at the end of payment.
* PaymentUtils.cs: a core file that contains the Lyra payment API logic (generate IDs, process signature, build redirect form).
* Web.config: application configuration file to set necessary parameters (at least, gateway credentials).
* Some other resources for styling pages and managing translations.

## First use

* Copy the content of this project to a directory at the root of your store on the web server.
* Set your Lyra API credentials in the Web.config file. You can also configure some other fields.
* Set the IPN URL to `www.your-site.com/PaymentResult.aspx` into your Lyra Back Office > Settings > Notification rules.
* Access `www.your-site.com/Checkout.aspx` from the browser.
* Follow the Lyra sample indications to perform the payment.

## Next steps

* You can customize checkout form (in Checkout.aspx) or plug your own form with data to be sent to the gateway (in CheckoutConfirm.aspx.cs).
* You should implement your own order management logic (update order status, manage stock, send confirmation e-mail, ...) in PaymentResult.aspx.cs.
* You should customize payment result messages and/or make redirection to your own result pages in PaymentResult.aspx.cs.
* Finally, you can change the ctx_mode parameter from `TEST` to `PRODUCTION` to switch to _real_ payment mode, with *all* the caution this decision expects.

## Note

* Please refer to the gateway documentation titled [Implementation guide - Interface with the payment gateway](https://payzen.io) to customize this sample code.

## License

Each source file included in this distribution is licensed under GNU GENERAL PUBLIC LICENSE (GPL 3.0). Please see LICENSE.txt for the full text of the GPL 3.0 license. It is also available through the world-wide-web at this URL: http://www.gnu.org/licenses/.