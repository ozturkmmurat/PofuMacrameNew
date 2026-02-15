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
        IDataResult<List<string>> GetFirstTwoPathNT(int productVariantId);
        IDataResult<ProductImage> GetById(int id);
        IDataResult<ProductImage> GetByIdNT(int id); //NT -> AsNoTracking
        IDataResult<ProductImage> GetByProductVariantId(int productVariantId);
        IResult Add(ProductImage productImage, IFormFile formFile);
        IResult AddList(ProductImageDto addProductImageDtos);
        IResult Update(ProductImage productImage, IFormFile formFile);
        IResult Delete(ProductImage productImage);
        IResult CheckImageLimit(int productVariantId, int fileCount);
    }
}
