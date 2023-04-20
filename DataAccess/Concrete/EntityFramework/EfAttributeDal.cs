using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfAttributeDal : EfEntityRepositoryBase<Entities.Concrete.Attribute, PofuMacrameContext>, IAttributeDal
    {
        public List<AttributeDto> GetAllFilterDto(Expression<Func<AttributeDto, bool>> filter = null)
        {
            using (PofuMacrameContext context = new PofuMacrameContext())
            {
                var result = from a in context.Attributes
                             join av in context.AttributeValues
                             on a.Id equals av.AttributeId

                             select new AttributeDto
                             {
                                 AttributeId = a.Id,
                                 AttributeValueId = av.Id,
                                 AttributeName = a.Name,
                                 AttributeValue = av.Value
                             };
                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }
    }
}
