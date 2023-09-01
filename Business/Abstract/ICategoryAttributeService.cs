using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.CategoryAttribute.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICategoryAttributeService
    {
        IDataResult<List<CategoryAttribute>> GetAll();
        IDataResult<List<CategoryAttribute>> GetAllByCategoryId(int categoryId);
        IDataResult<CategoryAttribute> GetByAttributeIdCategoryId(int attributeId, int categoryId);
        IDataResult<List<ViewCategoryAttributeDto>> GetAllDtoTrueSlicer(int categoryId);
        IDataResult<List<ViewCategoryAttributeDto>> GetAllViewDtoTrueSlicerAttribute(int categoryId);
        IResult Add(CategoryAttribute categoryAttribute);
        IResult Update(CategoryAttribute categoryAttribute);
        IResult Delete(CategoryAttribute categoryAttribute);
    }
}
