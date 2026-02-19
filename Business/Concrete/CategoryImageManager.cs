using Business.Abstract;
using Business.Constans;
using Core.Helpers.FileHelper;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.CategoryImage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Business.Concrete
{
    public class CategoryImageManager : ICategoryImageService
    {
        ICategoryImageDal _categoryImageDal;
        IFileHelper _fileHelper;

        public CategoryImageManager(ICategoryImageDal categoryImageDal, IFileHelper fileHelper)
        {
            _categoryImageDal = categoryImageDal;
            _fileHelper = fileHelper;
        }

        public IResult Add(CategoryImage categoryImage, IFormFile file)
        {
            if (categoryImage != null && file != null)
            {
                categoryImage.Path = _fileHelper.Upload(file, PathConstans.ImagesPath);
                categoryImage.CreateDate = DateTime.Now;
                // Yeni eklenen fotoğraf son sırayı alır
                var existing = _categoryImageDal.GetAll(x => x.CategoryId == categoryImage.CategoryId);
                categoryImage.SequenceNumber = existing.Count > 0 ? existing.Max(x => x.SequenceNumber) + 1 : 1;
                _categoryImageDal.Add(categoryImage);
                return new SuccessResult(Messages.SuccessAdd);
            }
            return new ErrorResult(Messages.UnSuccessAdd);
        }

        public IResult AddList(CategoryImageDto addCategoryImageDto)
        {
            if (addCategoryImageDto != null && addCategoryImageDto.Files != null && addCategoryImageDto.Files.Count > 0)
            {
                var existing = _categoryImageDal.GetAll(x => x.CategoryId == addCategoryImageDto.CategoryId);
                int startSequence = existing.Count > 0 ? existing.Max(x => x.SequenceNumber) + 1 : 1;
                var list = new List<CategoryImage>();
                for (int i = 0; i < addCategoryImageDto.Files.Count; i++)
                {
                    var categoryImage = new CategoryImage
                    {
                        CategoryId = addCategoryImageDto.CategoryId,
                        CreateDate = DateTime.Now,
                        Path = _fileHelper.Upload(addCategoryImageDto.Files[i], PathConstans.ImagesPath),
                        SequenceNumber = startSequence + i
                    };
                    list.Add(categoryImage);
                }
                _categoryImageDal.AddRange(list);
                return new SuccessResult(Messages.SuccessAdd);
            }
            return new ErrorResult(Messages.UnSuccessAdd);
        }

        public IResult Delete(CategoryImage categoryImage)
        {
            if (categoryImage != null)
            {
                if (!string.IsNullOrEmpty(categoryImage.Path))
                {
                    var fullPath = Path.Combine(PathConstans.ImagesPath, categoryImage.Path);
                    _fileHelper.Delete(fullPath);
                }
                _categoryImageDal.Delete(categoryImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<CategoryImage>> GetAll()
        {
            var result = _categoryImageDal.GetAll();
            if (result != null)
                return new SuccessDataResult<List<CategoryImage>>(result.OrderBy(x => x.SequenceNumber).ToList());
            return new ErrorDataResult<List<CategoryImage>>();
        }

        public IDataResult<List<CategoryImage>> GetAllByCategoryId(int categoryId)
        {
            var result = _categoryImageDal.GetAll(x => x.CategoryId == categoryId);
            if (result != null)
                return new SuccessDataResult<List<CategoryImage>>(result.OrderBy(x => x.SequenceNumber).ToList());
            return new ErrorDataResult<List<CategoryImage>>();
        }

        public IDataResult<CategoryImage> GetById(int id)
        {
            var result = _categoryImageDal.Get(x => x.Id == id);
            if (result != null)
                return new SuccessDataResult<CategoryImage>(result);
            return new ErrorDataResult<CategoryImage>();
        }

        public IResult Update(CategoryImage categoryImage, IFormFile file)
        {
            if (categoryImage != null)
            {
                var existing = _categoryImageDal.Get(x => x.Id == categoryImage.Id);
                if (existing == null)
                    return new ErrorResult();

                existing.CategoryId = categoryImage.CategoryId;
                existing.SequenceNumber = categoryImage.SequenceNumber; // Kullanıcı sırayı kendi belirler
                existing.CreateDate = DateTime.Now;

                if (file != null)
                {
                    var oldFullPath = !string.IsNullOrEmpty(existing.Path)
                        ? Path.Combine(PathConstans.ImagesPath, existing.Path)
                        : null;
                    existing.Path = oldFullPath != null
                        ? _fileHelper.Update(file, oldFullPath, PathConstans.ImagesPath)
                        : _fileHelper.Upload(file, PathConstans.ImagesPath);
                }

                _categoryImageDal.Update(existing);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
