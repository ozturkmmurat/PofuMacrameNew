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
    public class EfVariantDal : EfEntityRepositoryBase<Variant, PofuMacrameContext>, IVariantDal
    {
        public List<VariantDto> GetAllFilterDto(Expression<Func<VariantDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from v in context.Variants
                             join p in context.Products
                             on v.ProductId equals p.Id
                             join pa in context.ProductAttributes
                             on p.Id equals pa.ProductId
                             join av in context.AttributeValues
                             on pa.AttributeValueId equals av.AttributeId
                             join a in context.Attributes
                             on pa.AttributeId equals a.Id

                             select new VariantDto
                             {
                                 ProductId = p.Id,
                                 ProductName = p.ProductName,
                                 AttrName = a.Name,
                                 AttrValue = av.Value,
                                 AttrtCode = av.Code
                             };
                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }
    }
}
