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

        public List<SelectListProductVariantDto> GetAllPvFilterDto(Expression<Func<SelectListProductVariantDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from p in context.Products
                              join pv in context.ProductVariants on p.Id equals pv.ProductId
                              join pi in context.ProductImages on pv.Id equals pi.ProductVariantId into piJoin
                              from pi in piJoin.DefaultIfEmpty()
                              join av in context.AttributeValues on pv.AttributeValueId equals av.Id
                              join ps in context.ProductStocks on p.Id equals ps.ProductId
                              select new SelectListProductVariantDto
                              {
                                  ProductId = p.Id,
                                  ProductVariantId = pv.Id,
                                  ParentId = pv.ParentId,
                                  AttributeValueId = av.Id,
                                  ProductName = p.ProductName,
                                  Description = p.Description,
                                  AttributeValue = av.Value,
                                  StockCode = ps.StockCode,
                                  Price = ps.Price,
                                  Quantity = ps.Quantity,
                                  ProductPaths = context.ProductImages
                                                      .Where(x => x.ProductVariantId == pv.Id)
                                                      .Take(2)
                                                      .Select(pi => pi.Path)
                                                      .ToList(),
                                  VariantPaths = context.ProductImages
                                                      .Where(x => x.ProductId == pv.ProductId && x.IsMain == true)
                                                      .Select(x => x.Path)
                                                      .ToList()
                              }).ToList();
                    

                return filter == null ? result.ToList() : result.Where(filter.Compile()).ToList();
            }
        }

        public SelectListProductVariantDto GetFilterDto(Expression<Func<SelectListProductVariantDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from p in context.Products
                              join pv in context.ProductVariants on p.Id equals pv.ProductId
                              join pi in context.ProductImages on pv.Id equals pi.ProductVariantId
                              join ps in context.ProductStocks on p.Id equals ps.ProductId
                              join av in context.AttributeValues on pv.AttributeValueId equals av.Id
                              select new SelectListProductVariantDto
                              {
                                  ProductId = p.Id,
                                  ProductVariantId = pv.Id,
                                  ParentId = pv.ParentId,
                                  AttributeValueId = av.Id,
                                  ProductName = p.ProductName,
                                  Description = p.Description,
                                  AttributeValue = av.Value,
                                  StockCode = ps.StockCode,
                                  Price = ps.Price,
                                  Quantity = ps.Quantity,
                                  ProductPaths = context.ProductImages.Select(x => x.Path).ToList()
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
                                CategoryName = c.CategoryName
                              });

                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }
    }
}
