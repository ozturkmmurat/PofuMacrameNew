using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IImageService
    {
        IDataResult<List<Image>> GetAll();
        IDataResult<Image> GetById(int id);
        IDataResult<Image> GetByEntTypeId(int entityTypeId); /*-->EntType --> EntityTypeId*/
        IResult Add(Image image, IFormFile formFile);
        IResult AddList(List<Image> images, IFormFile formFile);
        IResult Update(Image image, IFormFile file);
        IResult Delete(Image image);

        //Check
        IResult CheckIfImageLimit(int entityTypeId);
    }
}
