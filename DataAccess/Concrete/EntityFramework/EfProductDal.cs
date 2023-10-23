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
using Microsoft.EntityFrameworkCore;
using Core.Utilities.Result.Concrete;
using System.IO;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, PofuMacrameContext>, IProductDal
    {

        public List<SelectListProductVariantDto> GetAllPvFilterDto(int categoryId, List<int> attributeValueIdList)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                if (categoryId == 0)
                {
                    var result = from p in context.Products
                                 join pv in context.ProductVariants on p.Id equals pv.ProductId
                                 where pv.ParentId == 0
                                 select new SelectListProductVariantDto
                                 {
                                     ProductId = p.Id,
                                     ProductVariantId = pv.Id,
                                     ParentId = pv.ParentId,
                                     ProductName = p.ProductName,
                                     Description = p.Description,
                                     AttributeValues = context.AttributeValues.Where(x => x.Id == pv.AttributeValueId).ToList(),
                                     ProductPaths = context.ProductImages
                                                         .Where(x => x.ProductVariantId == pv.Id)
                                                         .Take(2)
                                                         .Select(pi => pi.Path)
                                                         .ToList(),
                                 };
                    return result.ToList();

                }
                else if (categoryId > 0 && attributeValueIdList.Count == 0)
                {
                    var result = from p in context.Products
                                 join pv in context.ProductVariants on p.Id equals pv.ProductId
                                 where (p.CategoryId == categoryId)  &&
                                       pv.ParentId == 0
                                 select new SelectListProductVariantDto
                                 {
                                     ProductId = p.Id,
                                     ProductVariantId = pv.Id,
                                     ParentId = pv.ParentId,
                                     ProductName = p.ProductName,
                                     Description = p.Description,
                                     AttributeValues = context.AttributeValues.Where(x => x.Id == pv.AttributeValueId).ToList(),
                                     ProductPaths = context.ProductImages
                                                         .Where(x => x.ProductVariantId == pv.Id)
                                                         .Take(2)
                                                         .Select(pi => pi.Path)
                                                         .ToList(),
                                 };
                    return result.ToList();

                }
                else if (categoryId > 0 && attributeValueIdList.Count > 0)
                {
                    var products = context.Products
    .Where(x => x.CategoryId == categoryId)
    .ToList();

                    var productIds = products.Select(p => p.Id).ToList();

                    var groupedAttributes = context.ProductAttributes
                        .Where(variant => productIds.Contains(variant.ProductId) &&
                                          attributeValueIdList.Contains(variant.AttributeValueId))
                        .AsEnumerable()  // groupedVariants'ı belleğe çek
                        .GroupBy(variant => variant.ProductId)
                        .Where(group => attributeValueIdList.All(attributeId => group.Any(g => g.AttributeValueId == attributeId)))
                        .Select(group => group.FirstOrDefault())
                        .ToList();

                    var filteredProducts = products
                    .Where(p => groupedAttributes.Any(gv => gv?.ProductId == p.Id))
                    .ToList();

                    var result = from p in filteredProducts
                                 join pv in context.ProductVariants
                                 on p.Id equals pv?.ProductId
                                 select new SelectListProductVariantDto
                                 {
                                     ProductId = p.Id,
                                     ProductVariantId = pv?.Id ?? 0,
                                     ParentId = pv.ParentId ?? 0,
                                     ProductName = p.ProductName,
                                     Description = p.Description,
                                     AttributeValues = pv != null ? context.AttributeValues.Where(x => x.Id == pv.AttributeValueId).ToList() : new List<AttributeValue>(),
                                     ProductPaths = pv != null ? context.ProductImages
                                                                    .Where(x => x.ProductVariantId == pv.Id)
                                                                    .Take(2)
                                                                    .Select(pi => pi.Path)
                                                                    .ToList() : new List<string>()
                                 };

                    return result.ToList();
                }
                return null;
            }
        }

        public SelectProductDetailDto GetFilterDto(Expression<Func<SelectProductDetailDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from p in context.Products
                              join pv in context.ProductVariants on p.Id equals pv.ProductId
                              join ps in context.ProductStocks on p.Id equals ps.ProductId
                              join pi in context.ProductImages on pv.Id equals pi.ProductVariantId
                              where pi.IsMain == true
                              select new SelectProductDetailDto
                              {
                                  ProductId = p.Id,
                                  ProductVariantId = pv.Id,
                                  ParentId = pv.ParentId,
                                  ProductName = p.ProductName,
                                  Description = p.Description,
                                  StockCode = ps.StockCode,
                                  Price = ps.Price,
                                  Quantity = ps.Quantity,
                                  MainImage = pi.Path,
                                  ProductPaths = context.ProductImages
                                                        .Where(x => x.ProductVariantId == pv.Id)
                                                        .Select(x => x.Path).ToList(),
                                  VariantPaths = context.ProductImages
                                                        .Where(x => x.ProductId == p.Id && x.IsMain == true)
                                                        .Select(x => x.Path).ToList()
                              });


                return filter == null ? result.FirstOrDefault() : result.Where(filter.Compile()).FirstOrDefault();
            }
        }

        public List<SelectProductDto> GetAllFilterDto(Expression<Func<SelectProductDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from p in context.Products
                              join c in context.Categories on p.CategoryId equals c.Id

                              select new SelectProductDto
                              {
                                  ProductId = p.Id,
                                  CategoryId = c.Id,
                                  ProductName = p.ProductName,
                                  CategoryName = c.CategoryName,
                                  ProductCode = p.ProductCode
                              });

                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }

        public SelectProductDto GetProductFilterDto(Expression<Func<SelectProductDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from p in context.Products
                              join c in context.Categories on p.CategoryId equals c.Id

                              select new SelectProductDto
                              {
                                  ProductId = p.Id,
                                  CategoryId = c.Id,
                                  ProductName = p.ProductName,
                                  CategoryName = c.CategoryName,
                                  ProductCode = p.ProductCode,
                                  Description = p.Description,
                              });

                return filter == null ? result.FirstOrDefault() : result.Where(filter).FirstOrDefault();
            }
        }
    }
}
