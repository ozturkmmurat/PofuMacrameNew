using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos.CategoryAttribute.Select;
using Entities.Dtos.Product;
using Entities.Dtos.Product.Select;
using Entities.EntitiyParameter.Product;
using Entities.EntityParameter.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<Product>
    {
        int GetTotalProduct(int categoryId);
        List<SelectListProductVariantDto> GetAllPvFilterDto(FilterProduct filterProduct);
        List<SelectProductDto> GetAllFilterDto(Expression<Func<SelectProductDto, bool>> filter = null);
        SelectProductDto GetProductFilterDto(Expression<Func<SelectProductDto, bool>> filter = null);

        SelectProductDetailDto GetFilterDto(Expression<Func<SelectProductDetailDto, bool>> filter = null); 
    }
}
