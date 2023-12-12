using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ISubOrderService
    {
        IDataResult<List<SubOrder>> GetAll();
        IDataResult<List<SubOrder>> GetAllByOrderId(int orderId);
        IDataResult<SubOrder> GetById(int id);
        IDataResult<SubOrder> OrderId(int orderId);
        IDataResult<SubOrder> GetByOrderIdPvId(int orderId, int productVariantId);
        IDataResult<List<SubOrder>> SubOrderStatusEdit(List<SubOrder> subOrders, int writeSubOrderStatus);
        IResult Add(SubOrder subOrder);
        IResult AddList(List<SubOrder> subOrders);
        IResult Update(SubOrder subOrder);
        IResult UpdateList(List<SubOrder> subOrders);
        IResult Delete(SubOrder subOrder);
    }
}
