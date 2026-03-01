using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EntityParameter.Iyzico
{
    public class PaymentResultPostParameter
    {
        /// <summary>
        /// Ödeme callback güvenliği için Order.Guid (callback URL'den gelir). Sipariş yalnızca bu değerle bulunur.
        /// </summary>
        public string Guid { get; set; }
    }
}
