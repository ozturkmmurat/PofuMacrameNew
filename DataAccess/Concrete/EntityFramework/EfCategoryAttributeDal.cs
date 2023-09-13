using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.CategoryAttribute.Select;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCategoryAttributeDal : EfEntityRepositoryBase<CategoryAttribute, PofuMacrameContext>, ICategoryAttributeDal
    {
        public List<ViewCategoryAttributeDto> GetAllFilterDto(Expression<Func<ViewCategoryAttributeDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from ca in context.CategoryAttributes
                             join c in context.Categories
                             on ca.CategoryId equals c.Id
                             join a in context.Attributes
                             on ca.AttributeId equals a.Id
                             join av in context.AttributeValues
                             on a.Id equals av.AttributeId

                             select new ViewCategoryAttributeDto
                             {
                                 CategoryId = ca.CategoryId,
                                 AttributeId = a.Id,
                                 AttributeName = a.Name,
                                 AttributeValues = context.AttributeValues.Where(x => x.AttributeId == a.Id).ToList(),
                                 Slicer = ca.Slicer,
                                 Attribute = ca.Attribute
                             };

             
                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }

        public List<SelectCategoryAttributeDto> GetAllSelectFilterDto(Expression<Func<SelectCategoryAttributeDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from ca in context.CategoryAttributes
                             join c in context.Categories
                             on ca.CategoryId equals c.Id
                             join a in context.Attributes
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
        }

        public List<ViewCategoryAttributeDto> GetAllTrueSlicerAttribute(int categoryId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from ca in context.CategoryAttributes
                             join c in context.Categories
                             on ca.CategoryId equals c.Id
                             join a in context.Attributes
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
}
