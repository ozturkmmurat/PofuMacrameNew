using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.Product.Select;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Core.Utilities.Result.Concrete;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using Entities.EntitiyParameter.Product;
using Entities.EntityParameter.Product;
using System.Xml.Linq;
using System.Collections;
using Core.Entities;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, PofuMacrameContext>, IProductDal
    {
        private readonly PofuMacrameContext _context;
        public EfProductDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }

        public List<SelectListProductVariantDto> GetAllPvFilterDto(FilterProduct filterProduct)
        {
            if (filterProduct.CategoryId == 0)
            {
                var result = from p in _context.Products.AsNoTracking().Skip(filterProduct.StartLength).Take(filterProduct.EndLength)
                             join pv in _context.ProductVariants.AsNoTracking() on p.Id equals pv.ProductId
                             where pv.ParentId == 0
                             select new SelectListProductVariantDto
                             {
                                 ProductId = p.Id,
                                 ProductVariantId = pv.Id,
                                 ParentId = pv.ParentId,
                                 ProductName = p.ProductName,
                                 Description = p.Description,
                                 AttributeValues = _context.AttributeValues.DefaultIfEmpty().AsNoTracking().Where(x => x.Id == pv.AttributeValueId).ToList(),
                                 ProductPaths = _context.ProductImages.AsNoTracking()
                                                     .Where(x => x.ProductVariantId == pv.Id)
                                                     .Take(2)
                                                     .Select(pi => pi.Path)
                                                     .ToList(),
                             };
                return result.ToList();

            }
            else if (filterProduct.CategoryId > 0 && filterProduct.Attributes.Count == 0)
            {
                var result = from p in _context.Products.AsNoTracking().Where(x => x.CategoryId == filterProduct.CategoryId).Skip(filterProduct.StartLength).Take(filterProduct.EndLength)
                             join pv in _context.ProductVariants.AsNoTracking() on p.Id equals pv.ProductId
                             where pv.ParentId == 0
                             select new SelectListProductVariantDto
                             {
                                 ProductId = p.Id,
                                 ProductVariantId = pv.Id,
                                 ParentId = pv.ParentId,
                                 ProductName = p.ProductName,
                                 Description = p.Description,
                                 AttributeValues = _context.AttributeValues.AsNoTracking().Where(x => x.Id == pv.AttributeValueId).ToList(),
                                 ProductPaths = _context.ProductImages
                                                     .Where(x => x.ProductVariantId == pv.Id)
                                                     .Take(2)
                                                     .Select(pi => pi.Path)
                                                     .ToList(),
                             };
                return result.ToList();

            }
            else if (filterProduct.CategoryId > 0 && filterProduct.Attributes.Count > 0)
            {
                var products = _context.Products.AsNoTracking()
                                .Where(x => x.CategoryId == filterProduct.CategoryId)
                                .ToList();

                var productIds = products.Select(p => p.Id).ToList();

                var productAttributes = _context.ProductAttributes.AsNoTracking()
                    .Where(variant => productIds.Contains(variant.ProductId))
                    .ToList();

                var filteredProducts = products.Where(product =>
                {
                    var productAttributesForProduct = productAttributes.Where(pa => pa.ProductId == product.Id).ToList();

                    return filterProduct.Attributes.All(filterAttribute =>
                    {
                        var matchingProductAttributes = productAttributesForProduct.Where(pa => pa.AttributeId == filterAttribute.Id).ToList();

                        return matchingProductAttributes.Any(pa => filterAttribute.ValueId.Contains(pa.AttributeValueId));
                    });
                }).ToList();

                var categoryAttributes = _context.CategoryAttributes.AsNoTracking().Where(x => x.CategoryId == filterProduct.CategoryId);
                if (categoryAttributes != null)
                {
                    if (categoryAttributes.Any())
                    {
                        var attributeIds = filterProduct.Attributes.Where(x => categoryAttributes.Any(a => a.AttributeId == x.Id)).Select(x => x.Id).ToList();

                        // Ardından bu AttributeId'leri kullanarak ValueId'leri bul
                        var valueIds = filterProduct.Attributes.Where(x => attributeIds.Contains(x.Id)).SelectMany(x => x.ValueId).ToList();

                        // Son olarak, bu ValueId'leri kullanarak ProductVariants içerisindeki eşleşmeleri bul
                        var slicerProduct = _context.ProductVariants.AsNoTracking().Where(x => x.AttributeValueId.HasValue && valueIds.Contains(x.AttributeValueId.Value) && productIds.Contains(x.ProductId)).ToList();

                        if (slicerProduct != null)
                        {
                            var mainProductResult = from p in filteredProducts
                                                    join spv in slicerProduct
                                                    on p.Id equals spv.ProductId
                                                    select new SelectListProductVariantDto
                                                    {
                                                        ProductId = p.Id,
                                                        ProductVariantId = spv?.Id ?? 0,
                                                        ParentId = spv.ParentId ?? 0,
                                                        ProductName = p.ProductName,
                                                        Description = p.Description,
                                                        AttributeValues = spv != null ? _context.AttributeValues.AsNoTracking().Where(x => x.Id == spv.AttributeValueId).ToList() : new List<AttributeValue>(),
                                                        ProductPaths = spv != null ? _context.ProductImages.AsNoTracking()
                                                                                             .Where(x => x.ProductVariantId == spv.Id)
                                                                                             .Take(2)
                                                                                             .Select(pi => pi.Path)
                                                                                             .ToList() : new List<string>()
                                                    };

                            return mainProductResult.ToList();
                        }
                    }

                    var result = from p in filteredProducts
                                 join pv in _context.ProductVariants.AsNoTracking()
                                 on p.Id equals pv?.ProductId
                                 select new SelectListProductVariantDto
                                 {
                                     ProductId = p.Id,
                                     ProductVariantId = pv?.Id ?? 0,
                                     ParentId = pv.ParentId ?? 0,
                                     ProductName = p.ProductName,
                                     Description = p.Description,
                                     AttributeValues = pv != null ? _context.AttributeValues.AsNoTracking().Where(x => x.Id == pv.AttributeValueId).ToList() : new List<AttributeValue>(),
                                     ProductPaths = pv != null ? _context.ProductImages.AsNoTracking()
                                                                .Where(x => x.ProductVariantId == pv.Id)
                                                                .Take(2)
                                                                .Select(pi => pi.Path)
                                                                .ToList() : new List<string>()
                                 };

                    return result.ToList();
                }
            }
            return null;
        }


        public SelectProductDetailDto GetFilterDto(Expression<Func<SelectProductDetailDto, bool>> filter = null)
        {
            var result = (from p in _context.Products
                          join pv in _context.ProductVariants on p.Id equals pv.ProductId
                          join ps in _context.ProductStocks on p.Id equals ps.ProductId
                          join pi in _context.ProductImages on pv.Id equals pi.ProductVariantId
                          where pi.IsMain == true
                          select new SelectProductDetailDto
                          {
                              ProductId = p.Id,
                              ProductVariantId = pv.Id,
                              ParentId = pv.ParentId,
                              ProductName = p.ProductName,
                              Description = p.Description,
                              StockCode = ps.StockCode,
                              Price = ps.Price,
                              Quantity = ps.Quantity,
                              MainImage = pi.Path,
                              ProductPaths = _context.ProductImages
                                                    .Where(x => x.ProductVariantId == pv.Id)
                                                    .Select(x => x.Path).ToList(),
                              VariantPaths = _context.ProductImages
                                                    .Where(x => x.ProductId == p.Id && x.IsMain == true)
                                                    .Select(x => x.Path).ToList()
                          });


            return filter == null ? result.FirstOrDefault() : result.Where(filter.Compile()).FirstOrDefault();
        }

        public List<SelectProductDto> GetAllFilterDto(Expression<Func<SelectProductDto, bool>> filter = null)
        {
            var result = (from p in _context.Products
                          join c in _context.Categories on p.CategoryId equals c.Id

                          select new SelectProductDto
                          {
                              ProductId = p.Id,
                              CategoryId = c.Id,
                              ProductName = p.ProductName,
                              CategoryName = c.CategoryName,
                              ProductCode = p.ProductCode
                          });

            return filter == null ? result.ToList() : result.Where(filter).ToList();
        }

        public SelectProductDto GetProductFilterDto(Expression<Func<SelectProductDto, bool>> filter = null)
        {
            var result = (from p in _context.Products
                          join c in _context.Categories on p.CategoryId equals c.Id

                          select new SelectProductDto
                          {
                              ProductId = p.Id,
                              CategoryId = c.Id,
                              ProductName = p.ProductName,
                              CategoryName = c.CategoryName,
                              ProductCode = p.ProductCode,
                              Description = p.Description,
                          });

            return filter == null ? result.FirstOrDefault() : result.Where(filter).FirstOrDefault();
        }

        public int GetTotalProduct(int categoryId)
        {
            if (categoryId != 0)
            {
                var result = _context.Products.Where(x => x.CategoryId == categoryId).Count();
                return result;
            }
            else
            {
                var result = _context.Products.Count();
                return result;
            }
        }

        public Tuple<List<ProductVariant>, List<ProductAttribute?>> ApplyFilteres(FilterProduct filterProduct)
        {
            var products = _context.Products.AsNoTracking()  //Kategoriye gore urunleri cek
                                           .Where(x => x.CategoryId == filterProduct.CategoryId)
                                           .ToList();

            var productIds = products.Select(p => p.Id).ToList(); //Cekilen urunlerin idlerini al

            var productAttributes = _context.ProductAttributes.AsNoTracking() // Bu urunlerin ozelliklerini al
                .Where(variant => productIds.Contains(variant.ProductId))
                .ToList();

            var categoryAttributes = _context.CategoryAttributes.AsNoTracking().Where(x => x.CategoryId == filterProduct.CategoryId); //İlgili kategorideki hangi attributeler ana grup alt grup ve normal ozellik onları kontrol icin cek
            if (categoryAttributes != null)
            {
                if (categoryAttributes.Any())
                {

                    // CategoryAttribute listesinden Slicer ve AttributeValue değeri false olanları filtrele
                    var filteredIdsFalse = categoryAttributes //Duz attributeleri al cinsiyet gibi urun detay da gozukmeyen
                        .Where(ca => !ca.Slicer && !ca.Attribute)
                        .Select(ca => ca.AttributeId).ToList();

                    var filteredIdsTrue = categoryAttributes //Urun detay da gozuekn attributeleri al
                        .Where(ca => ca.Slicer || ca.Attribute)
                        .Select(ca => ca.AttributeId).ToList();

                    var falseCheck = productAttributes.Where(x => filteredIdsFalse.Any(y => x.AttributeId == y)).ToList(); // Urun ozelliklerinde hangileri duz ozellik onları bul

                    var filteredAttributeFalseProduct = filterProduct.Attributes.Where(x => falseCheck.Any(y => y.AttributeId == x.Id)).SelectMany(x => x.ValueId).ToList(); //paramtrede duz ozellik secilmis mi  var ise valueIdlerini al

                    var filteredEnd = productAttributes.Where(x => filteredAttributeFalseProduct.Contains(x.AttributeValueId)).ToList(); //Duz ozelliklere gore filtrele

                    var trueCheck = productAttributes.Where(x => filteredIdsTrue.Any(y => x.AttributeId == y)).ToList(); // Urun ozelliklerinde hangileri duz ozellik onları bul

                    var filteredAttributeTrueProduct = filterProduct.Attributes.Where(x => trueCheck.Any(y => y.AttributeId == x.Id)).SelectMany(x => x.ValueId).ToList(); //paramtrede duz ozellik secilmis mi  var ise valueIdlerini al

                    var filteredEndTrue = productAttributes.Where(x => filteredAttributeTrueProduct.Contains(x.AttributeValueId)).ToList(); //Duz ozelliklere gore filtrele

                    if (filteredEnd?.Count > 0) //Duz ozellik secilmis ise bura calissin
                    {
                        var filteredAttributeFalse = filterProduct.Attributes
                         .Where(fa => filteredIdsFalse.Contains(fa.Id))
                         .ToList();

                        var getFilteredAttributeFalseId = filteredAttributeFalse.SelectMany(x => x.ValueId).ToList();

                        var filterProductAttributes = productAttributes.Where(x => getFilteredAttributeFalseId.Contains(x.AttributeValueId));

                        var filteredProductAttributesId = filterProductAttributes.Select(x => x.ProductId);

                        var productVariantFalse = _context.ProductVariants.Where(x => x.ParentId == 0 && filteredProductAttributesId.Contains(x.ProductId)).ToList();

                        return Tuple.Create(productVariantFalse, filteredEnd);
                    }
                    else if(filteredAttributeFalseProduct?.Count == 0 && filteredEndTrue?.Count > 0) //Duz ozellik secilmemis ise sadece urun detay daki ozellikler secilmis ise burasi calissin
                    {
                        var trueCheckFilter = productAttributes.Where(x => filteredIdsTrue.Any(y => y == x.AttributeId)).Select(t => t.AttributeValueId).Distinct().ToList();
                        var test = _context.ProductVariants.Where(x => productIds.Contains(x.ProductId)).ToList();
                        var filteredAttributeTrue = test.Where(x => trueCheckFilter.Any(y => x.AttributeValueId == y)).ToList();
                        filteredEnd = null;
                       return Tuple.Create(filteredAttributeTrue, filteredEnd);
                    }
                    else if (filteredEnd?.Count == 0)
                    {

                    }

                    
                }

            }
            return null;
        }

        public List<SelectListProductVariantDto> ExecuteFilteres(List<SelectListProductVariantDto> filterProducts)
        {
            var result = from pv in filterProducts
                         join p in _context.Products.AsNoTracking()
                         on pv.ProductId equals p.Id
                         select new SelectListProductVariantDto
                         {
                             ProductId = p.Id,
                             CategoryId = p.CategoryId,
                             ProductVariantId = pv?.ProductVariantId ?? 0,
                             ParentId = pv.ParentId ?? 0,
                             ProductName = p.ProductName,
                             Description = p.Description,
                             AttributeValues = pv != null ? _context.AttributeValues.AsNoTracking()
                                                   .Where(av => pv.AttributeValues.Select(a => a.Id).Contains(av.Id))
                                                   .ToList() : new List<AttributeValue>(),
                             ProductPaths = pv != null ? _context.ProductImages.AsNoTracking()
                                                              .Where(x => x.ProductVariantId == pv.ProductId)
                                                              .Take(2)
                                                              .Select(pi => pi.Path)
                                                              .ToList() : new List<string>()
                         };

            return result.ToList();
        }

        public List<SelectListProductVariantDto> DefaultOnNoFilter(FilterProduct filterProduct)
        {
            if (filterProduct.CategoryId > 0 && filterProduct.Attributes.Count == 0)
            {
                var result = from p in _context.Products.AsNoTracking().Where(x => x.CategoryId == filterProduct.CategoryId)
                             join pv in _context.ProductVariants.AsNoTracking() on p.Id equals pv.ProductId
                             where pv.ParentId == 0
                             select new SelectListProductVariantDto
                             {
                                 ProductId = p.Id,
                                 ProductVariantId = pv.Id,
                                 ParentId = pv.ParentId,
                                 ProductName = p.ProductName,
                                 Description = p.Description,
                                 AttributeValues = _context.AttributeValues.AsNoTracking().Where(x => x.Id == pv.AttributeValueId).ToList(),
                                 ProductPaths = _context.ProductImages
                                                     .Where(x => x.ProductVariantId == pv.Id)
                                                     .Take(2)
                                                     .Select(pi => pi.Path)
                                                     .ToList(),
                             };
                return result.ToList();

            }
            return null;
        }

        public List<SelectListProductVariantDto> RandomDefaultOnNoFilter(FilterProduct filterProduct)
        {
            if (filterProduct.CategoryId == 0 && filterProduct.Attributes.Count == 0)
            {
                var result = from p in _context.Products.AsNoTracking().Skip(filterProduct.StartLength).Take(filterProduct.EndLength)
                             join pv in _context.ProductVariants.AsNoTracking() on p.Id equals pv.ProductId
                             where pv.ParentId == 0
                             select new SelectListProductVariantDto
                             {
                                 ProductId = p.Id,
                                 ProductVariantId = pv.Id,
                                 ParentId = pv.ParentId,
                                 ProductName = p.ProductName,
                                 Description = p.Description,
                                 AttributeValues = _context.AttributeValues.DefaultIfEmpty().AsNoTracking().Where(x => x.Id == pv.AttributeValueId).ToList(),
                                 ProductPaths = _context.ProductImages.AsNoTracking()
                                                     .Where(x => x.ProductVariantId == pv.Id)
                                                     .Take(2)
                                                     .Select(pi => pi.Path)
                                                     .ToList(),
                             };
                return result.ToList();

            }
            return null;

        }
    }
}

