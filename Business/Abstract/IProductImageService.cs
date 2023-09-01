using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Product;
using Entities.Dtos.ProductImage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductImageService
    {
        IDataResult<List<ProductImage>> GetAll();
        IDataResult<List<ProductImage>> GetAllByProductVariantId(int productVariantId);
        IDataResult<ProductImage> GetById(int id);
        IDataResult<ProductImage> GetByProductIdProductVariantId(int productId, int productVariantId);
        IResult Add(ProductImage productAttributeImage, IFormFile formFile);
        IResult AddList(List<AddProductImageDto> addProductImageDtos);
        IResult Update(ProductImage productAttributeImage);
        IResult Delete(ProductImage productAttributeImage);
    }
}
