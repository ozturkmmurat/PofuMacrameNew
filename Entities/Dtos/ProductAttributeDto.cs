using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class ProductAttributeDto : IDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string AttributeName { get; set; }
    }
}
