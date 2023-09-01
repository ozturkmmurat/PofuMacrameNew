using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Product.Select
{
    public class SelectProductDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
    }
}
