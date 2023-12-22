using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.SubOrder.Select
{
    public class SelectOrderedProducts
    {
        public int OrderId { get; set; }
        public int VariantId { get; set; }
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderCode { get; set; }
        public string ProductName { get; set; }
        public string Attribute { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int? SubOrderStatus { get; set; }
    }
}
