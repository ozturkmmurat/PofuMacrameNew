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
        List<SelectCategoryAttributeDto> GetAllSelectFilterDto(Expression<Func<SelectCategoryAttributeDto, bool>> filter = null);
        List<ViewCategoryAttributeDto> GetCategorySlicerAttribute(int categoryId);
        List<ViewCategoryAttributeDto> GetAllTrueSlicerAttribute(int categoryId);
    }
}
