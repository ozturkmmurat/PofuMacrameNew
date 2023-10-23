using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EntitiyParameter.Product
{
    //Get islemklerinde eger degisken gerekiyor ise kullaniliyor.
    public class FilterProduct
    {
        public int CategoryId { get; set; }
        public List<int> Attributes { get; set; }
    }
}
