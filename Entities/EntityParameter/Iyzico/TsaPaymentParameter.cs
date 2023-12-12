using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Entities.EntityParameter.CartItem;

namespace Entities.EntityParameter.Iyzico
{
    public class TsaPaymentParameter : IDto
    {
        public int AddressId { get; set; }
        public string TcNo { get; set; }
        public List<CartItemParameter> CartItems { get; set; }
    }
}
