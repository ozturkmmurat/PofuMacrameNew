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

        public IResult AddList(List<SubOrder> subOrders)
        {
            if (subOrders != null & subOrders.Count > 0)
            {
                _subOrderDal.AddRange(subOrders);
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

        public IDataResult<List<SubOrder>> GetAllByOrderId(int orderId)
        {
            var result = _subOrderDal.GetAll(x => x.OrderId == orderId);
            if (result != null)
            {
                return new SuccessDataResult<List<SubOrder>>(result);
            }
            return new ErrorDataResult<List<SubOrder>>();
        }

        public IDataResult<SubOrder> GetById(int id)
        {
            var result = _subOrderDal.Get(x => x.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<SubOrder>(result);
            }
            return new ErrorDataResult<SubOrder>();
        }

        public IDataResult<SubOrder> GetByOrderIdPvId(int orderId, int productVariantId)
        {
            var result = _subOrderDal.Get(x => x.OrderId == orderId && x.VariantId == productVariantId);
            if (result != null)
            {
                return new SuccessDataResult<SubOrder>(result);
            }
            return new ErrorDataResult<SubOrder>();
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

        public IDataResult<List<SubOrder>> SubOrderStatusEdit(List<SubOrder> subOrders, int writeSubOrderStatus)
        {
            if (subOrders != null & subOrders.Count > 0)
            {
                for (int i = 0; i < subOrders.Count; i++)
                {
                    subOrders[i].SubOrderStatus = writeSubOrderStatus;
                };
                return new SuccessDataResult<List<SubOrder>>(subOrders);
            }
            return new ErrorDataResult<List<SubOrder>>();
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

        public IResult UpdateList(List<SubOrder> subOrders)
        {
            if(subOrders != null & subOrders.Count > 0)
            {
                _subOrderDal.UpdateRange(subOrders);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
