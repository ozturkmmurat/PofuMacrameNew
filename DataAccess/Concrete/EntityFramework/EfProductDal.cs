using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.Product.Select;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Entities.Dtos.Variant.Select;
using Microsoft.EntityFrameworkCore;
using Core.Utilities.Result.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, PofuMacrameContext>, IProductDal
    {
        public List<SelectProductDto> GetAllFilterDto()
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {

                var result = (from p in context.Products
                              join c in context.Categories
                              on p.CategoryId equals c.Id
                              join ca in context.CategoryAttributes
                              on c.Id equals ca.CategoryId
                              join pa in context.ProductAttributes
                              on p.Id equals pa.ProductId
                              join pai in context.ProductAttributeImages
                              on pa.Id equals pai.ProductAttributeId
                              join v in context.Variants
                              on p.Id equals v.ProductId
                              join ps in context.ProductStocks
                              on v.Id equals ps.VariantId
                              where ca.Slicer == true
                              select new SelectProductDto
                              {
                                  ProductId = p.Id,
                                  CategoryId = c.Id,
                                  VariantId = v.Id,
                                  ProductAttributeId = pa.Id,
                                  ProductStockId = ps.Id,
                                  ProductName = p.ProductName,
                                  Description = p.Description,
                                  Price = v.Price,
                                  StockCode = v.StockCode,
                                  Quantity = ps.Quantity,
                                  Paths = context.ProductAttributeImages
                                      .Where(x => x.ProductAttributeId == pa.Id)
                                      .Take(2)
                                      .Select(vi => vi.Path)
                                      .ToList()

                              }).AsEnumerable()
                                .GroupBy(x => new { x.ProductId, x.VariantId, x.ProductAttributeId })
              .Select(x => x.FirstOrDefault())
              .ToList();

                return result.ToList();
            }
        }
    }
}
