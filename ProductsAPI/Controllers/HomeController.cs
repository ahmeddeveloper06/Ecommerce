using Newtonsoft.Json;
using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProductsAPI.Controllers
{
    public class HomeController : Controller
    {
        List<InvoiceItems> invoiceItemslist = new List<InvoiceItems>();
        ECommerceDBEntities db = new ECommerceDBEntities();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult AddProduct()
        {
            return View("index");
        }

        [HttpPost]
        public ActionResult AddProduct(Product pro, HttpPostedFileBase fileImage)
        {
            string fileName = null;
            if (fileImage != null)
            {
                string rootPath = Server.MapPath("~/ProductImages");
                string ext = Path.GetExtension(fileImage.FileName);
                fileName = DateTime.Now.ToString("dd-MM-yyyy-hhmmss") + ext;

                string newFile = Path.Combine(rootPath, fileName);

                fileImage.SaveAs(newFile);
            }
            try
            {
                pro.image = fileName;
                db.Product.Add(pro);
                db.SaveChanges();
                var cart = new Cart
                {
                    amount = 10,
                    Productid = 2,
                };


                TempData["msg"] = "s:تمت الاضافة بنجاح";
            }
            catch
            {
                TempData["msg"] = "e:Error validations check your inputs";
            }

            return View("index");
        }

        public ActionResult GetProducts()
        {
            return View();
        }

        public ActionResult GetProductsByApi()
        {
            IEnumerable<Product> products = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51173/api/");
                //HTTP GET
                var responseTask = client.GetAsync("GetProducts");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Product>>();
                    readTask.Wait();

                    products = readTask.Result;
                   
                }
                else //web api sent error response 
                {
                    //log response status here..

                    products = Enumerable.Empty<Product>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
           
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCart(int id, int? amount)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51173/api/AddToCart");
                Cart cart = new Cart();
                cart.Productid = id;
                cart.amount = amount;
                //HTTP POST
                var postTask = client.PostAsJsonAsync<Cart>("AddToCart", cart);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View("GetProducts");
        }

        public ActionResult Transaction(UserData userData)
        {
            InvoiceItems invoice = new InvoiceItems();
           foreach(var x in db.Cart.ToList())
            {
                invoice.ItemName = x.Product.Name;
                invoice.UnitPrice = x.Product.UnitPrice;
                invoice.Quantity = x.amount;
                invoiceItemslist.Add(invoice);
            }
            
            string jsonItems = JsonConvert.SerializeObject(invoiceItemslist);
            userData.invoiceItemslist = invoiceItemslist;
            string json1= JsonConvert.SerializeObject(userData);
            string json = "{\"PaymentMethodId\":\"2\",\"CustomerName\": \"Ahmed\",\"DisplayCurrencyIso\": \"KWD\",\"MobileCountryCode\":\"+965\",\"CustomerMobile\": \"92249038\",\"CustomerEmail\": \"aramadan@myfatoorah.com\",\"InvoiceValue\": 100,\"CallBackUrl\": \"https://google.com\",\"ErrorUrl\": \"https://google.com\",\"Language\": \"en\",\"CustomerReference\" :\"ref 1\",\"CustomerCivilId\":12345678,\"UserDefinedField\": \"Custom field\",\"ExpireDate\": \"\",\"CustomerAddress\" :{\"Block\":\"\",\"Street\":\"\",\"HouseBuildingNo\":\"\",\"Address\":\"\",\"AddressInstructions\":\"\"},\"InvoiceItems\": [{\"ItemName\": \"Product 01\",\"Quantity\": 1,\"UnitPrice\": 100}]}";

            //####### Initiate Payment ######

            string token = "7Fs7eBv21F5xAocdPvvJ-sCqEyNHq4cygJrQUFvFiWEexBUPs4AkeLQxH4pzsUrY3Rays7GVA6SojFCz2DMLXSJVqk8NG-plK-cZJetwWjgwLPub_9tQQohWLgJ0q2invJ5C5Imt2ket_-JAlBYLLcnqp_WmOfZkBEWuURsBVirpNQecvpedgeCx4VaFae4qWDI_uKRV1829KCBEH84u6LYUxh8W_BYqkzXJYt99OlHTXHegd91PLT-tawBwuIly46nwbAs5Nt7HFOozxkyPp8BW9URlQW1fE4R_40BXzEuVkzK3WAOdpR92IkV94K_rDZCPltGSvWXtqJbnCpUB6iUIn1V-Ki15FAwh_nsfSmt_NQZ3rQuvyQ9B3yLCQ1ZO_MGSYDYVO26dyXbElspKxQwuNRot9hi3FIbXylV3iN40-nCPH4YQzKjo5p_fuaKhvRh7H8oFjRXtPtLQQUIDxk-jMbOp7gXIsdz02DrCfQIihT4evZuWA6YShl6g8fnAqCy8qRBf_eLDnA9w-nBh4Bq53b1kdhnExz0CMyUjQ43UO3uhMkBomJTXbmfAAHP8dZZao6W8a34OktNQmPTbOHXrtxf6DS-oKOu3l79uX_ihbL8ELT40VjIW3MJeZ_-auCPOjpE3Ax4dzUkSDLCljitmzMagH2X8jN8-AYLl46KcfkBV"; //token value to be placed here;
            string baseURL = "https://apitest.myfatoorah.com";
            string url = baseURL + "/v2/InitiatePayment"; HttpClient client = new HttpClient();
            byte[] cred = Encoding.UTF8.GetBytes("Bearer " + token);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var parameters = new Dictionary<string, string> { { "InvoiceAmount", "100" }, { "CurrencyIso", "KWD" } };
            var encodedContent = new FormUrlEncodedContent(parameters);
            HttpResponseMessage messge = client.PostAsync(url, encodedContent).Result;
            if (messge.IsSuccessStatusCode)
            {
                string result = messge.Content.ReadAsStringAsync().Result;
               
            }
            else {
                string result = messge.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            //####### Execute Payment ######

            url = baseURL + "/v2/ExecutePayment";
            client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(json1, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            messge = client.PostAsync(url, content).Result;
            if (messge.IsSuccessStatusCode)
            {
                string result = messge.Content.ReadAsStringAsync().Result;
                Transactions ts = new Transactions();
                ts.InvoiceMessage = result;
                db.Transactions.Add(ts);
                db.SaveChanges();

                TempData["msg"] = "s:تمت العملية بنجاح";
            }
            else {
                string result = messge.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                TempData["msg"] = "e:Error validations check your inputs";
            }

            return View("GetProducts");
        }
    }
}
