using Business.Abstract;
using Business.Constans;
using Core.Business;
using Core.Helpers.FileHelper;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ImageManager : IImageService
    {
        IImageDal _imageDal;
        IFileHelper _fileHelper;
        public ImageManager(IImageDal imageDal, IFileHelper fileHelper)
        {
            _imageDal = imageDal;
            _fileHelper = fileHelper;
        }
        public IResult Add(Image image, IFormFile formFile)
        {
            IResult result = BusinessRules.Run(CheckIfImageLimit(image.EntityTypeId));
            if (image != null && result == null)
            {
                image.ImagePath = _fileHelper.Upload(formFile, PathConstans.ImagesPath);
                image.CreateDate = DateTime.Now;
                _imageDal.Add(image);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult AddList(List<Image> images, IFormFile formFile)
        {
            if (images !=null && images.Count <=5)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    images[i].ImagePath = _fileHelper.Upload(formFile, PathConstans.ImagesPath);
                    images[i].CreateDate = DateTime.Now;
                }
                _imageDal.AddRange(images);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult CheckIfImageLimit(int entityTypeId)
        {
            var result = _imageDal.GetAll(x => x.EntityTypeId == entityTypeId).Count;
            if (result >= 5)
            {
                return new ErrorResult();
            }
            return new SuccessResult(); 
        }

        public IResult Delete(Image image)
        {
            if (image != null)
            {
                _fileHelper.Delete(PathConstans.ImagesPath + image.ImagePath);
                _imageDal.Delete(image);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<Image>> GetAll()
        {
            var result = _imageDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<Image>>(result);
            }
            return new ErrorDataResult<List<Image>>();
        }

        public IDataResult<Image> GetByEntTypeId(int entityTypeId)
        {
            var result = _imageDal.Get(x => x.EntityTypeId == entityTypeId);
            if (result != null)
            {
                return new SuccessDataResult<Image>(result);
            }
            return new ErrorDataResult<Image>();
        }

        public IDataResult<Image> GetById(int id)
        {
            var result = _imageDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<Image>(result);
            }
            return new ErrorDataResult<Image>();
        }

        public IResult Update(Image image, IFormFile file)
        {
            if (image != null)
            {
                image.ImagePath = _fileHelper.Update(file, PathConstans.ImagesPath + image.ImagePath, PathConstans.ImagesPath);
                _imageDal.Update(image);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
