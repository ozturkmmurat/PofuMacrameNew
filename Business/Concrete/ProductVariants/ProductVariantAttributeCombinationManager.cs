using Business.Abstract.ProductVariants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Dtos.ProductVariant.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete.ProductVariants
{
    public class ProductVariantAttributeCombinationManager : IProductVariantAttributeCombinationService
    {
        IProductVariantDal _productVariantDal;
        public ProductVariantAttributeCombinationManager(IProductVariantDal productVariantDal)
        {
            _productVariantDal = productVariantDal;
        }
        public IDataResult<List<List<ProductVariantAttributeValueDto>>> GetAllCombinationAttributeValue(int productId)
        {
            var result = _productVariantDal.GetSubProductAttributeDto(productId);
            if (result != null && result.Any())
            {
                List<List<ProductVariantAttributeValueDto>> allCombinations = new List<List<ProductVariantAttributeValueDto>>();

                // Ana grupları bulun
                var mainGroups = result.Where(va => va.ParentId == 0).ToList();

                foreach (var mainGroup in mainGroups)
                {
                    List<List<ProductVariantAttributeValueDto>> combinations = new List<List<ProductVariantAttributeValueDto>>();

                    // Başlangıç üst grubunu bulun
                    var startGroup = result.FirstOrDefault(v => v.ParentId == mainGroup.ParentId && v.AttributeValueId == mainGroup.AttributeValueId);

                    if (startGroup != null)
                    {
                        // Başlangıç üst grubu için kombinasyonları oluşturun
                        List<ProductVariantAttributeValueDto> combination = new List<ProductVariantAttributeValueDto> { startGroup };
                        GenerateSubCombinations(result, combination, Convert.ToInt32(startGroup.AttributeValueId), combinations);
                    }

                    allCombinations.AddRange(combinations);
                }

                return new SuccessDataResult<List<List<ProductVariantAttributeValueDto>>>(allCombinations);
            }
            return new ErrorDataResult<List<List<ProductVariantAttributeValueDto>>>();

        }

        public void GenerateSubCombinations(List<ProductVariantAttributeValueDto> variants, List<ProductVariantAttributeValueDto> combination, int parentAttributeValueId, List<List<ProductVariantAttributeValueDto>> combinations)
        {
            // Alt grupları bulun (Belirli bir ParentId'ye sahip olanlar)
            var subGroups = variants.Where(v => v.ParentId == parentAttributeValueId).ToList();

            if (subGroups.Count == 0)
            {
                // Bu bir kombinasyonu tamamladık, listeye ekleyin
                combinations.Add(new List<ProductVariantAttributeValueDto>(combination));
                return;
            }

            foreach (var subGroup in subGroups)
            {
                // Alt grup için kombinasyonları oluşturun
                combination.Add(subGroup);
                GenerateSubCombinations(variants, combination, Convert.ToInt32(subGroup.AttributeValueId), combinations);
                combination.RemoveAt(combination.Count - 1); // Kombinasyonu geri al
            }
        }
        public IDataResult<List<List<ProductVariantAttributeValueDto>>> GetCombinationAttributeValue(int productId, int attributeValueId)
        {
            var result = _productVariantDal.GetSubProductAttributeDto(productId);
            if (result != null)
            {
                List<List<ProductVariantAttributeValueDto>> combinations = new List<List<ProductVariantAttributeValueDto>>();
                var mainGroups = result.Where(va => va.ParentId == 0 && va.AttributeValueId == attributeValueId).FirstOrDefault();

                // Başlangıç üst grubunu bulun
                var startGroup = result.FirstOrDefault(v => v.ParentId == mainGroups.ParentId && v.AttributeValueId == attributeValueId);

                if (startGroup != null)
                {
                    // Başlangıç üst grubu için kombinasyonları oluşturun
                    List<ProductVariantAttributeValueDto> combination = new List<ProductVariantAttributeValueDto> { startGroup };
                    GenerateSubCombinations(result, combination, Convert.ToInt32(startGroup.AttributeValueId), combinations);
                }

                return new SuccessDataResult<List<List<ProductVariantAttributeValueDto>>>(combinations);
            }
            return new ErrorDataResult<List<List<ProductVariantAttributeValueDto>>>();
        }

        public IDataResult<List<ProductVariantAttributeValueDto>> GetAllEndCombinationAttributeValue(int productId)
        {
            var result = GetAllCombinationAttributeValue(productId).Data;
            if (result != null)
            {
                if (result.Count() > 0)
                {
                    List<ProductVariantAttributeValueDto> productVariantAttrList = new List<ProductVariantAttributeValueDto>();
                    foreach (var items in result)
                    {
                        ProductVariantAttributeValueDto productVariantAttributeValueDto = new ProductVariantAttributeValueDto();
                        foreach (var item in items)
                        {
                            if (item != items.Last())
                            {
                                productVariantAttributeValueDto.AttributeValue += item.AttributeName + ": " + item.AttributeValue + " - ";
                            }
                            if (item == items.First() && (item.ParentId == 0 || item.ParentId == null))
                            {
                                productVariantAttributeValueDto.ProductVariantId = item.ProductVariantId;
                            }
                            if (item == items.Last())
                            {
                                productVariantAttributeValueDto.EndProductVariantId = item.ProductVariantId;
                                productVariantAttributeValueDto.ProductId = item.ProductId;
                                productVariantAttributeValueDto.ParentId = item.ParentId;
                                productVariantAttributeValueDto.AttributeValue += item.AttributeName + ": " + item.AttributeValue;
                                productVariantAttrList.Add(productVariantAttributeValueDto);
                            }
                        }
                    }
                    return new SuccessDataResult<List<ProductVariantAttributeValueDto>>(productVariantAttrList);
                }
            }
            return new ErrorDataResult<List<ProductVariantAttributeValueDto>>();
        }

        public IDataResult<List<ProductVariantAttributeValueDto>> GetEndCombinationAttributeValue(int productId, int attributeValueId)
        {
            var result = GetCombinationAttributeValue(productId, attributeValueId).Data;
            if (result != null)
            {
                if (result.Count() > 0)
                {
                    List<ProductVariantAttributeValueDto> productVariantAttrList = new List<ProductVariantAttributeValueDto>();
                    foreach (var items in result)
                    {
                        ProductVariantAttributeValueDto productVariantAttributeValueDto = new ProductVariantAttributeValueDto();
                        foreach (var item in items)
                        {
                            if (item != items.Last())
                            {
                                productVariantAttributeValueDto.AttributeValue += item.AttributeName + ": " + item.AttributeValue + " - ";
                            }
                            if (item == items.First() && (item.ParentId == 0 || item.ParentId == null))
                            {
                                productVariantAttributeValueDto.ProductVariantId = item.ProductVariantId;
                            }
                            if (item == items.Last())
                            {
                                productVariantAttributeValueDto.EndProductVariantId = item.ProductVariantId;
                                productVariantAttributeValueDto.ProductId = item.ProductId;
                                productVariantAttributeValueDto.ParentId = item.ParentId;
                                productVariantAttributeValueDto.AttributeValue += item.AttributeName + ": " + item.AttributeValue;
                                productVariantAttrList.Add(productVariantAttributeValueDto);
                            }
                        }
                    }
                    return new SuccessDataResult<List<ProductVariantAttributeValueDto>>(productVariantAttrList);
                }
            }
            return new ErrorDataResult<List<ProductVariantAttributeValueDto>>();
        }
    }
}
