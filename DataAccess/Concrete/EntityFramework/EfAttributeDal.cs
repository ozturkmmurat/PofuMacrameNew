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
    }
}
