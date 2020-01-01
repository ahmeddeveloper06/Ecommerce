using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductsAPI
{
    public class UserData
    {
        public int PaymentMethodId = 2;
        public string CustomerName { get; set; }
        public string DisplayCurrencyIso = "KWD";
        public string MobileCountryCode { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public int InvoiceValue = 100;
        public string CallBackUrl= "https://google.com";
        public string ErrorUrl = "https://google.com";
        public string Language = "en";
        public string CustomerReference { get; set; }

        public Object invoiceItemslist { get; set; }

    }

    public class InvoiceItems
    {
       public string ItemName { get; set; }
        public int? Quantity { get; set; }
        public int? UnitPrice { get; set; }
    }
}