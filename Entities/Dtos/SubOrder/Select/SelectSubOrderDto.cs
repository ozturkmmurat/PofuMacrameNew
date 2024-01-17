using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.SubOrder.Select
{
    public class SelectSubOrderDto : IDto
    {
        public int SubOrderId { get; set; }
        public int VariantId { get; set; }
        public int ParentId { get; set; }
        public string ProductName { get; set; }
        public string Attribute { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public decimal Kdv { get; set; }
        public decimal NetPrice { get; set; }
        public int? SubOrderStatus { get; set; }
    }
}
