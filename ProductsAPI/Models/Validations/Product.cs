using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductsAPI.Models
{
   
        [MetadataType(typeof(UserMetaData))]
        public partial class Product
        {
        }

        public class UserMetaData
        {
            [Required(ErrorMessage = "يجب ادخال هذا الحقل")]
            public string Name { get; set; }
            [Required(ErrorMessage = "يجب ادخال هذا الحقل")]
            [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
            public Nullable<int> UnitPrice { get; set; }
        }
    }
