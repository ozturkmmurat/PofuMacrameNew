using Core.Entities;
using Entities.Dtos.SubOrder.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Order.Select
{
    public class SelectUserOrderDto : IDto
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public List<SelectSubOrderDto> SelectSubOrderDtos { get; set; } 
    }
}
