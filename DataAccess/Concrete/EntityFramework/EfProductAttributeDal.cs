using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductAttributeDal : EfEntityRepositoryBase<ProductAttribute, PofuMacrameContext>, IProductAttributeDal
    {
        public List<ProductAttributeDto> GetAllFilterDto(Expression<Func<ProductAttributeDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from pa in context.ProductAttributes
                             join p in context.Products
                             on pa.ProductId equals p.Id
                             join a in context.Attributes
                             on pa.AttributeId equals a.Id


                             select new ProductAttributeDto
                             {
                                 ProductId = p.Id,
                                 ProductName = p.ProductName,
                                 AttributeName = a.Name
                             };
                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }
    }
}
