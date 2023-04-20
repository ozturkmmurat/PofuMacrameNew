using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IVariantDal : IEntityRepository<Variant>
    {
        List<VariantDto> GetAllFilterDto(Expression<Func<VariantDto, bool>> filter = null);
    }
}
