using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos.CategoryAttribute.Select;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface ICategoryAttributeDal : IEntityRepository<CategoryAttribute>
    {
        List<ViewCategoryAttributeDto> GetAllFilterDto(Expression<Func<ViewCategoryAttributeDto, bool>> filter = null);
        List<ViewCategoryAttributeDto> GetAllTrueSlicerAttribute(int categoryId);
    }
}
