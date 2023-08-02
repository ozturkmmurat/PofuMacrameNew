using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Product;
using Entities.Dtos.Variant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductAttributeImageService
    {
        IDataResult<List<ProductAttributeImage>> GetAll();
        IDataResult<ProductAttributeImage> GetById(int id);
        IResult Add(ProductAttributeImage productAttributeImage, IFormFile formFile);
        IResult AddList(List<ProductAttributeImage> productAttributeImage, List<IFormFile> formFiles);
        IResult Update(ProductAttributeImage productAttributeImage);
        IResult Delete(ProductAttributeImage productAttributeImage);
    }
}
