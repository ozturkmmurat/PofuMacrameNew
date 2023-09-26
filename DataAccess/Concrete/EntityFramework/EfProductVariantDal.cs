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
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductVariantDal : EfEntityRepositoryBase<ProductVariant, PofuMacrameContext>, IProductVariantDal
    {
        //Urun detay sayfasi icin kullaniliyor. Kullanilan yerler --> (Kullanicilarin girdigi urunlerin detay bolumu)
        public List<ProductVariantDetailAttributeDto> GetProductDetailAttribute(int productId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from pv in context.ProductVariants.Where(x => x.ProductId == productId )
                              join a in context.Attributes
                              on pv.AttributeId equals a.Id

                              select new ProductVariantDetailAttributeDto
                              {
                                  ParentId = pv.ParentId,
                                  ProductVariantId = pv.Id,
                                  AttributeId = pv.AttributeId,
                                  AttributeValueId = pv.AttributeValueId,
                                  AttributeName =  a.Name,
                                  AttributeValues = new List<AttributeValue>(),
                                  ProductPaths = context.ProductImages
                                                        .Where(x => x.ProductVariantId == pv.Id)
                                                        .Select(x => x.Path).ToList()
                              }).ToList();

                return result;
            }
        }
        //İlgili urunun ana varyantlarinin kombinasyonunu yaparken kullaniliyor. Kullanilan yerler -->(Admin Panel ProductStock)
        public List<ProductVariantAttributeValueDto> GetSubProductAttributeDto(int productId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from pv in context.ProductVariants.Where(x => x.ProductId == productId)
                              join av in context.AttributeValues on pv.AttributeValueId equals av.Id
                              join a in context.Attributes on av.AttributeId equals a.Id

                              select new ProductVariantAttributeValueDto
                              {
                                  ProductVariantId = pv.Id,
                                  ProductId = pv.ProductId,
                                  AttributeId = a.Id,
                                  AttributeValueId = pv.AttributeValueId,
                                  ParentId = pv.ParentId,
                                  AttributeName = a.Name,
                                  AttributeValue =  av.Value
                              }).ToList();

                return result;
            }
        }
    }
}
