using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Order
{
    public class OrderDto : IDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderCode { get; set; }
        public List<Entities.Concrete.SubOrder> SubOrders { get; set; }
    }
}
