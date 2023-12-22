using Core.DataAccess;
using Core.Entities;
using Entities.Concrete;
using Entities.Dtos.Order.Select;
using Entities.Dtos.Product.Select;
using Entities.EntitiyParameter.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IOrderDal : IEntityRepository<Order>
    {
        List<SelectUserOrderDto> GetAllUserOrder(int userId);
        List<SelectUserOrderDto> GetAllUserOrderAdmin();
        SelectUserOrderDto GetUserOrder(int userId, int orderId);

    }
}
