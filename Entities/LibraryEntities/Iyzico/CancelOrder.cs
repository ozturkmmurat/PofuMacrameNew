using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.LibraryEntities.Iyzico
{
    public class CancelOrder
    {
        public int OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Description { get; set; }
    }
}
