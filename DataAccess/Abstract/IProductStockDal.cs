using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos.ProductStock;
using Entities.Dtos.ProductVariant.Select;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IProductStockDal : IEntityRepository<ProductStock>
    {
        List<SelectProductStockDto> GetByDto(int productId);

    }
}
