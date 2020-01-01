using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductsAPI.Controllers
{
    public class GetCartsController : ApiController
    {
        ECommerceDBEntities db = new ECommerceDBEntities();
        public IHttpActionResult GetCarts()
        {
            var items = (from s in db.Cart
                         select new
                         {
                             s.id,
                             s.Productid,
                             s.amount,
                            


                         }).ToList();

            return Ok(new { items });
        }
    }
}
