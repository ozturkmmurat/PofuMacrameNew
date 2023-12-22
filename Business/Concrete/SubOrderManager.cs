using Business.Abstract;
using Business.Abstract.ProductVariants;
using Business.BusinessAspects.Autofac;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos.Order.Select;
using Entities.Dtos.SubOrder.Select;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class SubOrderManager : ISubOrderService
    {
        ISubOrderDal _subOrderDal;
        IProductImageService _productImageService;
        IProductVariantService _productVariantService;
        public SubOrderManager(ISubOrderDal subOrderDal, IProductImageService productImageService, IProductVariantService productVariantService)
        {
            _subOrderDal = subOrderDal;
            _productImageService=productImageService;
            _productVariantService=productVariantService;
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

        public bool CheckSubOrder(int orderId, int subOrderId, int userId)
        {
            return _subOrderDal.CheckSubOrder(orderId, subOrderId, userId);
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

        [SecuredOperation("admin")]
        public IDataResult<List<SelectOrderedProducts>> GetAllOrderedProduct()
        {
            var result = _subOrderDal.GetAllOrderedProduct();
            if (result != null & result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    var getProductVariantAttributeResult = _productVariantService.GetProductVariantAttribute(result[i].ParentId);
                    result[i].ImagePath = _productImageService.GetByProductVariantId(getProductVariantAttributeResult.Data.VariantId).Data.Path;
                    result[i].Attribute = _productVariantService.GetProductVariantAttribute(result[i].ParentId).Data.Attribute;
                }
                return new SuccessDataResult<List<SelectOrderedProducts>>(result);
            }
            return new ErrorDataResult<List<SelectOrderedProducts>>();
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
            if (subOrders != null & subOrders.Count > 0)
            {
                _subOrderDal.UpdateRange(subOrders);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
