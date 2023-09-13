using Business.Abstract;
using Business.Constans;
using Core.Business;
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
                var repeatedData = CheckRepeatedData(categoryAttribute);
                var checkAttributeSlicer = CheckSliderAttribute(categoryAttribute);
                IResult result = BusinessRules.Run(repeatedData, checkAttributeSlicer);
                if (result  != null)
                {
                    return new ErrorResult(result.Message);
                }
                _categoryAttributeDal.Add(categoryAttribute);
                return new SuccessResult(Messages.SuccessAdd);
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
            if (result != null)
            {
                return new SuccessDataResult<List<CategoryAttribute>>(result);
            }
            return new ErrorDataResult<List<CategoryAttribute>>();
        }

        public IDataResult<List<CategoryAttribute>> GetAllByCategoryId(int categoryId)
        {
            var result = _categoryAttributeDal.GetAll(x => x.CategoryId == categoryId);
            if (result != null)
            {
                return new SuccessDataResult<List<CategoryAttribute>>(result);
            }
            return new ErrorDataResult<List<CategoryAttribute>>();
        }

        public IDataResult<List<SelectCategoryAttributeDto>> GetAllSlctCategoryByCategoryId(int categoryId)
        {
            var result = _categoryAttributeDal.GetAllSelectFilterDto(x => x.CategoryId == categoryId);
            if (result != null)
            {
                return new SuccessDataResult<List<SelectCategoryAttributeDto>>(result);
            }
            return new ErrorDataResult<List<SelectCategoryAttributeDto>>();
        }

        public IDataResult<List<ViewCategoryAttributeDto>> GetAllDtoTrueSlicer(int categoryId)
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
            var result = _categoryAttributeDal.GetAllFilterDto(x => x.CategoryId == categoryId && x.Slicer == true || x.Attribute == true)
                .GroupBy(x => x.AttributeId).Select(x => x.FirstOrDefault()).ToList();
            if (result != null)
            {
                return new SuccessDataResult<List<ViewCategoryAttributeDto>>(result);
            }
            return new ErrorDataResult<List<ViewCategoryAttributeDto>>();
        }


        public IDataResult<CategoryAttribute> GetByAttributeIdCategoryId(int attributeId, int categoryId)
        {
            var result = _categoryAttributeDal.Get(x => x.AttributeId == attributeId && x.CategoryId == categoryId);
            if (result != null)
            {
                return new SuccessDataResult<CategoryAttribute>(result);
            }
            return new ErrorDataResult<CategoryAttribute>();
        }

        public IResult Update(CategoryAttribute categoryAttribute)
        {
            if (categoryAttribute != null)
            {
                var repeatedData = CheckRepeatedData(categoryAttribute);
                var checkAttributeSlicer = CheckSliderAttribute(categoryAttribute);
                IResult rulesResult = BusinessRules.Run(repeatedData, checkAttributeSlicer);
                if (rulesResult  != null)
                {
                    return new ErrorResult(rulesResult.Message);
                }

                _categoryAttributeDal.Update(categoryAttribute);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult CheckRepeatedData(CategoryAttribute categoryAttribute)
        {
            var result = GetByAttributeIdCategoryId(categoryAttribute.AttributeId, categoryAttribute.CategoryId).Data;
            if (result == null)
            {
                return new SuccessResult();
            }else if (result.Required != categoryAttribute.Required && result.Attribute == categoryAttribute.Attribute && result.Slicer == categoryAttribute.Slicer)
            {
                return new SuccessResult();
            }else if (result.Slicer != categoryAttribute.Slicer)
            {
                return new SuccessResult();
            }
            else if (result.Attribute != categoryAttribute.Attribute)
            {
                return new SuccessResult();
            }

            return new ErrorResult(Messages.RepeatedData);
        }

        public IResult CheckSliderAttribute(CategoryAttribute categoryAttribute)
        {
            if (categoryAttribute != null)
            {
                if (categoryAttribute.Attribute == true && categoryAttribute.Slicer == true)
                {
                    return new ErrorResult(Messages.CheckSliderAttribute);
                }
            }
            return new SuccessResult();
        }


    }
}
