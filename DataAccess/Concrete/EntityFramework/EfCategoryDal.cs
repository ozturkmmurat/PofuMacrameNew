using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.Category.Select;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category, PofuMacrameContext>, ICategoryDal
    {
        private readonly PofuMacrameContext _context;
        public EfCategoryDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }

        public List<SelectCategoryDto> GetAllCategoryHierarchy(List<Category> categories, int? parentId)
        {
            return categories
            .Where(c => c.ParentId == parentId)
            .Select(c => new SelectCategoryDto
            {
                Id = c.Id,
                ParentId = c.ParentId,
                CategoryName = c.CategoryName,
                SubCategories = GetAllCategoryHierarchy(categories, c.Id)
            })
            .ToList();
        }
    }
}
