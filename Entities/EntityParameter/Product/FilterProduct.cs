using Entities.EntityParameter.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EntitiyParameter.Product
{
    //Get islemklerinde eger degisken gerekiyor ise kullaniliyor.
    public class FilterProduct
    {
        public int CategoryId { get; set; }
        public int StartLength { get; set; }
        public int EndLength { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<FilterAttribute> Attributes { get; set; }
    }
}
