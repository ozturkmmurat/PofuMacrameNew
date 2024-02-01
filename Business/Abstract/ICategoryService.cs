using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.Category.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<Category>> GetAll();
        IDataResult<List<Category>> GetAllAsNoTracking();
        IDataResult<List<Category>> GetAllSubCategory(int categoryId);
        IDataResult<List<SelectCategoryDto>> GetAllCategoryHierarchy();
        IDataResult<Category> GetById(int id);
        IResult Add(Category category);
        IResult Update(Category category);
        IResult Delete(Category category);
    }
}
