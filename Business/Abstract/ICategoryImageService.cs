using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.CategoryImage;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ICategoryImageService
    {
        IDataResult<List<CategoryImage>> GetAll();
        IDataResult<List<CategoryImage>> GetAllByCategoryId(int categoryId);
        IDataResult<CategoryImage> GetById(int id);
        IResult Add(CategoryImage categoryImage, IFormFile file);
        IResult AddList(CategoryImageDto addCategoryImageDto);
        IResult Update(CategoryImage categoryImage, IFormFile file);
        IResult Delete(CategoryImage categoryImage);
    }
}
