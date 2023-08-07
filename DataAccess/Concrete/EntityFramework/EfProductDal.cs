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
using System.IO;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, PofuMacrameContext>, IProductDal
    {
        public List<SelectListProductDto> GetAllFilterDto()
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                
                var result = (from p in context.Products
                               join pv in context.ProductVariants
                               on p.Id equals pv.ProductId
                               join pi in context.ProductImages
                               on pv.Id equals pi.ProductVariantId
                               join ps in context.ProductStocks
                               on pv.ProductId equals ps.ProductId
                               let productImages = context.ProductImages
                                                         .Where(x => x.ProductVariantId == pv.Id)
                               select new SelectListProductDto
                               {
                                   ProductId = p.Id,
                                   ProductVariantId = pv.Id,
                                   ParentId = pv.ParentId,
                                   ProductName = p.ProductName,
                                   Description = p.Description,
                                   IsVariant = p.IsVariant,
                                   PvStockCode = pv.StockCode,
                                   Price = ps.Price,
                                   ProductPaths = productImages.Take(2).Select(pi => pi.Path).ToList(),
                                   VariantPaths = productImages.GroupBy(x => x.ProductVariantId).Select(x => x.FirstOrDefault().Path).AsEnumerable().ToList()
                               }).AsEnumerable()
                               .GroupBy(x => x.ProductId) // Ürün bazlı gruplama yap
                .Select(group => group.First()) // Her üründen sadece birini seç
                .ToList();

                return result;
            }
        }
    }
}