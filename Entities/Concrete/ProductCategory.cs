using Core.Entities;
using System;

namespace Entities.Concrete
{
    public class ProductCategory : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MainCategoryId { get; set; }
        public int CategoryId { get; set; }
    }
}
