using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos.Variant.Select;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IVariantDal : IEntityRepository<Variant>
    {
        List<ViewVariantDto> GetAllFilterDto(Expression<Func<ViewVariantDto, bool>> filter = null);
    }
}
