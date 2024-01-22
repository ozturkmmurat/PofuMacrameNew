using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Category.Select
{
    public class SelectCategoryDto : IDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string CategoryName { get; set; }
        public List<SelectCategoryDto> SubCategories { get; set; }
    }
}
