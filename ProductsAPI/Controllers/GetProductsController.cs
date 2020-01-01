using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductsAPI.Controllers
{
    public class GetProductsController : ApiController
    {
        ECommerceDBEntities db = new ECommerceDBEntities();
        public IHttpActionResult GetProducts()
        {
            var items = (from s in db.Product
                            select new
                            {
                                s.id,
                                s.Name,
                                s.detaiel,
                                s.UnitPrice,


                            }).ToList();

            return Ok(items);
        }
    }
}
