using Business.Abstract;
using Business.Constans;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.ProductCategory;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class ProductCategoryManager : IProductCategoryService
    {
        private readonly IProductCategoryDal _productCategoryDal;

        public ProductCategoryManager(IProductCategoryDal productCategoryDal)
        {
            _productCategoryDal = productCategoryDal;
        }

        /// <summary>
        /// MainCategoryId > 0 ise ana kategori satırı (CategoryId=0, MainCategoryId=değer) eklenir.
        /// CategoryId listesi varsa ek kategoriler (CategoryId=id, MainCategoryId=0) eklenir. MainCategoryId zorunlu değil.
        /// </summary>
        public IResult AddProductCategories(ProductCategoryDto productCategoryDto)
        {
            if (productCategoryDto == null)
                return new ErrorResult(Messages.DataRuleFail);

            if (productCategoryDto.ProductId <= 0)
                return new ErrorResult(Messages.DataRuleFail);

            var toInsert = new List<ProductCategory>();

            if (productCategoryDto.MainCategoryId > 0)
            {
                toInsert.Add(new ProductCategory
                {
                    ProductId = productCategoryDto.ProductId,
                    CategoryId = 0,
                    MainCategoryId = productCategoryDto.MainCategoryId
                });
            }

            if (productCategoryDto.CategoryId != null && productCategoryDto.CategoryId.Count > 0)
            {
                var mainId = productCategoryDto.MainCategoryId;
                var extraItems = productCategoryDto.CategoryId
                    .Where(x => x > 0 && x != mainId)
                    .Distinct()
                    .Select(categoryId => new ProductCategory
                    {
                        ProductId = productCategoryDto.ProductId,
                        CategoryId = categoryId,
                        MainCategoryId = 0
                    })
                    .ToList();
                toInsert.AddRange(extraItems);
            }

            if (toInsert.Count > 0)
                _productCategoryDal.AddRange(toInsert);
            return new SuccessResult();
        }

        public IResult Add(ProductCategory productCategory)
        {
            if (productCategory == null)
                return new ErrorResult(Messages.DataRuleFail);
            _productCategoryDal.Add(productCategory);
            return new SuccessResult();
        }

        public IResult Delete(ProductCategory productCategory)
        {
            if (productCategory == null)
                return new ErrorResult(Messages.DataRuleFail);
            _productCategoryDal.Delete(productCategory);
            return new SuccessResult();
        }

        public IResult DeleteByProductId(int productId)
        {
            if (productId <= 0)
                return new ErrorResult(Messages.DataRuleFail);
            var list = _productCategoryDal.GetAll(x => x.ProductId == productId);
            if (list != null && list.Count > 0)
                _productCategoryDal.DeleteRange(list);
            return new SuccessResult();
        }

        public IResult DeleteExtraByProductId(int productId)
        {
            if (productId <= 0)
                return new ErrorResult(Messages.DataRuleFail);
            var list = _productCategoryDal.GetAll(x => x.ProductId == productId && x.MainCategoryId == 0);
            if (list != null && list.Count > 0)
                _productCategoryDal.DeleteRange(list);
            return new SuccessResult();
        }

        public IDataResult<List<ProductCategory>> GetAll()
        {
            var result = _productCategoryDal.GetAll();
            if (result != null)
                return new SuccessDataResult<List<ProductCategory>>(result);
            return new ErrorDataResult<List<ProductCategory>>(Messages.UnSuccessGet);
        }

        public IDataResult<List<ProductCategory>> GetAllAsNoTracking()
        {
            var result = _productCategoryDal.GetAllAsNoTracking();
            if (result != null)
                return new SuccessDataResult<List<ProductCategory>>(result);
            return new ErrorDataResult<List<ProductCategory>>(Messages.UnSuccessGet);
        }

        public IDataResult<ProductCategory> GetById(int id)
        {
            var result = _productCategoryDal.Get(x => x.Id == id);
            if (result != null)
                return new SuccessDataResult<ProductCategory>(result);
            return new ErrorDataResult<ProductCategory>(Messages.UnSuccessGet);
        }

        /// <summary>
        /// İlgili ürünün ana kategori kaydı (CategoryId == 0; MainCategoryId ana kategorinin id'si).
        /// </summary>
        public IDataResult<ProductCategory> GetByProductMainCategory(int productId)
        {
            if (productId <= 0)
                return new ErrorDataResult<ProductCategory>(Messages.DataRuleFail);
            var result = _productCategoryDal.GetAsNoTracking(x => x.ProductId == productId && x.CategoryId == 0);
            if (result != null)
                return new SuccessDataResult<ProductCategory>(result);
            return new ErrorDataResult<ProductCategory>(Messages.UnSuccessGet);
        }

        public IDataResult<List<ProductCategory>> GetAllByProductIdAsNoTracking(int productId)
        {
            if (productId <= 0)
                return new ErrorDataResult<List<ProductCategory>>(Messages.DataRuleFail);
            var result = _productCategoryDal.GetAllAsNoTracking(x => x.ProductId == productId);
            if (result != null)
                return new SuccessDataResult<List<ProductCategory>>(result);
            return new ErrorDataResult<List<ProductCategory>>(Messages.UnSuccessGet);
        }

        public IResult Update(ProductCategory productCategory)
        {
            if (productCategory == null)
                return new ErrorResult(Messages.DataRuleFail);
            _productCategoryDal.Update(productCategory);
            return new SuccessResult();
        }
    }
}
