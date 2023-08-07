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
    public class ProductImageManager : IProductImageService
    {
        IProductmageDal _productImageDal;
        IFileHelper _fileHelper;
        public ProductImageManager(IProductmageDal productImageDal, IFileHelper fileHelper)
        {
            _productImageDal = productImageDal;
            _fileHelper = fileHelper;
        }
        public IResult Add(ProductImage productAttributeImage, IFormFile formFile)
        {
            if (productAttributeImage != null)
            {
                productAttributeImage.Path = _fileHelper.Upload(formFile, PathConstans.ImagesPath);
                productAttributeImage.CreateDate = DateTime.Now;
                _productImageDal.Add(productAttributeImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult AddList(List<ProductImage> productAttributeImages, List<IFormFile> formFiles)
        {
            if (productAttributeImages != null)
            {
                for (int i = 0; i < formFiles.Count; i++)
                {
                    ProductImage productAttributeImage = new ProductImage();
                    productAttributeImage.ProductAttributeId = productAttributeImages[i].ProductAttributeId;
                    productAttributeImage.Path = _fileHelper.Upload(formFiles[i], PathConstans.ImagesPath);
                    productAttributeImages.Add(productAttributeImage);
                }
                _productImageDal.AddRange(productAttributeImages);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(ProductImage productAttributeImage)
        {
            if (productAttributeImage != null)
            {
                _productImageDal.Delete(productAttributeImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<ProductImage>> GetAll()
        {
            var result = _productImageDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<ProductImage>>(result);
            }
            return new ErrorDataResult<List<ProductImage>>();
        }

        public IDataResult<List<ProductImage>> GetAllByProductVariantId(int productVariantId)
        {
            var result = _productImageDal.GetAll(x => x.ProductVariantId == productVariantId);
            if (result != null)
            {
                return new SuccessDataResult<List<ProductImage>>(result);
            }
            return new ErrorDataResult<List<ProductImage>>();
        }

        public IDataResult<ProductImage> GetById(int id)
        {
            var result = _productImageDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<ProductImage>(result);
            }
            return new ErrorDataResult<ProductImage>();
        }

        public IResult Update(ProductImage productAttributeImage)
        {
            if (productAttributeImage != null)
            {
                _productImageDal.Update(productAttributeImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
