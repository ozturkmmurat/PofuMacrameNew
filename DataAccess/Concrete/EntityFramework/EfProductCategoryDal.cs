using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductCategoryDal : EfEntityRepositoryBase<ProductCategory, PofuMacrameContext>, IProductCategoryDal
    {
        private readonly PofuMacrameContext _context;

        public EfProductCategoryDal(PofuMacrameContext context) : base(context)
        {
            _context = context;
        }
    }
}
