using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class ProductPriceFactor : IEntity
    {
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public decimal ExtraPrice { get; set; }
        public bool Status { get; set; }
    }
}
