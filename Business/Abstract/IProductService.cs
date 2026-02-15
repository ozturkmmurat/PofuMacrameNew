using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.Product;
using Entities.Dtos.Product.Select;
using Entities.Dtos.ProductVariant;
using Entities.EntitiyParameter.Product;
using Entities.EntityParameter.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();
        IDataResult<List<SelectProductDto>> GetallProductDto();
        IDataResult<int> GetTotalProduct(int categoryId);
        IDataResult<SelectProductDetailDto> GetProductDetailDtoByPvId(int productVariantId);
        IDataResult<List<SelectListProductVariantDto>> GetAllProductVariantDtoGroupVariant(FilterProduct filterProduct);
        IDataResult<List<SelectListProductVariantDto>> ProcessProductVariantData(List<SelectListProductVariantDto> processProductVariants);
        IDataResult<Product> GetById(int id);
        IDataResult<SelectProductDto> GetByProductDto(int productId);
        /// <summary>
        /// Ürün + ProductCategory ekler. ProductId 0, MainCategoryId zorunlu.
        /// </summary>
        IResult Add(ProductDto dto);
        IResult TsaAdd(AddProductVariant addProductVariant);
        /// <summary>
        /// Ürün bilgisi + ek kategorileri günceller. ProductId zorunlu; ana kategori değiştirilmez.
        /// </summary>
        IResult TsaUpdate(ProductDto dto);
        IResult Update(Product product);
        IResult Delete(Product product);
    }
}
