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
        public List<ProductVariantAttributeDto> GetAllFilterDto(Expression<Func<ProductVariantAttributeDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from pv in context.ProductVariants
                             join p in context.Products on pv.ProductId equals p.Id
                             join av in context.AttributeValues on pv.AttributeValueId equals av.Id
                             join a in context.Attributes on av.AttributeId equals a.Id
                             select new ProductVariantAttributeDto
                             {
                                 ProductVariantId = pv.Id,
                                 ProductId = p.Id,
                                 ParentId = pv.ParentId,
                                 ProductPaths = context.ProductImages
                                                      .Where(x => x.ProductVariantId == pv.Id)
                                                      .Take(2)
                                                      .Select(pi => pi.Path)
                                                      .ToList(),
                                 
                             };
                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }

        public List<MainProductVariantAttributeDto> GetProductVariantAttributes(int productId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from pv in context.ProductVariants.Where(x => x.ProductId == productId)
                              join a in context.Attributes
                              on pv.AttributeId equals a.Id

                              select new MainProductVariantAttributeDto
                              {
                                  ProductId = pv.ProductId,
                                  ParentId = pv.ParentId,
                                  AttributeName = a.Name,
                                  AttributeId = a.Id,
                                  ProductVariantAttributeValueDtos = new List<ProductVariantAttributeValueDto>()
                              }).ToList().GroupBy(x => x.AttributeId).Select(group => group.First()).ToList();

                return result;
            }
        }

        public List<ProductVariantAttributeValueDto> GetMainProductAttributeValue(int productId, int? parentId, int attributeId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from pv in context.ProductVariants.Where(x => x.ProductId == productId && x.ParentId == parentId && x.AttributeId == attributeId)
                              join av in context.AttributeValues
                              on pv.AttributeValueId equals av.Id

                              select new ProductVariantAttributeValueDto
                              {
                                  ProductVariantId = pv.Id,
                                  ProductId = pv.ProductId,
                                  ParentId = pv.ParentId,
                                  AttributeValue =  av.Value,
                              }).ToList();

                return result;
            }
        }

        public List<ProductVariantAttributeValueDto> GetSubProductAttributeDto(int productVariantId)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = (from pv in context.ProductVariants.Where(x => x.Id == productVariantId)
                              join av in context.AttributeValues
                              on pv.AttributeValueId equals av.Id

                              select new ProductVariantAttributeValueDto
                              {
                                  ProductVariantId = pv.Id,
                                  ProductId = pv.ProductId,
                                  ParentId = pv.ParentId,
                                  AttributeValue =  av.Value
                              }).ToList();

                return result;
            }
        }
    }
}
