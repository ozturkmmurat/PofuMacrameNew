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
        IDataResult<SubOrder> OrderId(int orderId);
        IResult Add(SubOrder subOrder);
        IResult Update(SubOrder subOrder);
        IResult Delete(SubOrder subOrder);
    }
}
