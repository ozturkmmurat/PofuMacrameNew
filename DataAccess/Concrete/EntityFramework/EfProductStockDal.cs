using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.ProductStock;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using Core.Utilities.Result.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductStockDal : EfEntityRepositoryBase<ProductStock, PofuMacrameContext>, IProductStockDal
    {
        public List<SelectProductStockDto> GetByDto(int productId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from ps in context.ProductStocks
                             join pv in context.ProductVariants on ps.ProductVariantId equals pv.Id
                             join p in context.Products on pv.ProductId equals p.Id
                             join c in context.Categories on p.CategoryId equals c.Id
                             where p.Id == productId
                             select new SelectProductStockDto
                             {
                                 ProductStockId = ps.Id,
                                 ProductId = p.Id,
                                 EndProductVariantId = ps.ProductVariantId,
                                 AttributeValueId = pv.AttributeValueId,
                                 ParentId = pv.ParentId,
                                 Price = ps.Price,
                                 Quantity = ps.Quantity,
                                 StockCode = ps.StockCode,
                             }).ToList();

                var productVariant = context.ProductVariants.ToList();
                var attributeValue = context.AttributeValues.ToList();
                var productVariantList = new List<ProductVariant>();
                var x = result.ToList();
                for (int i = 0; i < result.ToList().Count; i++)
                {
                    for (int j = 0; j < productVariant.Count; j++)
                    {
                        if (result[i].ParentId == productVariant[j].Id)
                        {
                           result[i].AttributeValue += attributeValue.FirstOrDefault(x => x.Id == result[i].AttributeValueId).Value + " ";
                            productVariantList.Add(productVariant.FirstOrDefault(x => x.Id == result[i].ParentId));
                            for (int k = 0; k < productVariantList.Count; k++)
                            {
                                result[i].AttributeValue += attributeValue.FirstOrDefault(x => x.Id == productVariantList[k].AttributeValueId).Value + " "; 
                                if (productVariantList[k].ParentId != null)
                                {
                                    productVariantList.Add(productVariant.FirstOrDefault(x => x.Id == productVariantList[k].ParentId));
                                }else if (productVariantList[k].ParentId == null)
                                {
                                    result[i].FirstProductVariantId = productVariantList[k].Id;
                                    result[i].AttributeId = productVariantList[k].AttributeId;
                                    result[i].AttributeValueId = productVariantList[k].AttributeValueId;
                                    result[i].ParentId = null;
                                }
                            }
                        }
                    }
                    productVariantList.Clear();
                }
                return result;
            }
        }
    }
}
