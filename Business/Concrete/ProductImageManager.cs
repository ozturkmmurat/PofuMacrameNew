using Business.Abstract;
using Business.Constans;
using Core.Helpers.FileHelper;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Product;
using Entities.Dtos.ProductImage;
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

        public IResult AddList(List<AddProductImageDto> addProductImageDtos)
        {
            if (addProductImageDtos != null)
            {
                List<ProductImage> productImages = new List<ProductImage>();
                for (int i = 0; i < addProductImageDtos.Count; i++)
                {
                    for (int j = 0; j < addProductImageDtos[i].Files.Count; j++)
                    {
                        ProductImage productImage = new ProductImage();
                        productImage.ProductId = addProductImageDtos[0].ProductId;
                        productImage.ProductVariantId = addProductImageDtos[0].ProductVariantId;
                        if (j == 0 && GetByProductIdProductVariantId(productImage.ProductId, productImage.ProductVariantId).Success == false)
                        {
                            productImage.IsMain = true;
                        }
                        else
                        {
                            productImage.IsMain = false;
                        }
                        productImage.AttributeValueId = addProductImageDtos[0].AttributeValueId;
                        productImage.CreateDate = DateTime.Now;
                        productImage.Path = _fileHelper.Upload(addProductImageDtos[i].Files[j], PathConstans.ImagesPath);
                        productImages.Add(productImage);
                    }
                }
               
                _productImageDal.AddRange(productImages);
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

        public IDataResult<ProductImage> GetByProductIdProductVariantId(int productId, int productVariantId)
        {
            var result = _productImageDal.Get(x => x.ProductId == productId && x.ProductVariantId == productVariantId && x.IsMain == true);
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
