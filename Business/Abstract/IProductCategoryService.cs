using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos.ProductCategory;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IProductCategoryService
    {
        /// <summary>
        /// Ürüne ana kategori (MainCategoryId) ve ek kategorileri (CategoryIds) ekler.
        /// </summary>
        IResult AddProductCategories(ProductCategoryDto productCategoryDto);
        IDataResult<List<ProductCategory>> GetAll();
        IDataResult<List<ProductCategory>> GetAllAsNoTracking();
        IDataResult<ProductCategory> GetById(int id);
        /// <summary>
        /// İlgili ürünün ana kategori kaydını getirir (CategoryId == 0 olan satır; MainCategoryId ana kategorinin id'sidir).
        /// </summary>
        IDataResult<ProductCategory> GetByProductMainCategory(int productId);
        IDataResult<List<ProductCategory>> GetAllByProductIdAsNoTracking(int productId);
        IResult Add(ProductCategory productCategory);
        IResult Update(ProductCategory productCategory);
        IResult Delete(ProductCategory productCategory);
        /// <summary>
        /// Ürüne ait tüm ProductCategory kayıtlarını siler.
        /// </summary>
        IResult DeleteByProductId(int productId);
        /// <summary>
        /// Ürüne ait sadece ek kategorileri (MainCategoryId == 0) siler. Ana kategori korunur.
        /// </summary>
        IResult DeleteExtraByProductId(int productId);
    }
}
