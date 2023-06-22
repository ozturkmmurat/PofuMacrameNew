using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.Product.Select;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, PofuMacrameContext>, IProductDal
    {
        public List<SelectProductDto> GetAllProductAndVariant()
        {
            using (PofuMacrameContext dbContext = new PofuMacrameContext())
            {
                var query = (
                    from variants in dbContext.Variants
                    join products in dbContext.Products on variants.ProductId equals products.Id
                    join firstImages in (
                        from images in dbContext.Images
                        group images by images.EntityId into g
                        select new { EntityId = g.Key, MinImageId = g.Min(x => x.Id) }
                    ) on variants.Id equals firstImages.EntityId into firstImagesGroup
                    from firstImages in firstImagesGroup.DefaultIfEmpty()
                    join images in dbContext.Images on firstImages.MinImageId equals images.Id into imagesGroup
                    from images in imagesGroup.DefaultIfEmpty()
                    join entityTypes in dbContext.EntityTypes on images.EntityTypeId equals entityTypes.Id into entityTypesGroup
                    from entityTypes in entityTypesGroup.DefaultIfEmpty()
                    join productStocks in dbContext.ProductStocks
                        on new { VariantId = (int?)variants.Id, variants.ProductId } equals new { VariantId = (int?)productStocks.VariantId, productStocks.ProductId } into productStocksGroup
                    from productStocks in productStocksGroup.DefaultIfEmpty()
                    select new SelectProductDto
                    {
                        ProductId = products.Id,
                        CategoryId = products.CategoryId,
                        VariantId = variants.Id,
                        Title = products.Title,
                        Description = products.Description,
                        ImagePath = images.ImagePath,
                        Price = productStocks.Price,
                        Quantity = productStocks.Quantity
                    }
                )
                .Union(
                    from products in dbContext.Products
                    join productStocks in dbContext.ProductStocks on products.Id equals productStocks.ProductId into productStocksGroup
                    from productStocks in productStocksGroup.DefaultIfEmpty()
                    join firstImages in (
                        from images in dbContext.Images
                        group images by images.EntityId into g
                        select new { EntityId = g.Key, MinImageId = g.Min(x => x.Id) }
                    ) on products.Id equals firstImages.EntityId into firstImagesGroup
                    from firstImages in firstImagesGroup
                    join images in dbContext.Images on firstImages.MinImageId equals images.Id into imagesGroup
                    from images in imagesGroup
                    join entityTypes in dbContext.EntityTypes on images.EntityTypeId equals entityTypes.Id into entityTypesGroup
                    from entityTypes in entityTypesGroup
                    where productStocks.VariantId == null
                    select new SelectProductDto
                    {
                        ProductId = products.Id,
                        CategoryId = products.CategoryId,
                        VariantId = null,
                        Title = products.Title,
                        Description = products.Description,
                        ImagePath = images.ImagePath,
                        Price = productStocks.Price,
                        Quantity = productStocks.Quantity
                    }
                );

                var result = query.ToList();

                return result;
            }


        }
    }
}
