using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EntityParameter.Product
{
    public class TotalFilterProduct
    {
        public int CategoryId { get; set; }
        public List<int> Attributes { get; set; }
    }
}
