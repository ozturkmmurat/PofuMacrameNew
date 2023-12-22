using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constans;
using Business.Utilities;
using Business.VirtualPos.Iyzico.Abstract;
using Core.Aspects.Autofac.Transaction;
using Core.Business;
using Core.Utilities.IoC;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.User;
using Entities.Concrete;
using Entities.Dtos.User;
using Entities.EntityParameter.CartItem;
using Entities.EntityParameter.Iyzico;
using Entities.LibraryEntities.Iyzico;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using Core.Extensions;

namespace Business.VirtualPos.Iyzico.Concrete
{
    public class IyzicoPaymentManager : IIyzicoPaymentService
    {
        IProductStockService _productStockService;
        IOrderService _orderService;
        ISubOrderService _subOrderService;
        IUserService _userService;
        private IHttpContextAccessor _httpContextAccessor;
        IMailService _mailService;

        public IyzicoPaymentManager(
            IProductStockService productStockService,
            IOrderService orderService,
            ISubOrderService subOrderService,
            IUserService userService,
            IMailService mailService)
        {
            _productStockService = productStockService;
            _orderService = orderService;
            _subOrderService = subOrderService;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _userService = userService;
            _mailService=mailService;
        }

        [SecuredOperation("user,admin")]
        public IDataResult<Cancel> CancelOrder(CancelOrder cancelOrder)
        {

            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            var orderResult = (Order)null;
            if (roleClaims.Contains("user"))
            {
                var getOrder = _orderService.GetByOrderIdUserId(cancelOrder.OrderId, ClaimHelper.GetUserId(_httpContextAccessor.HttpContext));
                if (!getOrder.Success)
                {
                    return new ErrorDataResult<Cancel>();
                }
                orderResult = getOrder.Data;

            }
            else if (roleClaims.Contains("admin"))
            {
                orderResult = _orderService.GetById(cancelOrder.OrderId).Data;

            }

            var subOrders = _subOrderService.GetAllByOrderId(cancelOrder.OrderId);
            if (orderResult != null & orderResult.OrderDate.Date == DateTime.Now.Date)
            {
                if (subOrders.Data == null || subOrders.Data.Count <= 0)
                {
                    return new ErrorDataResult<Cancel>(message: Messages.checkSubOrder);
                }
                var shrodJsonResult = ShredJsonData(cancelOrder).Data;
                CreateCancelRequest request = new CreateCancelRequest();
                request.Locale = Locale.TR.ToString();
                request.PaymentId = shrodJsonResult.PaymentId;

                Cancel cancel = Cancel.Create(request, GetOptions().Data);
                if (cancel.Status == "success")
                {
                    Order updateOrder = new Order()
                    {
                        Id = orderResult.Id,
                        UserId = orderResult.UserId,
                        TotalPrice = orderResult.TotalPrice,
                        OrderCode = orderResult.OrderCode,
                        OrderDate = orderResult.OrderDate,
                        OrderStatus = 4, // Sipariş iptal edildi ise status 4,
                        PaymentResultJson = orderResult.PaymentResultJson,
                        CancelResultJson = JsonSerializer.Serialize(cancel),
                        PaymentToken = ""
                    };
                    var orderUpdateResult = _orderService.Update(updateOrder);

                    if (!orderUpdateResult.Success)
                    {
                        return new ErrorDataResult<Cancel>();
                    }
                    var writeSubOrder = _subOrderService.SubOrderStatusEdit(subOrders.Data, 4);
                    var updateSubOrder = _subOrderService.UpdateList(writeSubOrder.Data);
                    if (!writeSubOrder.Success || !updateSubOrder.Success)
                    {
                        return new ErrorDataResult<Cancel>();
                    }


                    if (roleClaims.Contains("user"))
                    {
                        _mailService.CancelOrder(ClaimHelper.GetUserName(_httpContextAccessor.HttpContext), ClaimHelper.GetUserLastName(_httpContextAccessor.HttpContext), updateOrder.Id);

                    }
                    else if (roleClaims.Contains("admin"))
                    {
                        _mailService.AdminCancelOrder();
                    }
                    return new SuccessDataResult<Cancel>(data: cancel);
                }
                else
                {
                    return new ErrorDataResult<Cancel>(message: cancel.ErrorMessage);
                }
            }
            return new ErrorDataResult<Cancel>(message: Messages.failCancelOrderDate);

        }

        [SecuredOperation("user,admin")]
        public IDataResult<Iyzipay.Options> GetOptions()
        {
            Iyzipay.Options options = new Iyzipay.Options();
            options.ApiKey = "sandbox-JhWWKjfLizjGbynSPDMeNfpl6MVSnFKz";
            options.SecretKey = "sandbox-KKyHHykgnBolgi1fqX1dlFLZTSNbeUR7";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";
            return new SuccessDataResult<Iyzipay.Options>(options);
        }

        [SecuredOperation("user,admin")]

        public IDataResult<CreateCheckoutFormInitializeRequest> MappingAddress(UserDto userDto, CreateCheckoutFormInitializeRequest request)
        {
            if (userDto != null)
            {
                Address shippingAddress = new Address();
                shippingAddress.ContactName = userDto.FirstName + " " + userDto.LastName;
                shippingAddress.City = userDto.UserCity;
                shippingAddress.Country = "Turkey";
                shippingAddress.Description = userDto.Address;
                shippingAddress.ZipCode = userDto.PostCode;
                request.ShippingAddress = shippingAddress;

                Address billingAddress = new Address();
                billingAddress.ContactName = userDto.FirstName + " " + userDto.LastName;
                billingAddress.City = userDto.UserCity;
                billingAddress.Country = "Turkey";
                billingAddress.Description = userDto.Address;
                billingAddress.ZipCode = userDto.PostCode;
                request.BillingAddress = billingAddress;

                return new SuccessDataResult<CreateCheckoutFormInitializeRequest>(request);
            }
            return new ErrorDataResult<CreateCheckoutFormInitializeRequest>();
        }

        [SecuredOperation("user,admin")]

        public IDataResult<CreateCheckoutFormInitializeRequest> MappingBuyer(UserDto userDto, TsaPaymentParameter tsaPaymentParameter, CreateCheckoutFormInitializeRequest request)
        {
            if (userDto != null)
            {
                Buyer buyer = new Buyer();
                buyer.Id = userDto.UserId.ToString();
                buyer.Name = userDto.FirstName;
                buyer.Surname = userDto.LastName;
                buyer.GsmNumber = "+90" + userDto.PhoneNumber;
                buyer.Email = userDto.Email;
                buyer.IdentityNumber = tsaPaymentParameter.TcNo != null && tsaPaymentParameter.TcNo.Count() == 11 ? tsaPaymentParameter.TcNo : "11111111111";
                buyer.LastLoginDate = "2015-10-05 12:43:35";
                buyer.RegistrationDate = "2013-04-21 15:12:09";
                buyer.RegistrationAddress = userDto.Address;
                buyer.Ip = "85.34.78.112";
                buyer.City = userDto.UserCity;
                buyer.Country = "Turkey";
                buyer.ZipCode = userDto.PostCode;
                request.Buyer = buyer;

                return new SuccessDataResult<CreateCheckoutFormInitializeRequest>(request);
            }
            return new ErrorDataResult<CreateCheckoutFormInitializeRequest>();
        }

        [SecuredOperation("user,admin")]
        [TransactionScopeAspect]
        public IDataResult<CheckoutForm> PaymentResult(PaymentResultPostParameter paymentResultPostParameter)
        {
            RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
            if (paymentResultPostParameter.OrderId > 0)
            {
                var getOrder = _orderService.GetByOrderIdUserId(paymentResultPostParameter.OrderId, ClaimHelper.GetUserId(_httpContextAccessor.HttpContext));
                if (getOrder.Data != null)
                {
                    var getSubOrder = _subOrderService.GetAllByOrderId(paymentResultPostParameter.OrderId);
                    if (getSubOrder.Data != null & getSubOrder.Data.Count > 0)
                    {
                        request.Token = getOrder.Data.PaymentToken;
                        CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, GetOptions().Data);
                        if (checkoutForm.Status == "success")
                        {
                            Order order = new Order()
                            {
                                Id = paymentResultPostParameter.OrderId,
                                UserId = getOrder.Data.UserId,
                                TotalPrice = getOrder.Data.TotalPrice,
                                OrderCode = getOrder.Data.OrderCode,
                                OrderDate = getOrder.Data.OrderDate,
                                OrderStatus = 1, // Siparis alindi ise 1
                                PaymentResultJson = JsonSerializer.Serialize(checkoutForm),
                                PaymentToken = getOrder.Data.PaymentToken,
                                Address = getOrder.Data.Address
                            };
                            var orderUpdateResult = _orderService.Update(order);
                            if (!orderUpdateResult.Success)
                            {
                                return new ErrorDataResult<CheckoutForm>();
                            }
                            var subOrderWrite = _subOrderService.SubOrderStatusEdit(getSubOrder.Data, 1);
                            var subOrderUpdatResult = _subOrderService.UpdateList(subOrderWrite.Data);
                            if (!subOrderWrite.Success || !subOrderUpdatResult.Success)
                            {
                                return new ErrorDataResult<CheckoutForm>();
                            }
                            _mailService.CreateOrder();
                            return new SuccessDataResult<CheckoutForm>(data: checkoutForm);
                        }
                        else
                        {
                            return new ErrorDataResult<CheckoutForm>(message: checkoutForm.ErrorMessage);
                        }
                    }
                }
            }
            return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);
        }

        [SecuredOperation("user,admin")]
        public IDataResult<Refund> RefundProduct(ReturningProduct returningProduct)
        {
            if (returningProduct.SubOrderId  > 0)
            {
                var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
                if (roleClaims.Contains("user"))
                {
                    if (!_subOrderService.CheckSubOrder(returningProduct.OrderId, returningProduct.SubOrderId, ClaimHelper.GetUserId(_httpContextAccessor.HttpContext)))
                    {
                        return new ErrorDataResult<Refund>();
                    }
                }


                SubOrder subOrder = new SubOrder();
                subOrder.OrderId = returningProduct.OrderId;

                CreateRefundRequest request = new CreateRefundRequest();
                CultureInfo turkishCulture = new CultureInfo("tr-TR");

                var sharedJsonResult = ShredJsonData(returningProduct).Data;
                request.Locale = Locale.TR.ToString();
                request.PaymentTransactionId = sharedJsonResult.PaymentTransactionId;
                request.Price = sharedJsonResult.PaidPrice;
                request.Currency = Currency.TRY.ToString();

                Refund refund = Refund.Create(request, GetOptions().Data);

                if (refund.Status == "success")
                {
                    var subOrderResult = _subOrderService.GetById(returningProduct.SubOrderId);

                    subOrder.Id = subOrderResult.Data.Id;
                    subOrder.OrderId = subOrderResult.Data.OrderId;
                    subOrder.VariantId = subOrderResult.Data.VariantId;
                    subOrder.Price = subOrderResult.Data.Price;
                    subOrder.ReturnResultJson = JsonSerializer.Serialize(refund);
                    subOrder.SubOrderStatus = 5; // Urun iade edildi ise => 5 
                    _subOrderService.Update(subOrder);
                }
                else
                {
                    return new ErrorDataResult<Refund>(message: refund.ErrorMessage);
                }

                if (roleClaims.Contains("user"))
                {
                    _mailService.RefundingProduct(ClaimHelper.GetUserName(_httpContextAccessor.HttpContext), ClaimHelper.GetUserLastName(_httpContextAccessor.HttpContext), subOrder.Id);
                }
                else if (roleClaims.Contains("admin"))
                {
                    _mailService.AdminRefundingProduct();
                }

                return new SuccessDataResult<Refund>(refund);
            }
            return new ErrorDataResult<Refund>();
        }

        public IDataResult<ReturningProduct> ShredJsonData(ReturningProduct returningProduct)
        {
            var order = _orderService.GetById(returningProduct.OrderId).Data.PaymentResultJson;
            JObject jsonObject = JObject.Parse(order);

            var paymentItem = jsonObject["PaymentItems"]
                .FirstOrDefault(item => item["ItemId"]?.ToString() == returningProduct.SubOrderId.ToString());

            if (paymentItem != null)
            {
                string paymentTransactionId = paymentItem["PaymentTransactionId"]?.ToString();

                var subOrder = _subOrderService.GetById(returningProduct.SubOrderId);
                returningProduct.PaymentTransactionId = paymentTransactionId;
                returningProduct.PaidPrice = subOrder.Data.Price.ToString().Replace(",", ".");
                return new SuccessDataResult<ReturningProduct>(returningProduct);
            }
            return new ErrorDataResult<ReturningProduct>();
        }

        public IDataResult<CancelOrder> ShredJsonData(CancelOrder cancelOrder)
        {
            var orderResult = _orderService.GetById(cancelOrder.OrderId).Data.PaymentResultJson;
            JObject jsonObject = JObject.Parse(orderResult);

            if (jsonObject["BasketId"].ToString() == cancelOrder.OrderId.ToString())
            {
                string paymentId = jsonObject["PaymentId"].ToString();
                cancelOrder.PaymentId = paymentId;
                return new SuccessDataResult<CancelOrder>(cancelOrder);
            }
            return new ErrorDataResult<CancelOrder>();
        }

        public IResult Test()
        {
            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = "1";
            request.PaidPrice = "1.2";
            request.Currency = Currency.TRY.ToString();
            request.BasketId = "B67832";
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = "http://localhost:4200/payment/paymentStatus";

            List<int> enabledInstallments = new List<int>();
            enabledInstallments.Add(2);
            enabledInstallments.Add(3);
            enabledInstallments.Add(6);
            enabledInstallments.Add(9);
            request.EnabledInstallments = enabledInstallments;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Binocular";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = "0.3";
            basketItems.Add(firstBasketItem);

            BasketItem secondBasketItem = new BasketItem();
            secondBasketItem.Id = "BI102";
            secondBasketItem.Name = "Game code";
            secondBasketItem.Category1 = "Game";
            secondBasketItem.Category2 = "Online Game Items";
            secondBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
            secondBasketItem.Price = "0.5";
            basketItems.Add(secondBasketItem);

            BasketItem thirdBasketItem = new BasketItem();
            thirdBasketItem.Id = "BI103";
            thirdBasketItem.Name = "Usb";
            thirdBasketItem.Category1 = "Electronics";
            thirdBasketItem.Category2 = "Usb / Cable";
            thirdBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            thirdBasketItem.Price = "0.2";
            basketItems.Add(thirdBasketItem);
            request.BasketItems = basketItems;

            var options = GetOptions().Data;


            CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);

            if (checkoutFormInitialize.Status == "success")
            {
                request.CallbackUrl = "https://localhost:44343/api/orderPayments/test2?x="+checkoutFormInitialize.ConversationId + "&y="+checkoutFormInitialize.Token;
                var result = Test2(checkoutFormInitialize.ConversationId, checkoutFormInitialize.Token);

                return new SuccessResult(result.Message);

            }



            var test = checkoutFormInitialize;
            return new ErrorResult();
        }

        public IResult Test2(string x, string y)
        {
            var options = GetOptions().Data;

            RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
            request.ConversationId = "123456789";
            request.Token = y;

            CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, options);
            return new SuccessResult();
        }

        [SecuredOperation("user,admin")]
        [TransactionScopeAspect]
        public IDataResult<Object> TsaPayment(TsaPaymentParameter tsaPaymentParameter)
        {
            var userId = ClaimHelper.GetUserId(_httpContextAccessor.HttpContext);
            if (userId > 0)
            {
                var userDto = _userService.GetUserDtoByUserId(userId, tsaPaymentParameter.AddressId).Data;
                if (userDto != null)
                {
                    CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();

                    var mappingBuyerResult = MappingBuyer(userDto, tsaPaymentParameter, request);
                    if (!mappingBuyerResult.Success)
                    {
                        return new ErrorDataResult<Object>(message: Messages.PaymentMappingBuyerFail);
                    }

                    MappingAddress(userDto, request);

                    if (tsaPaymentParameter.CartItems != null & tsaPaymentParameter.CartItems.Count > 0)
                    {
                        Order order = new Order();
                        order.OrderCode = CreateCodeTime.GenerateOrderCode();
                        order.UserId = userDto.UserId;
                        order.OrderStatus = 0;
                        order.Address = $"Adres Başlığı : {userDto.AddressTitle} Şehir : {userDto.UserCity}  Posta Kodu : {userDto.PostCode} Adres: {userDto.Address} Telefon Numarası: {userDto.PhoneNumber}";
                        order.TotalPrice = tsaPaymentParameter.CartItems.Sum(x => x.product.Price * x.Quantity);
                        var addOrder = _orderService.Add(order);
                        if (addOrder.Success)
                        {
                            List<BasketItem> basketItems = new List<BasketItem>();
                            for (int i = 0; i < tsaPaymentParameter.CartItems.Count; i++)
                            {
                                ProductStock productStock = new ProductStock();
                                productStock.ProductVariantId = tsaPaymentParameter.CartItems[i].product.EndProductVariantId;
                                productStock.Price = tsaPaymentParameter.CartItems[i].product.Price;
                                productStock.Quantity = tsaPaymentParameter.CartItems[i].Quantity;
                                IResult rulesResult = BusinessRules.Run(_productStockService.CheckProductStock(productStock), _productStockService.CheckProductStockPrice(productStock));
                                if (rulesResult == null)
                                {
                                    for (int j = 0; j < tsaPaymentParameter.CartItems[i].Quantity; j++)
                                    {
                                        SubOrder subOrder = new SubOrder();
                                        subOrder.OrderId = order.Id;
                                        subOrder.VariantId = tsaPaymentParameter.CartItems[i].product.EndProductVariantId;
                                        subOrder.Price = tsaPaymentParameter.CartItems[i].product.Price;
                                        subOrder.SubOrderStatus = 0;
                                        var subOrderAddResult = _subOrderService.Add(subOrder);
                                        if (!subOrderAddResult.Success)
                                        {
                                            return new ErrorDataResult<object>();
                                        }
                                        BasketItem basketItem = new BasketItem();
                                        basketItem.Id = subOrder.Id.ToString();
                                        basketItem.Name = tsaPaymentParameter.CartItems[i].product.ProductName;
                                        basketItem.Category1 = tsaPaymentParameter.CartItems[i].product.CategoryName;
                                        basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                                        basketItem.Price = tsaPaymentParameter.CartItems[i].product.Price.ToString();
                                        basketItems.Add(basketItem);
                                        if (i == tsaPaymentParameter.CartItems.Count -1)
                                        {
                                            if (j == tsaPaymentParameter.CartItems[i].Quantity -1)
                                            {
                                                request.Locale = Locale.TR.ToString();
                                                request.ConversationId = order.Id.ToString();
                                                request.Price = order.TotalPrice.ToString();
                                                request.PaidPrice = order.TotalPrice.ToString();
                                                request.Currency = Currency.TRY.ToString();
                                                request.BasketId = order.Id.ToString();
                                                request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
                                                request.CallbackUrl = "http://localhost:4200/payment/paymentStatus/"+ order.Id;
                                                request.BasketItems = basketItems;

                                                var options = GetOptions().Data;
                                                CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
                                                if (checkoutFormInitialize.Status == "success")
                                                {
                                                    order.PaymentToken = checkoutFormInitialize.Token;
                                                    _orderService.Update(order);
                                                    return new SuccessDataResult<Object>(data: checkoutFormInitialize.PaymentPageUrl);
                                                }
                                                else
                                                {
                                                    return new ErrorDataResult<Object>();
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    return new ErrorDataResult<Object>();
                                }

                            }
                        }


                    }
                }
            }
            return new ErrorDataResult<Object>();
        }
    }
}

