using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Product;
using Entities.Dtos.Product.Select;
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
        IDataResult<List<ProductVariantGroupDetailDto>> GetSubProductVariantDetail(List<ProductVariantGroupDetailDto>  productVariantGroups, int productId, int parentId);
        IDataResult<List<ProductVariantGroupDetailDto>> GetDefaultProductVariantDetail(int productId, int parentId);
        IDataResult<List<ProductVariant>> MapProductVariantCombination(int productId, int productVariantId);
        IDataResult<List<ProductVariantAttributeValueDto>> GetProductVariantCombination(int productId, int productVariantId);
        IDataResult<List<List<ProductVariantAttributeValueDto>>> GetAllProductVariantCombination(int productId);
        IDataResult<ProductVariant> GetProductVariantByParentIdNT(int parentId);
        IDataResult<ProductVariant> GetById(int id);
        IDataResult<ProductVariant> GetByIdNT(int id); // NT -> AsNoTracking
        IDataResult<ProductVariant> GetByParentIdNT(int parentId); // NT -> AsNoTracking

        IDataResult<ProductVariant> GetByProductId(int productId);
        IDataResult<ProductVariant> MainVariantEndVariantNT(int productVariantId);
        IDataResult<ProductVariant> EndVariantMainVariantNT(int parentId);
        IDataResult<SelectListProductVariantDto> DtoEndVariantMainVariantNT(int parentId);
        IDataResult<SelectListProductVariantDto> DtoMainVariantEndVariantNT(int productVariantId);
        IDataResult<List<string>> ProductVariantImage(int productVariantId);
        IDataResult<ProductVariant> GetByParentIdAttrValueId(int productId,int? parentId, int? attributeValueId, int? attributeId);
        IDataResult<ProductVariantAttributeDto> GetProductVariantAttribute(int parentId); //İlgili varyantın attributelerini getirir. Sonuncu varyant dan başlayıp
        IResult Add(ProductVariant variant);
        IResult AddTsaProductVariant(AddProductVariantDto addProductVariant);
        IResult AddList(List<ProductVariant> variants);
        IResult Update(ProductVariant variant);
        IResult Delete(ProductVariant variant);
    }
}
