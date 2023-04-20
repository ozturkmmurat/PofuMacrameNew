using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IOrderService
    {
        IDataResult<List<Order>> GetAll();
        IDataResult<List<Order>> GetAllByUserId(int userId);
        IDataResult<Order> OrderCode(string orderCode);
        IResult Add(Order order);
        IResult Update(Order order);
        IResult Delete(Order order);
    }
}
