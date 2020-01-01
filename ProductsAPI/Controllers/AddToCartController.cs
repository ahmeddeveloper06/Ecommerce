using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductsAPI.Controllers
{
    public class AddToCartController : ApiController
    {
        ECommerceDBEntities db = new ECommerceDBEntities();
        [HttpPost]
        public IHttpActionResult AddToCart(Cart cart)
        {
           

            db.Cart.Add(cart);
            db.SaveChanges();
            return Ok();
        }

    }
}
