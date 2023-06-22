using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
