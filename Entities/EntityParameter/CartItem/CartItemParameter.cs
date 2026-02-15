using Entities.Dtos.ProductVariant.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EntityParameter.CartItem
{
    public class CartItemParameter
    {
        public ProductVariantAttributeValueDto product { get; set; }
        public int Quantity { get; set; }
    }
}
