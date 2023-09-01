﻿using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos.CategoryAttribute.Select;
using Entities.Dtos.Product;
using Entities.Dtos.Product.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<Product>
    {
        List<SelectListProductVariantDto> GetAllPvFilterDto(Expression<Func<SelectListProductVariantDto, bool>> filter = null);
        List<SelectProductDto> GetAllFilterDto(Expression<Func<SelectProductDto, bool>> filter = null);
        SelectListProductVariantDto GetFilterDto(Expression<Func<SelectListProductVariantDto, bool>> filter = null); 
    }
}
