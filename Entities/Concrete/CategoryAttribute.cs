using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class CategoryAttribute : IEntity
    {
        public int Id { get; set; }
        public int AttributeId { get; set; }
        public int AttributeValueId { get; set; }
        public bool Slicer { get; set; }
    }
}
