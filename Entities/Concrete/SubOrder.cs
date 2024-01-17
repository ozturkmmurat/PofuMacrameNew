using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class SubOrder : IEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int VariantId { get; set; }
        public decimal Price { get; set; }
        public decimal Kdv { get; set; }
        public decimal NetPrice { get; set; }
        public string ReturnResultJson { get; set; }
        public int? SubOrderStatus { get; set; }
    }
}
