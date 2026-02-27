using Business.Abstract;
using Business.Constans;
using Core.Helpers.FileHelper;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Business.Concrete
{
    public class SiteContentManager : ISiteContentService
    {
        private readonly ISiteContentDal _siteContentDal;
        private readonly IFileHelper _fileHelper;

        public SiteContentManager(ISiteContentDal siteContentDal, IFileHelper fileHelper)
        {
            _siteContentDal = siteContentDal;
            _fileHelper = fileHelper;
        }

        public IResult Add(SiteContent siteContent, IFormFile file)
        {
            if (siteContent == null)
                return new ErrorResult(Messages.UnSuccessAdd);

            if (file != null)
                siteContent.ImageUrl = _fileHelper.Upload(file, PathConstans.ImagesPath);
            else
                siteContent.ImageUrl = siteContent?.ImageUrl ?? string.Empty;

            _siteContentDal.Add(siteContent);
            return new SuccessResult(Messages.SuccessAdd);
        }

        public IResult Update(SiteContent siteContent, IFormFile file)
        {
            if (siteContent == null)
                return new ErrorResult(Messages.UnSuccessUpdate);

            var existing = _siteContentDal.Get(x => x.Id == siteContent.Id);
            if (existing == null)
                return new ErrorResult(Messages.UnSuccessUpdate);

            existing.ContentKey = siteContent.ContentKey;
            existing.Title = siteContent.Title;
            existing.Description = siteContent.Description;
            existing.LinkUrl = siteContent.LinkUrl;
            existing.DisplayOrder = siteContent.DisplayOrder;
            existing.Status = siteContent.Status;

            if (file != null)
            {
                if (!string.IsNullOrEmpty(existing.ImageUrl))
                {
                    var oldFullPath = Path.Combine(PathConstans.ImagesPath, existing.ImageUrl);
                    existing.ImageUrl = _fileHelper.Update(file, oldFullPath, PathConstans.ImagesPath);
                }
                else
                {
                    existing.ImageUrl = _fileHelper.Upload(file, PathConstans.ImagesPath);
                }
            }
            else if (!string.IsNullOrEmpty(siteContent.ImageUrl))
            {
                existing.ImageUrl = siteContent.ImageUrl;
            }

            _siteContentDal.Update(existing);
            return new SuccessResult(Messages.SuccessUpdate);
        }

        public IResult Delete(SiteContent siteContent)
        {
            if (siteContent == null)
                return new ErrorResult(Messages.UnSuccessDelete);

            var entity = _siteContentDal.Get(x => x.Id == siteContent.Id);
            if (entity == null)
                return new ErrorResult(Messages.UnSuccessDelete);

            if (!string.IsNullOrEmpty(entity.ImageUrl))
            {
                var fullPath = Path.Combine(PathConstans.ImagesPath, entity.ImageUrl);
                _fileHelper.Delete(fullPath);
            }

            _siteContentDal.Delete(entity);
            return new SuccessResult(Messages.SuccessDelete);
        }

        public IDataResult<List<SiteContent>> GetAll()
        {
            var result = _siteContentDal.GetAllAsNoTracking();
            if (result != null)
                return new SuccessDataResult<List<SiteContent>>(result);
            return new ErrorDataResult<List<SiteContent>>(Messages.UnSuccessGet);
        }

        public IDataResult<SiteContent> GetById(int id)
        {
            var result = _siteContentDal.GetAsNoTracking(x => x.Id == id);
            if (result != null)
                return new SuccessDataResult<SiteContent>(result);
            return new ErrorDataResult<SiteContent>(Messages.UnSuccessGet);
        }

        public IDataResult<List<SiteContent>> GetAllByContentKey(string contentKey)
        {
            var result = _siteContentDal.GetAllAsNoTracking(x => x.ContentKey == contentKey).OrderBy(x => x.DisplayOrder).ToList();
            if (result != null)
                return new SuccessDataResult<List<SiteContent>>(result);

            return new ErrorDataResult<List<SiteContent>>(Messages.UnSuccessGet);
        }
    }
}
