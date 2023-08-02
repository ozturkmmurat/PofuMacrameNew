using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CategoryImageManager : ICategoryImageService
    {
        ICategoryImageDal _categoryImageDal;
        public CategoryImageManager(ICategoryImageDal categoryImageDal)
        {
            _categoryImageDal = categoryImageDal;
        }
        public IResult Add(CategoryImage categoryImage)
        {
            if (categoryImage != null)
            {
                _categoryImageDal.Add(categoryImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(CategoryImage categoryImage)
        {
            if (categoryImage != null)
            {
                _categoryImageDal.Delete(categoryImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<CategoryImage>> GetAll()
        {
            var result = _categoryImageDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<CategoryImage>>(result);
            }
            return new ErrorDataResult<List<CategoryImage>>();
        }

        public IDataResult<CategoryImage> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IResult Update(CategoryImage categoryImage)
        {
            if (categoryImage != null)
            {
                _categoryImageDal.Update(categoryImage);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
