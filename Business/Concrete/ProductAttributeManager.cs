using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductAttributeManager : IProductAttributeService
    {
        private readonly IProductAttributeDal _productAttributeDal;
        private readonly ICategoryAttributeService _categoryAttributeService;

        public ProductAttributeManager(IProductAttributeDal productAttributeDal, ICategoryAttributeService categoryAttributeService)
        {
            _productAttributeDal = productAttributeDal;
            _categoryAttributeService = categoryAttributeService;
        }
        public IResult Add(ProductAttribute productAttribute)
        {
            if (productAttribute != null)
            {
                _productAttributeDal.Add(productAttribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult AddRange(List<ProductAttribute> productAttributes)
        {
            if (productAttributes == null || productAttributes.Count == 0)
            {
                return new ErrorResult();
            }

            var attributeIds = productAttributes.Select(pa => pa.AttributeId).Distinct().ToList();
            var categoryAttributesResult = _categoryAttributeService.GetByAttributeIds(attributeIds);
            if (!categoryAttributesResult.Success || categoryAttributesResult.Data == null)
            {
                return new ErrorResult();
            }

            var categoryAttributes = categoryAttributesResult.Data;
            var attributeIdsSlicerOrAttributeTrue = new HashSet<int>(categoryAttributes
                .Where(ca => ca.Slicer || ca.Attribute)
                .Select(ca => ca.AttributeId)
                .Distinct());
            var attributeIdsSlicerAndAttributeFalse = new HashSet<int>(categoryAttributes
                .Where(ca => !ca.Slicer && !ca.Attribute)
                .Select(ca => ca.AttributeId)
                .Distinct()
                .Where(id => !attributeIdsSlicerOrAttributeTrue.Contains(id)));

            var productAttributesFalseToAdd = productAttributes
                .Where(pa => attributeIdsSlicerAndAttributeFalse.Contains(pa.AttributeId))
                .ToList();
            var productAttributesExcluded = productAttributes
                .Where(pa => attributeIdsSlicerOrAttributeTrue.Contains(pa.AttributeId))
                .ToList();

            // False grubu: DB'de olup gelen listede olmayanları kaldır, gelen listede olup DB'de olmayanları ekle
            if (productAttributesFalseToAdd.Count > 0)
            {
                var productIds = productAttributesFalseToAdd.Select(pa => pa.ProductId).Distinct().ToList();
                var existingInDb = _productAttributeDal.GetAll(pa =>
                    productIds.Contains(pa.ProductId) && attributeIdsSlicerAndAttributeFalse.Contains(pa.AttributeId));

                var incomingSet = new HashSet<(int ProductId, int AttributeId, int AttributeValueId)>(
                    productAttributesFalseToAdd.Select(pa => (pa.ProductId, pa.AttributeId, pa.AttributeValueId)));

                var toRemove = existingInDb
                    .Where(e => !incomingSet.Contains((e.ProductId, e.AttributeId, e.AttributeValueId)))
                    .ToList();
                var toAdd = productAttributesFalseToAdd
                    .Where(i => !existingInDb.Any(e => e.ProductId == i.ProductId && e.AttributeId == i.AttributeId && e.AttributeValueId == i.AttributeValueId))
                    .ToList();

                if (toRemove.Count > 0)
                    _productAttributeDal.DeleteRange(toRemove);
                if (toAdd.Count > 0)
                    _productAttributeDal.AddRange(toAdd);
            }

            if (productAttributesExcluded.Count > 0)
                _productAttributeDal.AddRange(productAttributesExcluded);

            return new SuccessResult();
        }

        public IResult Delete(ProductAttribute productAttribute)
        {
            if (productAttribute != null)
            {
                _productAttributeDal.Delete(productAttribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<ProductAttribute>> GetAll()
        {
            var result = _productAttributeDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<ProductAttribute>>(result);
            }
            return new ErrorDataResult<List<ProductAttribute>>();
        }

        public IDataResult<List<ProductAttribute>> GetAllByProductId(int productId)
        {
            var result = _productAttributeDal.GetAll(x => x.ProductId == productId);
            if (result != null)
            {
                return new SuccessDataResult<List<ProductAttribute>>(result);
            }
            return new ErrorDataResult<List<ProductAttribute>>();
        }

        public IDataResult<List<ProductAttribute>> GetAllByProductIds(List<int> productIds)
        {
            var result = _productAttributeDal.GetAllProductIdListNT(productIds);
            if (result != null & result.Count > 0)
            {
                return new SuccessDataResult<List<ProductAttribute>>(result);
            }
            return new ErrorDataResult<List<ProductAttribute>>();
        }

        public IDataResult<ProductAttribute> MappingProductAttribute(ProductVariant productVariant)
        {
            if (productVariant != null && productVariant.AttributeId != 0 && productVariant.AttributeValueId != 0)
            {
                ProductAttribute productAttribute = new ProductAttribute();
                productAttribute.ProductId = productVariant.ProductId;
                productAttribute.AttributeValueId = productVariant.AttributeValueId;
                productAttribute.AttributeId = productVariant.AttributeId;
                return new SuccessDataResult<ProductAttribute>(productAttribute);
            }
            return new ErrorDataResult<ProductAttribute>();
        }

        public IResult Update(ProductAttribute productAttribute)
        {
            if (productAttribute != null)
            {
                _productAttributeDal.Update(productAttribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
