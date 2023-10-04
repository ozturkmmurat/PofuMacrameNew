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
        public List<ProductVariantDetailAttributeDto> GetAllProductDetailAttributeByParentId(int parentId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from pv in context.ProductVariants.Where(x => x.ParentId == parentId)
                              join a in context.Attributes
                              on pv.AttributeId equals a.Id

                              select new ProductVariantDetailAttributeDto
                              {
                                  ParentId = pv.ParentId,
                                  AttributeId = pv.AttributeId,
                                  AttributeName =  a.Name,
                                  AttributeValues = new List<AttributeValue>(),
                              }).ToList();

                return result;
            }
        }

        //Urun detay sayfasi icin kullaniliyor. Kullanilan yerler --> (Kullanicilarin girdigi urunlerin detay bolumu)
        public List<ProductVariantDetailAttributeDto> GetAllProductDetailAttributeByProductId(int productId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from pv in context.ProductVariants.Where(x => x.ProductId == productId )
                              join a in context.Attributes
                              on pv.AttributeId equals a.Id

                              select new ProductVariantDetailAttributeDto
                              {
                                  ParentId = pv.ParentId,
                                  AttributeId = pv.AttributeId,
                                  AttributeName =  a.Name,
                                  AttributeValues = new List<AttributeValue>(),
                              }).ToList();

                return result;
            }
        }

        public List<ProductVariantAttributeValueDto> GetAllProductDetailAttributeByProductIdParentId(int productId, int parentId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from pv in context.ProductVariants.Where(x => x.ProductId == productId && x.ParentId == parentId)
                              join av in context.AttributeValues 
                              on pv.AttributeValueId equals av.Id
                              select new ProductVariantAttributeValueDto
                              {
                                  ProductId = pv.ProductId,
                                  ParentId = pv.ParentId,
                                  ProductVariantId = pv.Id,
                                  AttributeId = pv.AttributeId.Value,
                                  AttributeValueId = av.Id,
                                  AttributeValue = av.Value
                              }).ToList();

                return result;
            }
        }

        public List<ProductVariantAttributeValueDto> GetAllSubProductAttributeDtoByParentId(int parentId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from pv in context.ProductVariants.Where(x => x.ParentId == parentId)
                              join a in context.Attributes
                              on pv.AttributeId equals a.Id
                              join av in context.AttributeValues
                              on pv.AttributeValueId equals av.Id
                              select new ProductVariantAttributeValueDto
                              {
                                  ProductId = pv.ProductId,
                                  ParentId = pv.ParentId,
                                  ProductVariantId = pv.Id,
                                  AttributeId = pv.AttributeId.Value,
                                  AttributeValueId = av.Id,
                                  AttributeName =  a.Name,
                                  AttributeValue = av.Value,
                              }).ToList();

                return result;
            }
        }

        //Panel de kullaniliyor. Kullanilan yerler --> (Stok alani)

        public List<ProductVariantAttributeValueDto> GetAllSubProductAttributeDtoProductId(int productId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from pv in context.ProductVariants.Where(x => x.ProductId == productId)
                              join a in context.Attributes 
                              on pv.AttributeId equals a.Id
                              join av in context.AttributeValues
                              on pv.AttributeValueId equals av.Id
                              select new ProductVariantAttributeValueDto
                              {
                                  ProductId = pv.ProductId,
                                  ParentId = pv.ParentId,
                                  ProductVariantId = pv.Id,
                                  AttributeId = pv.AttributeId.Value,
                                  AttributeValueId = av.Id,
                                  AttributeName =  a.Name,
                                  AttributeValue = av.Value
                              }).ToList();

                return result;
            }
        }
    }
}

