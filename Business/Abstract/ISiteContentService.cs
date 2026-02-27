using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ISiteContentService
    {
        IDataResult<List<SiteContent>> GetAll();
        IDataResult<SiteContent> GetById(int id);
        IDataResult<List<SiteContent>> GetAllByContentKey(string contentKey);
        IResult Add(SiteContent siteContent, IFormFile file);
        IResult Update(SiteContent siteContent, IFormFile file);
        IResult Delete(SiteContent siteContent);
    }
}
