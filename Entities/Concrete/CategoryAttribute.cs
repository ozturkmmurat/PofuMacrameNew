using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class CategoryAttribute : IEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int AttributeId { get; set; }
        public int? VariableId { get; set; }
        public bool Slicer { get; set; }
        public bool Attribute { get; set; }
        public bool Required { get; set; }
    }
}
