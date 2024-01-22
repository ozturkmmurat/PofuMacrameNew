using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos.Category.Select;
using Entities.Dtos.Product.Select;
using Entities.EntitiyParameter.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface ICategoryDal : IEntityRepository<Category>
    {
        List<SelectCategoryDto> GetAllCategoryHierarchy(List<Category> categories, int? parentId);
    }
}
