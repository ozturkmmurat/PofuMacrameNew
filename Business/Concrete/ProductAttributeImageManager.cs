using Business.Abstract;
using Business.Constans;
using Core.Helpers.FileHelper;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Product;
using Entities.Dtos.Variant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductAttributeImageManager : IProductAttributeImageService
    {
        IProductAttributeImageDal _productAttributeImageDal;
        IFileHelper _fileHelper;
        public ProductAttributeImageManager(IProductAttributeImageDal productAttributeImageDal, IFileHelper fileHelper)
        {
            _productAttributeImageDal = productAttributeImageDal;
            _fileHelper = fileHelper;
        }
        public IResult Add(ProductAttributeImage productAttributeImage, IFormFile formFile)
        {
            if (productAttributeImage != null)
            {
                productAttributeImage.Path = _fileHelper.Upload(formFile, PathConstans.ImagesPath);
                productAttributeImage.CreateDate = DateTime.Now;
                _productAttributeImageDal.Add(productAttributeImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult AddList(List<ProductAttributeImage> productAttributeImages, List<IFormFile> formFiles)
        {
            if (productAttributeImages != null)
            {
                for (int i = 0; i < formFiles.Count; i++)
                {
                    ProductAttributeImage productAttributeImage = new ProductAttributeImage();
                    productAttributeImage.ProductAttributeId = productAttributeImages[i].ProductAttributeId;
                    productAttributeImage.Path = _fileHelper.Upload(formFiles[i], PathConstans.ImagesPath);
                    productAttributeImages.Add(productAttributeImage);
                }
                _productAttributeImageDal.AddRange(productAttributeImages);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(ProductAttributeImage productAttributeImage)
        {
            if (productAttributeImage != null)
            {
                _productAttributeImageDal.Delete(productAttributeImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<ProductAttributeImage>> GetAll()
        {
            var result = _productAttributeImageDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<ProductAttributeImage>>(result);
            }
            return new ErrorDataResult<List<ProductAttributeImage>>();
        }

        public IDataResult<ProductAttributeImage> GetById(int id)
        {
            var result = _productAttributeImageDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<ProductAttributeImage>(result);
            }
            return new ErrorDataResult<ProductAttributeImage>();
        }

        public IResult Update(ProductAttributeImage productAttributeImage)
        {
            if (productAttributeImage != null)
            {
                _productAttributeImageDal.Update(productAttributeImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
