using Business.Abstract;
using Business.Constans;
using Core.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.CategoryAttribute;
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
            var result = _categoryAttributeDal.GetCategorySlicerAttribute(categoryId);
            if (result != null)
            {
                return new SuccessDataResult<List<ViewCategoryAttributeDto>>(result);
            }
            return new ErrorDataResult<List<ViewCategoryAttributeDto>>();
        }


        public IDataResult<List<CategoryAttribute>> GetByAttributeIds(List<int> attributeIds)
        {
            if (attributeIds == null || !attributeIds.Any())
                return new ErrorDataResult<List<CategoryAttribute>>();
            var result = _categoryAttributeDal.GetAll(x => attributeIds.Contains(x.AttributeId));
            if (result != null)
                return new SuccessDataResult<List<CategoryAttribute>>(result);
            return new ErrorDataResult<List<CategoryAttribute>>();
        }

        public IDataResult<CategoryAttribute> GetByAttributeIdCategoryId(int attributeId, int categoryId)
        {
            var result = _categoryAttributeDal.GetAsNoTracking(x => x.AttributeId == attributeId && x.CategoryId == categoryId);
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

        public IDataResult<List<FilterCategoryAttributeDto>> GetAllCategoryAttributeFilter(int categoryId)
        {
            var result = _categoryAttributeDal.GetAllCategoryAttributeFilter(categoryId);
            if (result != null)
            {
                return new SuccessDataResult<List<FilterCategoryAttributeDto>>(result);
            }
            return new ErrorDataResult<List<FilterCategoryAttributeDto>>();
        }

        public IResult CheckSlicer(CategoryAttribute categoryAttribute)
        {
            var result = GetByCategoryIdSlicer(categoryAttribute.CategoryId, categoryAttribute.Slicer).Data;
            if (result != null)
            {
                return new ErrorResult(Messages.CheckSlicer);
            }
            return new SuccessResult();
        }

        public IDataResult<CategoryAttribute> GetByCategoryIdSlicer(int categoryId, bool slicer)
        {
            var result = _categoryAttributeDal.Get(x => x.CategoryId == categoryId && x.Slicer == slicer);
            if (result != null)
            {
                return new SuccessDataResult<CategoryAttribute>(result);
            }
            return new ErrorDataResult<CategoryAttribute>();
        }

        public IDataResult<List<CategoryAttribute>> GetAllByCategoryIdSlicerAttribute(int categoryId, bool slicer, bool attribute)
        {
            var result = _categoryAttributeDal.GetAllAsNoTracking(x => x.CategoryId == categoryId && x.Slicer == slicer && x.Attribute == attribute);
            if (result != null)
            {
                return new SuccessDataResult<List<CategoryAttribute>>(result);
            }
            return new ErrorDataResult<List<CategoryAttribute>>();
        }

        public IDataResult<List<CategoryAttributeDto>> GetAllCategoryAttribute(CategoryAttributeDto categoryAttributeDto)
        {
            if (categoryAttributeDto.CategoryId == null || !categoryAttributeDto.CategoryId.Any())
            {
                return new ErrorDataResult<List<CategoryAttributeDto>>();
            }
            var result = _categoryAttributeDal.GetAllCategoryAttribute(categoryAttributeDto);
            if (result != null)
            {
                return new SuccessDataResult<List<CategoryAttributeDto>>(result);
            }
            return new ErrorDataResult<List<CategoryAttributeDto>>();
        }
    }
}
