using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.Category;
using Entities.Dtos.Category.Select;
using Entities.EntityParameter.Category;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<CategoryDto> GetRandomCategoriesWithFirstImage(FilterCategoryDto filter)
        {
            if (filter == null || filter.EndLength <= 0)
                return new List<CategoryDto>();

            var query = _context.Categories
                .AsNoTracking()
                .Where(c => _context.CategoryImages.Any(ci => ci.CategoryId == c.Id))
                .OrderBy(c => Guid.NewGuid())
                .Skip(filter.StartLength)
                .Take(filter.EndLength)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.Id,
                    CategoryName = c.CategoryName,
                    ParentId = c.ParentId,
                    ImagePath = _context.CategoryImages
                        .Where(ci => ci.CategoryId == c.Id)
                        .OrderBy(ci => ci.SequenceNumber)
                        .Select(ci => ci.Path)
                        .FirstOrDefault()
                });
            return query.ToList();
        }
    }
}
