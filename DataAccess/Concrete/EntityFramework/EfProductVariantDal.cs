using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Entities.Dtos.ProductVariant.Select;
using Entities.Dtos.Product.Select;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductVariantDal : EfEntityRepositoryBase<ProductVariant, PofuMacrameContext>, IProductVariantDal
    {
        public List<SelectProductVariantDetailDto> GetAllFilterDto(Expression<Func<SelectProductVariantDetailDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from pv in context.ProductVariants
                             join p in context.Products on pv.ProductId equals p.Id
                             join av in context.AttributeValues on pv.AttributeValueId equals av.Id
                             join a in context.Attributes on av.AttributeId equals a.Id
                             select new SelectProductVariantDetailDto
                             {
                                 ProductVariantId = pv.Id,
                                 ProductId = p.Id,
                                 ParentId = pv.ParentId,
                                 ProductPaths = context.ProductImages
                                                      .Where(x => x.ProductVariantId == pv.Id)
                                                      .Take(2)
                                                      .Select(pi => pi.Path)
                                                      .ToList(),
                                 
                             };
                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }
    }
}
