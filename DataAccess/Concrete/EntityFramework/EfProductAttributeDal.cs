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
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductAttributeDal : EfEntityRepositoryBase<ProductAttribute, PofuMacrameContext>, IProductAttributeDal
    {
        private readonly PofuMacrameContext _context;
        public EfProductAttributeDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }

        public List<ProductAttribute> GetAllProductIdListNT(List<int> ids)
        {
            var result = _context.ProductAttributes.AsNoTracking().Where(x => ids.Contains(x.ProductId)).ToList();
            return result;
        }

        public List<ProductAttributeDto> GetProductVariantAttribute(Expression<Func<ProductAttributeDto, bool>> filter = null)
        {
            var result = from pa in _context.ProductAttributes
                         join p in _context.Products
                         on pa.ProductId equals p.Id
                         join a in _context.Attributes
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
