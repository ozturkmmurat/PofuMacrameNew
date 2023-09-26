using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Product;
using Entities.Dtos.ProductVariant;
using Entities.Dtos.ProductVariant.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Abstract.ProductVariants
{
    public interface IProductVariantService
    {
        //Variant
        IDataResult<List<ProductVariant>> GetAll();
        IDataResult<List<ProductVariant>> GetAllByProductId(int productId);
        IDataResult<List<ProductVariant>> GetAllByProductIdAttrId(int productId, int? attributeId);
        IDataResult<List<ProductVariantDetailAttributeDto>> GetProductVariantDetail(int productId, int productVariantId);
        IDataResult<List<ProductVariant>> MapProductVariantCombination(int productId, int attributeValueId);
        IDataResult<List<ProductVariantAttributeValueDto>> GetProductVariantCombination(int productId, int attributeValueId);
        IDataResult<List<List<ProductVariantAttributeValueDto>>> GetAllProductVariantCombination(int productId);
        IDataResult<ProductVariant> GetById(int id);
        IDataResult<ProductVariant> GetByProductId(int productId);
        IDataResult<ProductVariant> GetByParentIdAttrValueId(int productId,int? parentId, int? attributeValueId, int? attributeId);
        IResult Add(ProductVariant variant);
        IResult AddTsaProductVariant(AddProductVariant addProductVariant);
        IResult AddList(List<ProductVariant> variants);
        IResult Update(ProductVariant variant);
        IResult Delete(ProductVariant variant);
    }
}
