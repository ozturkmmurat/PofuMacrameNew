using Core.Utilities.Result.Abstract;
using Entities.Dtos.ProductVariant.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract.ProductVariants
{
    public interface IProductVariantAttributeCombinationService
    {
        IDataResult<List<List<ProductVariantAttributeValueDto>>> GetAllCombinationAttributeValue(int productId);
        IDataResult<List<ProductVariantAttributeValueDto>> GetAllEndCombinationAttributeValue(int productId);
        IDataResult<List<ProductVariantAttributeValueDto>> GetEndCombinationAttributeValue(int productId, int attributeValueId);

        IDataResult<List<List<ProductVariantAttributeValueDto>>> GetCombinationAttributeValue(int productId, int attributeValueId);

        void GenerateSubCombinations(List<ProductVariantAttributeValueDto> variants, List<ProductVariantAttributeValueDto> combination, int parentAttributeValueId, List<List<ProductVariantAttributeValueDto>> combinations);

    }
}
