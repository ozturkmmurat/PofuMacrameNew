using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos.Order.Select;
using Entities.Dtos.SubOrder.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface ISubOrderDal : IEntityRepository<SubOrder>
    {
        List<SelectOrderedProducts> GetAllOrderedProduct();
        bool CheckSubOrder(int orderId, int subOrderId, int userId);
    }
}
