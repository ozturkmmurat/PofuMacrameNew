using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.Category;
using Entities.Dtos.Category.Select;
using Entities.EntityParameter.Category;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<Category>> GetAllAsNoTracking();
        IDataResult<List<Category>> GetAllSubCategory(int categoryId);
        IDataResult<List<SelectCategoryDto>> GetAllCategoryHierarchy();
        IDataResult<List<CategoryDto>> GetAllRandomCategory(FilterCategoryDto filter);
        IDataResult<Category> GetById(int id);
        IResult Add(Category category);
        IResult Update(Category category);
        IResult Delete(Category category);
    }
}
