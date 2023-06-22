using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using Entities.Dtos.Variant.Select;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfVariantDal : EfEntityRepositoryBase<Variant, PofuMacrameContext>, IVariantDal
    {
    }
}
