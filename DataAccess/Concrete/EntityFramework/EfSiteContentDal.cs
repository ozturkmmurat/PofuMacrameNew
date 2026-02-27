using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfSiteContentDal : EfEntityRepositoryBase<SiteContent, PofuMacrameContext>, ISiteContentDal
    {
        public EfSiteContentDal(PofuMacrameContext context) : base(context)
        {
        }
    }
}
