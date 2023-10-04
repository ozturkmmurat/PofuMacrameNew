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
        IDataResult<List<ProductVariantAttributeValueDto>> GetEndCombinationAttributeValue(int productId, int productVariantId);

        IDataResult<List<ProductVariantAttributeValueDto>> GetCombinationAttributeValue(int productId, int productVariantId);

        void GenerateManySubCombinations(List<ProductVariantAttributeValueDto> variants, List<ProductVariantAttributeValueDto> combination, int parentId, List<List<ProductVariantAttributeValueDto>> combinations);
        void GenerateSubCombinations(List<ProductVariantAttributeValueDto> variants, List<ProductVariantAttributeValueDto> combination, int parentId);

    }
}
