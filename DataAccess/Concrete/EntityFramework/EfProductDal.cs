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

        public List<SelectListProductVariantDto> GetAllPvFilterDto()
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from p in context.Products
                              join pv in context.ProductVariants.Where(x => x.ParentId == 0) on p.Id equals pv.ProductId
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
                              });


                return result.ToList();
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
                                ProductId = p .Id,
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
