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
using Entities.EntitiyParameter.Product;
using Entities.EntityParameter.Product;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, PofuMacrameContext>, IProductDal
    {

        public List<SelectListProductVariantDto> GetAllPvFilterDto(FilterProduct filterProduct)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                if (filterProduct.CategoryId == 0)
                {
                    var result = from p in context.Products.Skip(filterProduct.StartLength).Take(filterProduct.EndLength)
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
                else if (filterProduct.CategoryId > 0 && filterProduct.Attributes.Count == 0)
                {
                    var result = from p in context.Products.Where(x => x.CategoryId == filterProduct.CategoryId).Skip(filterProduct.StartLength).Take(filterProduct.EndLength)
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
                else if (filterProduct.CategoryId > 0 && filterProduct.Attributes.Count > 0)
                {
                    var products = context.Products
                                    .Where(x => x.CategoryId == filterProduct.CategoryId)
                                    .ToList();

                    var productIds = products.Select(p => p.Id).ToList();

                    var productAttributes = context.ProductAttributes
                        .Where(variant => productIds.Contains(variant.ProductId))
                        .ToList();

                    var filteredProducts = products.Where(product =>
                    {
                        var productAttributesForProduct = productAttributes.Where(pa => pa.ProductId == product.Id).ToList();

                        return filterProduct.Attributes.All(filterAttribute =>
                        {
                            var matchingProductAttributes = productAttributesForProduct.Where(pa => pa.AttributeId == filterAttribute.Id).ToList();

                            return matchingProductAttributes.Any(pa => filterAttribute.ValueId.Contains(pa.AttributeValueId));
                        });
                    }).ToList();

                    var categoryAttributes = context.CategoryAttributes.Where(x => x.CategoryId == filterProduct.CategoryId && x.Slicer == true);
                    if (categoryAttributes != null)
                    {
                        if (categoryAttributes.Any()) // Count() yerine Any() kullanmak daha verimlidir
                        {
                            var attributeIds = filterProduct.Attributes.Where(x => categoryAttributes.Any(a => a.AttributeId == x.Id)).Select(x => x.Id).ToList();

                            // Ardından bu AttributeId'leri kullanarak ValueId'leri bul
                            var valueIds = filterProduct.Attributes.Where(x => attributeIds.Contains(x.Id)).SelectMany(x => x.ValueId).ToList();

                            // Son olarak, bu ValueId'leri kullanarak ProductVariants içerisindeki eşleşmeleri bul
                            var slicerProduct = context.ProductVariants.Where(x => x.AttributeValueId.HasValue && valueIds.Contains(x.AttributeValueId.Value)).ToList();
                            if (slicerProduct != null)
                            {
                                var mainProductResult = from p in filteredProducts
                                                        join spv in slicerProduct
                                                        on p.Id equals spv.ProductId
                                                        select new SelectListProductVariantDto
                                                        {
                                                            ProductId = p.Id,
                                                            ProductVariantId = spv?.Id ?? 0,
                                                            ParentId = spv.ParentId ?? 0,
                                                            ProductName = p.ProductName,
                                                            Description = p.Description,
                                                            AttributeValues = spv != null ? context.AttributeValues.Where(x => x.Id == spv.AttributeValueId).ToList() : new List<AttributeValue>(),
                                                            ProductPaths = spv != null ? context.ProductImages
                                                                                                 .Where(x => x.ProductVariantId == spv.Id)
                                                                                                 .Take(2)
                                                                                                 .Select(pi => pi.Path)
                                                                                                 .ToList() : new List<string>()
                                                        };

                                return mainProductResult.ToList();
                            }
                        }

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

        public int GetTotalProduct(int categoryId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                if (categoryId != 0)
                {
                    var result = context.Products.Where(x => x.CategoryId == categoryId).Count();
                    return result;
                }
                else
                {
                    var result = context.Products.Count();
                    return result;
                }
            }
        }
    }
}
