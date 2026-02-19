using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Entities.Dtos.ProductAttribute;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductAttributeDal : EfEntityRepositoryBase<ProductAttribute, PofuMacrameContext>, IProductAttributeDal
    {
        private readonly PofuMacrameContext _context;
        public EfProductAttributeDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }

        public List<ProductAttribute> GetAllProductIdListNT(List<int> ids)
        {
            var result = _context.ProductAttributes.AsNoTracking().Where(x => ids.Contains(x.ProductId)).ToList();
            return result;
        }

        public List<ProductAttributeDto> GetProductVariantAttribute(Expression<Func<ProductAttributeDto, bool>> filter = null)
        {
            var result = from pa in _context.ProductAttributes
                         join p in _context.Products
                         on pa.ProductId equals p.Id
                         join a in _context.Attributes
                         on pa.AttributeId equals a.Id


                         select new ProductAttributeDto
                         {
                             ProductId = p.Id,
                             ProductName = p.ProductName,
                             AttributeName = a.Name
                         };
            return filter == null ? result.ToList() : result.Where(filter).ToList();
        }

        public List<ProductAttributeDto> GetAllProductAttribute(ProductAttributeDto productAttributeDto)
        {
            if (productAttributeDto?.CategoryId == null || !productAttributeDto.CategoryId.Any())
                return new List<ProductAttributeDto>();

            var categoryIds = productAttributeDto.CategoryId;
            var query = from ca in _context.CategoryAttributes.AsNoTracking()
                         .Where(x => categoryIds.Contains(x.CategoryId) && x.Attribute == false && x.Slicer == false)
                        join av in _context.AttributeValues.AsNoTracking()
                        on ca.AttributeId equals av.AttributeId
                        select new Entities.Dtos.ProductAttribute.ProductAttributeDto
                        {
                            AttributeId = av.AttributeId,
                            AttributeValueId = av.Id,
                            AttributeValue = av.Value
                        };
            var list = query.ToList();
            var result = list.GroupBy(x => x.AttributeValueId).Select(g => g.First()).ToList();
            return result;
        }

        public List<FilterProductAttributeDto> GetFilterAttributesByProductIds(List<int> productIds)
        {
            if (productIds == null || !productIds.Any())
                return new List<FilterProductAttributeDto>();

            var productAttributes = _context.ProductAttributes
                .AsNoTracking()
                .Where(pa => productIds.Contains(pa.ProductId))
                .ToList();

            var distinctAttributeIds = productAttributes
                .Select(pa => pa.AttributeId)
                .Distinct()
                .ToList();

            if (distinctAttributeIds.Count == 0)
                return new List<FilterProductAttributeDto>();

            var attributeNames = _context.Attributes
                .AsNoTracking()
                .Where(a => distinctAttributeIds.Contains(a.Id))
                .ToDictionary(a => a.Id, a => a.Name);

            var allValueIds = productAttributes
                .Select(pa => pa.AttributeValueId)
                .Distinct()
                .ToList();

            var attributeValuesById = _context.AttributeValues
                .AsNoTracking()
                .Where(av => allValueIds.Contains(av.Id))
                .ToDictionary(av => av.Id);

            var result = new List<FilterProductAttributeDto>();

            foreach (var attributeId in distinctAttributeIds)
            {
                var valueIdsForAttribute = productAttributes
                    .Where(pa => pa.AttributeId == attributeId)
                    .Select(pa => pa.AttributeValueId)
                    .Distinct()
                    .ToList();

                var values = valueIdsForAttribute
                    .Where(id => attributeValuesById.ContainsKey(id))
                    .Select(id => attributeValuesById[id])
                    .ToList();

                result.Add(new FilterProductAttributeDto
                {
                    AttributeId = attributeId,
                    AttributeName = attributeNames.GetValueOrDefault(attributeId),
                    AttributeValues = values
                });
            }

            return result;
        }
    }
}
