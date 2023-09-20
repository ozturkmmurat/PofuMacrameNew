using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Product;
using Entities.Dtos.ProductVariant;
using Entities.Dtos.ProductVariant.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductVariantService
    {
        //Variant
        IDataResult<List<ProductVariant>> GetAll();
        IDataResult<List<ProductVariant>> GetAllByProductId(int productId);
        IDataResult<List<ProductVariant>> GetAllByProductIdAttrId(int productId, int? attributeId);
        IDataResult<List<ProductVariant>> GetSubProductVariantById(int productVariantId);
        IDataResult<List<ProductVariantAttributeDto>> GetAllMainProductVariant(int productId);
        IDataResult<List<MainProductVariantAttributeDto>> GetProductVariantAttribute(int productId);
        IDataResult<List<ProductVariantAttributeValueDto>> GetMainProductVariantAttrValue(int productId, int? parentId, int attributeId);
        IDataResult<ProductVariant> GetById(int id);
        IDataResult<ProductVariant> GetByProductId(int productId);
        IDataResult<ProductVariant> GetByParentIdAttrValueId(int productId ,int? parentId, int? attributeValueId);
        IResult Add(ProductVariant variant);
        IResult AddTsaProductVariant(AddProductVariant addProductVariant);
        IResult AddList(List<ProductVariant> variants);
        IResult Update(ProductVariant variant);
        IResult Delete(ProductVariant variant);
    }
}
