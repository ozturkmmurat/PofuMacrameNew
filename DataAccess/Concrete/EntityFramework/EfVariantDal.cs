using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using Entities.Dtos.Variant.Select;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfVariantDal : EfEntityRepositoryBase<Variant, PofuMacrameContext>, IVariantDal
    {
        public List<ViewVariantDto> GetAllFilterDto(Expression<Func<ViewVariantDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from v in context.Variants
                              join p in context.Products
                              on v.ProductId equals p.Id
                              join ps in context.ProductStocks
                              on v.Id equals ps.VariantId
                              join pa in context.ProductAttributes
                              on p.Id equals pa.ProductId
                              join pai in context.ProductAttributeImages
                              on pa.Id equals pai.ProductAttributeId
                              select new ViewVariantDto
                              {
                                  VariantId = v.Id,
                                  ProductId = p.Id,
                                  ProductName = p.ProductName,
                                  Description = p.Description,
                                  StockCode = v.StockCode,
                                  Price = v.Price,
                                  ProductAttributeId = pa.Id,
                                  Paths = context.ProductAttributeImages
                                      .Where(x => x.ProductAttributeId == pa.Id)
                                      .Take(2)
                                      .Select(vi => vi.Path)
                                      .ToList()
                              }).AsEnumerable()
                                .GroupBy(x => new { x.ProductAttributeId })
                                .Select(x => x.FirstOrDefault())
                                .ToList();

                return result;
            }
        }
    }
}
