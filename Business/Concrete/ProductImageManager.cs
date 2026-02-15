using Business.Abstract;
using Business.Constans;
using Core.Business;
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
using System.Linq;
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
        public IResult Add(ProductImage productImage, IFormFile formFile)
        {
            if (productImage != null)
            {
                productImage.Path = _fileHelper.Upload(formFile, PathConstans.ImagesPath);
                productImage.CreateDate = DateTime.Now;
                _productImageDal.Add(productImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult AddList(ProductImageDto addProductImageDtos)
        {
            if (addProductImageDtos != null)
            {
                List<ProductImage> productImages = new List<ProductImage>();
                if (addProductImageDtos.Files != null)
                {
                    if (addProductImageDtos.Files.Count > 0)
                    {
                        var checkImageLimit = CheckImageLimit(addProductImageDtos.ProductVariantId, addProductImageDtos.Files.Count);
                        IResult result = BusinessRules.Run(checkImageLimit);
                        if (result != null)
                        {
                            return new ErrorResult(checkImageLimit.Message);
                        }
                        for (int j = 0; j < addProductImageDtos.Files.Count; j++)
                        {
                            ProductImage productImage = new ProductImage();
                            productImage.ProductVariantId = addProductImageDtos.ProductVariantId;
                            productImage.ProductId = addProductImageDtos.ProductId;
                            if (j == 0 && GetByProductVariantId(productImage.ProductVariantId).Success == false)
                            {
                                productImage.IsMain = true;
                            }
                            else
                            {
                                productImage.IsMain = false;
                            }
                            productImage.CreateDate = DateTime.Now;
                            productImage.Path = _fileHelper.Upload(addProductImageDtos.Files[j], PathConstans.ImagesPath);
                            productImages.Add(productImage);
                        }
                        _productImageDal.AddRange(productImages);
                        return new SuccessResult(Messages.SuccessAdd);
                    }
                }
            }
            return new ErrorResult(Messages.UnSuccessAdd);
        }

        public IResult CheckImageLimit(int productVariantId, int fileCount)
        {
            var imageCount = GetAllByProductVariantId(productVariantId).Data.Count;
            var limit = imageCount + fileCount;
            if (limit <= 5)
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.ImageLimit + " " + imageCount);
        }

        public IResult Delete(ProductImage productImage)
        {
            if (productImage != null)
            {
                _productImageDal.Delete(productImage);
                if (productImage.IsMain == true)
                {
                    var controlImage = GetByProductVariantId(productImage.ProductVariantId).Data;
                    if (controlImage != null)
                    {
                        controlImage.IsMain = true;
                        Update(controlImage, null);
                    }
                }

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

        public IDataResult<ProductImage> GetByIdNT(int id)
        {
            var result = _productImageDal.GetAsNoTracking(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<ProductImage>(result);
            }
            return new ErrorDataResult<ProductImage>();
        }

        public IDataResult<ProductImage> GetByProductVariantId(int productVariantId)
        {
            var result = _productImageDal.Get(x => x.ProductVariantId == productVariantId && x.IsMain == true);
            if (result != null)
            {
                return new SuccessDataResult<ProductImage>(result);
            }
            return new ErrorDataResult<ProductImage>();
        }

        public IDataResult<List<string>> GetFirstTwoPathNT(int productVariantId)
        {
            var result = _productImageDal.GetFirstTwoPhotosNT(productVariantId);

            if (result != null)
            {
                return new SuccessDataResult<List<string>>(result);
            }
            return new ErrorDataResult<List<string>>();
        }

        public IResult Update(ProductImage productImage, IFormFile formFile)
        {
            if (productImage != null)
            {
                var getAllImage = GetAllByProductVariantId(productImage.ProductVariantId);
                if (getAllImage.Data?.Count > 0)
                {
                    var updateData = getAllImage.Data.FirstOrDefault(x => x.Id == productImage.Id);
                    updateData.IsMain = true;
                    updateData.CreateDate = DateTime.Now;
                    updateData.ProductVariantId = productImage.ProductVariantId;
                    if (formFile != null)
                    {
                        productImage.Path = _fileHelper.Upload(formFile, PathConstans.ImagesPath);
                    }
                    else
                    {
                        productImage.Path = updateData.Path;
                    }
                    getAllImage.Data.Remove(updateData);
                    _productImageDal.Update(updateData);

                    getAllImage.Data.Select(x => x.IsMain = false).ToList();
                    _productImageDal.UpdateRange(getAllImage.Data);
                }
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
