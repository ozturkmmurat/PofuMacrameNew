using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class ProductAttribute : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public int AttributeValueId { get; set; }
    }
}
