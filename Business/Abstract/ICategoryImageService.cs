using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICategoryImageService
    {
        IDataResult<List<CategoryImage>> GetAll();
        IDataResult<CategoryImage> GetById(int id);
        IResult Add(CategoryImage categoryImage);
        IResult Update(CategoryImage categoryImage);
        IResult Delete(CategoryImage categoryImage);
    }
}
