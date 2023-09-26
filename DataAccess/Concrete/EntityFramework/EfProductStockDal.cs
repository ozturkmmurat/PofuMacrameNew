using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Entities.Dtos.ProductStock;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using Core.Utilities.Result.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductStockDal : EfEntityRepositoryBase<ProductStock, PofuMacrameContext>, IProductStockDal
    {
    }
}
