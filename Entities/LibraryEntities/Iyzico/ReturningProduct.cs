using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.LibraryEntities.Iyzico
{
    public class ReturningProduct
    {
        public int SubOrderId { get; set; }
        public int OrderId { get; set; }
        public string Description { get; set; }
        public string PaymentTransactionId { get; set; }
        public string PaidPrice { get; set; }
    }
}
