using Core.Entities;
using System.Collections.Generic;

namespace Entities.Dtos.ProductCategory
{
    public class ProductCategoryDto : IDto
    {
        public int ProductCategoryId { get; set; }
        public int ProductId { get; set; }
        public int MainCategoryId { get; set; }
        public List<int> CategoryId { get; set; }
    }
}
