using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.CategoryAttribute.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class CategoryAttributeManager : ICategoryAttributeService
    {
        ICategoryAttributeDal _categoryAttributeDal;
        public CategoryAttributeManager(ICategoryAttributeDal categoryAttributeDal)
        {
            _categoryAttributeDal = categoryAttributeDal;
        }
        public IResult Add(CategoryAttribute categoryAttribute)
        {
            if (categoryAttribute != null)
            {
                _categoryAttributeDal.Add(categoryAttribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(CategoryAttribute categoryAttribute)
        {
            if (categoryAttribute != null)
            {
                _categoryAttributeDal.Delete(categoryAttribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<CategoryAttribute>> GetAll()
        {
            var result = _categoryAttributeDal.GetAll();
            if (result == null)
            {
                return new SuccessDataResult<List<CategoryAttribute>>(result);
            }
            return new ErrorDataResult<List<CategoryAttribute>>();
        }

        public IDataResult<List<ViewCategoryAttributeDto>> GetAllDtoTrueSlicerAttribute(int categoryId)
        {
            var result = _categoryAttributeDal.GetAllTrueSlicerAttribute(categoryId);
            if (result != null)
            {
                return new SuccessDataResult<List<ViewCategoryAttributeDto>>(result);
            }
            return new ErrorDataResult<List<ViewCategoryAttributeDto>>();
        }

        public IDataResult<List<ViewCategoryAttributeDto>> GetAllViewDtoTrueSlicerAttribute(int categoryId)
        {
            var result = _categoryAttributeDal.GetAllFilterDto(x => x.CategoryId == categoryId && x.Slicer == true || x.Attribute == true);
            if (result != null)
            {
                return new SuccessDataResult<List<ViewCategoryAttributeDto>>(result);
            }
            return new ErrorDataResult<List<ViewCategoryAttributeDto>>();
        }

        public IResult Update(CategoryAttribute categoryAttribute)
        {
            if (categoryAttribute != null)
            {
                _categoryAttributeDal.Update(categoryAttribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
