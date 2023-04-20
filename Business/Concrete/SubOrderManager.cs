using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class SubOrderManager : ISubOrderService
    {
        ISubOrderDal _subOrderDal;
        public SubOrderManager(ISubOrderDal subOrderDal)
        {
            _subOrderDal = subOrderDal;
        }
        public IResult Add(SubOrder subOrder)
        {
            if (subOrder != null)
            {
                _subOrderDal.Add(subOrder);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Delete(SubOrder subOrder)
        {
            if (subOrder != null)
            {
                _subOrderDal.Delete(subOrder);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IDataResult<List<SubOrder>> GetAll()
        {
            var result = _subOrderDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<SubOrder>>(result);
            }
            return new ErrorDataResult<List<SubOrder>>();
        }

        public IDataResult<SubOrder> OrderId(int orderId)
        {
            var result = _subOrderDal.Get(x => x.OrderId == orderId);
            if (result != null)
            {
                return new SuccessDataResult<SubOrder>(result);
            }
            return new ErrorDataResult<SubOrder>();
        }

        public IResult Update(SubOrder subOrder)
        {
            if (subOrder != null)
            {
                _subOrderDal.Update(subOrder);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
