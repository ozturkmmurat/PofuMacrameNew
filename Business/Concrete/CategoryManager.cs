using Business.Abstract;
using Business.Constans;
using Core.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.Category.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;
        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public IResult Add(Category category)
        {
            if (category != null)
            {
                _categoryDal.Add(category);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(Category category)
        {
            if (category != null)
            {
                _categoryDal.Delete(category);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<Category>> GetAll()
        {
            var result = _categoryDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<Category>>(result);
            }
            return new ErrorDataResult<List<Category>>();
        }

        public IDataResult<List<Category>> GetAllAsNoTracking()
        {
            var result = _categoryDal.GetAllAsNoTracking();
            if (result != null)
            {
                return new SuccessDataResult<List<Category>>(result);
            }
            return new ErrorDataResult<List<Category>>();
        }

        public IDataResult<List<SelectCategoryDto>> GetAllCategoryHierarchy()
        {
            var categories = GetAllAsNoTracking();
            if (categories != null)
            {
                var hierarchyCategories = _categoryDal.GetAllCategoryHierarchy(categories.Data, 0);
                if (hierarchyCategories != null)
                {
                    return new SuccessDataResult<List<SelectCategoryDto>>(hierarchyCategories);
                }
            }
            return new ErrorDataResult<List<SelectCategoryDto>>();
        }

        public IDataResult<List<Category>> GetAllSubCategory(int categoryId)
        {
            var result = _categoryDal.GetAllAsNoTracking(x => x.ParentId == categoryId);
            if (result?.Count > 0)
            {
                return new SuccessDataResult<List<Category>>(result);
            }
            return new ErrorDataResult<List<Category>>();
        }

        public IDataResult<Category> GetById(int id)
        {
            var result = _categoryDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<Category>(result);
            }
            return new ErrorDataResult<Category>();
        }

        public IResult Update(Category category)
        {
            if (category != null)
            {
                _categoryDal.Update(category);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
