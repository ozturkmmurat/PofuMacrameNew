using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.CategoryAttribute;
using Entities.Dtos.CategoryAttribute.Select;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCategoryAttributeDal : EfEntityRepositoryBase<CategoryAttribute, PofuMacrameContext>, ICategoryAttributeDal
    {
        private readonly PofuMacrameContext _context;
        public EfCategoryAttributeDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
        public List<ViewCategoryAttributeDto> GetCategorySlicerAttribute(int categoryId)
        {
            var result = from ca in _context.CategoryAttributes.Where(x => x.CategoryId == categoryId && (x.Attribute == true || x.Slicer == true))
                         join c in _context.Categories.Where(x => x.Id == categoryId)
                         on ca.CategoryId equals c.Id
                         join a in _context.Attributes
                         on ca.AttributeId equals a.Id

                         select new ViewCategoryAttributeDto
                         {
                             CategoryId = ca.CategoryId,
                             AttributeId = a.Id,
                             AttributeName = a.Name,
                             AttributeValues = _context.AttributeValues.Where(x => x.AttributeId == a.Id).ToList(),
                             Slicer = ca.Slicer,
                             Attribute = ca.Attribute
                         };


            return result.ToList();
        }

        public List<SelectCategoryAttributeDto> GetAllSelectFilterDto(Expression<Func<SelectCategoryAttributeDto, bool>> filter = null)
        {
            var result = from ca in _context.CategoryAttributes
                         join c in _context.Categories
                         on ca.CategoryId equals c.Id
                         join a in _context.Attributes
                         on ca.AttributeId equals a.Id

                         select new SelectCategoryAttributeDto
                         {
                             CategoryAttributeId = ca.Id,
                             CategoryId = c.Id,
                             AttributeId = a.Id,
                             VariableId = ca.VariableId,
                             Attribute = ca.Attribute,
                             Slicer = ca.Slicer,
                             Required = ca.Required,
                         };
            return filter == null ? result.ToList() : result.Where(filter).ToList();
        }

        public List<ViewCategoryAttributeDto> GetAllTrueSlicerAttribute(int categoryId)
        {
            var result = from ca in _context.CategoryAttributes
                         join c in _context.Categories
                         on ca.CategoryId equals c.Id
                         join a in _context.Attributes
                         on ca.AttributeId equals a.Id

                         select new ViewCategoryAttributeDto
                         {
                             CategoryId = ca.CategoryId,
                             AttributeId = a.Id,
                             AttributeName = a.Name,
                             Slicer = ca.Slicer,
                             Attribute = ca.Attribute
                         };
            return result.ToList();
        }

    }
}
