using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos.Category;
using Entities.Dtos.Category.Select;
using Entities.EntityParameter.Category;
using Entities.EntitiyParameter.Product;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface ICategoryDal : IEntityRepository<Category>
    {
        List<SelectCategoryDto> GetAllCategoryHierarchy(List<Category> categories, int? parentId);
        List<CategoryDto> GetRandomCategoriesWithFirstImage(FilterCategoryDto filter);
    }
}
