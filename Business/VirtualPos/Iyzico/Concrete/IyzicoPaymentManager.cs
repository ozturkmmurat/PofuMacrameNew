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
using Entities.Dtos.ProductStock;
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
        IProductPriceFactorService _productPriceFactorService;
        private IHttpContextAccessor _httpContextAccessor;
        IMailService _mailService;

        public IyzicoPaymentManager(
            IProductStockService productStockService,
            IOrderService orderService,
            ISubOrderService subOrderService,
            IUserService userService,
            IProductPriceFactorService productPriceFactorService,
            IMailService mailService)
        {
            _productStockService = productStockService;
            _orderService = orderService;
            _subOrderService = subOrderService;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _userService = userService;
            _productPriceFactorService = productPriceFactorService;
            _mailService = mailService;
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
            if (orderResult != null & orderResult.OrderDate.Date > DateTime.Now.AddDays(-14))
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
                    orderResult.OrderStatus = 4; // Siparis iptal edildi ise status 4
                    orderResult.PaymentResultJson = JsonSerializer.Serialize(cancel);
                    orderResult.PaymentToken = "";

                    var orderUpdateResult = _orderService.Update(orderResult);

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
                        _mailService.CancelOrder(ClaimHelper.GetUserName(_httpContextAccessor.HttpContext), ClaimHelper.GetUserLastName(_httpContextAccessor.HttpContext), orderResult.OrderCode);

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

        private void MappingBuyerForGuest(TsaPaymentParameter param, CreateCheckoutFormInitializeRequest request)
        {
            if (param == null) return;
            Buyer buyer = new Buyer();
            buyer.Id = "0";
            var nameParts = (param.FullName ?? "").Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            buyer.Name = nameParts.Length > 0 ? nameParts[0] : param.FullName ?? "";
            buyer.Surname = nameParts.Length > 1 ? nameParts[1] : "";
            buyer.GsmNumber = "+90" + (param.Phone ?? "").TrimStart('0');
            buyer.Email = param.Email ?? "";
            buyer.IdentityNumber = !string.IsNullOrEmpty(param.TcNo) && param.TcNo.Length == 11 ? param.TcNo : "11111111111";
            buyer.LastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buyer.RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buyer.RegistrationAddress = param.Address ?? "";
            buyer.Ip = "85.34.78.112";
            buyer.City = param.City ?? "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = param.PostCode ?? "";
            request.Buyer = buyer;
        }

        private void MappingAddressForGuest(TsaPaymentParameter param, CreateCheckoutFormInitializeRequest request)
        {
            if (param == null) return;
            var contactName = (param.FullName ?? "").Trim();
            var city = param.City ?? "Istanbul";
            var desc = param.Address ?? "";
            var zip = param.PostCode ?? "";
            Address shippingAddress = new Address();
            shippingAddress.ContactName = contactName;
            shippingAddress.City = city;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = desc;
            shippingAddress.ZipCode = zip;
            request.ShippingAddress = shippingAddress;
            Address billingAddress = new Address();
            billingAddress.ContactName = contactName;
            billingAddress.City = city;
            billingAddress.Country = "Turkey";
            billingAddress.Description = desc;
            billingAddress.ZipCode = zip;
            request.BillingAddress = billingAddress;
        }

        [TransactionScopeAspect]
        public IDataResult<CheckoutForm> PaymentResult(PaymentResultPostParameter paymentResultPostParameter)
        {
            if (paymentResultPostParameter == null)
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            var guid = paymentResultPostParameter.Guid?.Trim();
            if (string.IsNullOrWhiteSpace(guid))
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            var getOrder = _orderService.GetByGuid(guid);
            if (getOrder == null || !getOrder.Success || getOrder.Data == null)
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            var order = getOrder.Data;
            if (order.Id <= 0)
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            if (order.OrderStatus != 0)
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            var getSubOrder = _subOrderService.GetAllByOrderId(order.Id);
            if (getSubOrder == null || !getSubOrder.Success || getSubOrder.Data == null || getSubOrder.Data.Count == 0)
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            if (string.IsNullOrWhiteSpace(order.PaymentToken))
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            var options = GetOptions()?.Data;
            if (options == null)
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            var request = new RetrieveCheckoutFormRequest();
            request.Token = order.PaymentToken;
            var checkoutForm = CheckoutForm.Retrieve(request, options);
            if (checkoutForm == null)
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            if (checkoutForm.Status != "success")
                return new ErrorDataResult<CheckoutForm>(message: checkoutForm.ErrorMessage ?? Messages.failCheckOrder);

            order.OrderStatus = 1;
            order.PaymentResultJson = JsonSerializer.Serialize(checkoutForm);

            var orderUpdateResult = _orderService.Update(order);
            if (orderUpdateResult == null || !orderUpdateResult.Success)
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            var subOrderWrite = _subOrderService.SubOrderStatusEdit(getSubOrder.Data, 1);
            if (subOrderWrite == null || !subOrderWrite.Success || subOrderWrite.Data == null)
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            var subOrderUpdatResult = _subOrderService.UpdateList(subOrderWrite.Data);
            if (subOrderUpdatResult == null || !subOrderUpdatResult.Success)
                return new ErrorDataResult<CheckoutForm>(Messages.failCheckOrder);

            if (!string.IsNullOrWhiteSpace(order.OrderCode) && !string.IsNullOrWhiteSpace(order.Email))
                _mailService.CreateOrder(order.OrderCode, order.Email);

            return new SuccessDataResult<CheckoutForm>(data: checkoutForm);
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

                var subOrderResult = _subOrderService.GetById(returningProduct.SubOrderId);
                CreateRefundRequest request = new CreateRefundRequest();

                var sharedJsonResult = ShredJsonData(returningProduct).Data;
                request.Locale = Locale.TR.ToString();
                request.PaymentTransactionId = sharedJsonResult.PaymentTransactionId;
                request.Price = sharedJsonResult.PaidPrice;
                request.Currency = Currency.TRY.ToString();

                Refund refund = Refund.Create(request, GetOptions().Data);

                if (refund.Status == "success")
                {
                    subOrderResult.Data.Id = subOrderResult.Data.Id;
                    subOrderResult.Data.OrderId = subOrderResult.Data.OrderId;
                    subOrderResult.Data.VariantId = subOrderResult.Data.VariantId;
                    subOrderResult.Data.Price = subOrderResult.Data.Price;
                    subOrderResult.Data.ReturnResultJson = JsonSerializer.Serialize(refund);
                    subOrderResult.Data.SubOrderStatus = 5; // Urun iade edildi ise => 5 
                    _subOrderService.Update(subOrderResult.Data);
                }
                else
                {
                    return new ErrorDataResult<Refund>(message: refund.ErrorMessage);
                }

                if (roleClaims.Contains("user"))
                {
                    _mailService.RefundingProduct(ClaimHelper.GetUserName(_httpContextAccessor.HttpContext), ClaimHelper.GetUserLastName(_httpContextAccessor.HttpContext), subOrderResult.Data.Id);
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
                returningProduct.PaidPrice = subOrder.Data.NetPrice.ToString().Replace(",", ".");
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

        [TransactionScopeAspect]
        public IDataResult<Object> TsaPayment(TsaPaymentParameter tsaPaymentParameter)
        {
            if (tsaPaymentParameter == null)
                return new ErrorDataResult<Object>();

            var userId = ClaimHelper.TryGetUserId(_httpContextAccessor.HttpContext);
            UserDto userDto = null;

            if (userId > 0)
            {
                var userResult = _userService.GetUserDtoByUserId(userId, tsaPaymentParameter.AddressId);
                if (userResult == null || !userResult.Success || userResult.Data == null)
                    return new ErrorDataResult<Object>(message: Messages.PaymentMappingBuyerFail);
                userDto = userResult.Data;
            }
            else if (userId == 0)
            {
                if (string.IsNullOrWhiteSpace(tsaPaymentParameter.FullName?.Trim()) ||
                    string.IsNullOrWhiteSpace(tsaPaymentParameter.Email?.Trim()) ||
                    string.IsNullOrWhiteSpace(tsaPaymentParameter.Phone?.Trim()))
                    return new ErrorDataResult<Object>(Messages.PaymentUserData);
            }

            if (tsaPaymentParameter.CartItems == null || tsaPaymentParameter.CartItems.Count == 0)
                return new ErrorDataResult<Object>();

            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
            if (userId > 0)
            {
                var mappingBuyerResult = MappingBuyer(userDto, tsaPaymentParameter, request);
                if (!mappingBuyerResult.Success)
                    return new ErrorDataResult<Object>(message: Messages.PaymentMappingBuyerFail);
                MappingAddress(userDto, request);
            }
            else
            {
                MappingBuyerForGuest(tsaPaymentParameter, request);
                MappingAddressForGuest(tsaPaymentParameter, request);
            }

            var productVariantIds = new List<int>();
            for (int i = 0; i < tsaPaymentParameter.CartItems.Count; i++)
            {
                var item = tsaPaymentParameter.CartItems[i];
                if (item?.product != null && item.Quantity > 0)
                    productVariantIds.Add(item.product.EndProductVariantId);
            }

            if (productVariantIds.Count == 0)
                return new ErrorDataResult<Object>();

            decimal orderPriceSum = 0;
            decimal orderExtraPrice = 0;
            var checkRequest = new ProductStockPriceCheckDto
            {
                ProductVariantId = productVariantIds,
                ProductPriceFactorId = tsaPaymentParameter.ProductPriceFactorId
            };
            var priceResult = _productStockService.CheckProductStockPrice(checkRequest);
            if (priceResult == null || !priceResult.Success || priceResult.Data == null || priceResult.Data.Count != productVariantIds.Count)
                return new ErrorDataResult<Object>();

            orderExtraPrice = priceResult.Data.Count > 0 ? priceResult.Data[0].ExtraPrice : 0;
            int index = 0;
            for (int i = 0; i < tsaPaymentParameter.CartItems.Count; i++)
            {
                if (tsaPaymentParameter.CartItems[i]?.product != null && index < priceResult.Data.Count)
                {
                    var netPrice = priceResult.Data[index].NetPrice;
                    tsaPaymentParameter.CartItems[i].product.NetPrice = netPrice;
                    tsaPaymentParameter.CartItems[i].product.Price = netPrice;
                    var qty = Math.Max(0, tsaPaymentParameter.CartItems[i].Quantity);
                    orderPriceSum += netPrice * qty;
                    index++;
                }
            }

            Order order = new Order();
            order.OrderCode = CreateCodeTime.GenerateOrderCode();
            order.Guid = System.Guid.NewGuid().ToString("N");
            order.UserId = userId > 0 ? userDto.UserId : 0;
            order.FullName = userId > 0 ? $"{userDto.FirstName} {userDto.LastName}".Trim() : (tsaPaymentParameter.FullName ?? "").Trim();
            order.Email = userId > 0 ? (userDto.Email ?? "") : (tsaPaymentParameter.Email ?? "").Trim();
            order.Phone = userId > 0 ? (userDto.PhoneNumber ?? "") : (tsaPaymentParameter.Phone ?? "").Trim();
            order.RecipientPhone = userId > 0 ? (userDto.PhoneNumber ?? "") : (tsaPaymentParameter.RecipientPhone ?? tsaPaymentParameter.Phone ?? "").Trim();
            order.ProductPriceFactorId = tsaPaymentParameter.ProductPriceFactorId;
            order.OrderStatus = 0;
            order.Description = tsaPaymentParameter.OrderDescription;
            order.Address = userId > 0
                ? $"Adres Başlığı : {userDto.AddressTitle} Şehir : {userDto.UserCity}  Posta Kodu : {userDto.PostCode} Adres: {userDto.Address} Telefon Numarası: {userDto.PhoneNumber}"
                : $"Şehir: {tsaPaymentParameter.City ?? ""} Posta Kodu: {tsaPaymentParameter.PostCode ?? ""} Adres: {tsaPaymentParameter.Address ?? ""} Telefon: {order.Phone} Alıcı Telefon : {order.RecipientPhone}";
            order.Price = orderPriceSum;
            order.ExtraPrice = orderExtraPrice;
            order.TotalPrice = order.Price + order.ExtraPrice;
            order.RequestedDeliveryStart = tsaPaymentParameter.RequestedDeliveryStart;
            order.RequestedDeliveryEnd = tsaPaymentParameter.RequestedDeliveryEnd;

            var addOrder = _orderService.Add(order);
            if (addOrder == null || !addOrder.Success)
                return new ErrorDataResult<Object>();

            if (order.Id <= 0)
                return new ErrorDataResult<Object>();

            List<BasketItem> basketItems = new List<BasketItem>();
            for (int i = 0; i < tsaPaymentParameter.CartItems.Count; i++)
            {
                if (tsaPaymentParameter.CartItems[i]?.product == null || tsaPaymentParameter.CartItems[i].Quantity <= 0)
                    return new ErrorDataResult<Object>();
                ProductStock productStock = new ProductStock();
                productStock.ProductVariantId = tsaPaymentParameter.CartItems[i].product.EndProductVariantId;
                productStock.Price = tsaPaymentParameter.CartItems[i].product.Price;
                productStock.Quantity = tsaPaymentParameter.CartItems[i].Quantity;
                productStock.Kdv = tsaPaymentParameter.CartItems[i].product.Kdv;
                productStock.NetPrice = tsaPaymentParameter.CartItems[i].product.NetPrice;
                IResult rulesResult = BusinessRules.Run(_productStockService.CheckProductStock(productStock));
                if (rulesResult == null)
                {
                    for (int j = 0; j < tsaPaymentParameter.CartItems[i].Quantity; j++)
                    {
                        SubOrder subOrder = new SubOrder();
                        subOrder.OrderId = order.Id;
                        subOrder.VariantId = tsaPaymentParameter.CartItems[i].product.EndProductVariantId;
                        subOrder.Price = tsaPaymentParameter.CartItems[i].product.Price;
                        subOrder.Kdv = tsaPaymentParameter.CartItems[i].product.Kdv;
                        subOrder.NetPrice = tsaPaymentParameter.CartItems[i].product.NetPrice;
                        subOrder.SubOrderStatus = 0;
                        var subOrderAddResult = _subOrderService.Add(subOrder);
                        if (subOrderAddResult == null || !subOrderAddResult.Success)
                            return new ErrorDataResult<object>();

                        BasketItem basketItem = new BasketItem();
                        basketItem.Id = subOrder.Id.ToString();
                        basketItem.Name = tsaPaymentParameter.CartItems[i].product.ProductName;
                        basketItem.Category1 = tsaPaymentParameter.CartItems[i].product.CategoryName;
                        basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                        basketItem.Price = tsaPaymentParameter.CartItems[i].product.Price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                        basketItems.Add(basketItem);

                        if (i == tsaPaymentParameter.CartItems.Count - 1 && j == tsaPaymentParameter.CartItems[i].Quantity - 1)
                        {
                            basketItems.Add(new BasketItem
                            {
                                Id = "KARGO",
                                Name = "Kargo Ücreti",
                                Category1 = "Kargo",
                                ItemType = BasketItemType.PHYSICAL.ToString(),
                                Price = orderExtraPrice.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                            });
                            request.Locale = Locale.TR.ToString();
                            request.ConversationId = order.Id.ToString();
                            request.Price = order.TotalPrice.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                            request.PaidPrice = order.TotalPrice.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                            request.Currency = Currency.TRY.ToString();
                            request.BasketId = order.Id.ToString();
                            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
                            request.CallbackUrl = "http://localhost:4200/payment/paymentStatus/" + order.Guid;
                            request.BasketItems = basketItems;

                            var optionsResult = GetOptions();
                            if (optionsResult == null || !optionsResult.Success || optionsResult.Data == null)
                                return new ErrorDataResult<Object>();
                            var options = optionsResult.Data;

                            var checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
                            if (checkoutFormInitialize == null)
                                return new ErrorDataResult<Object>();
                            if (checkoutFormInitialize.Status != "success" || string.IsNullOrWhiteSpace(checkoutFormInitialize.Token))
                                return new ErrorDataResult<Object>();

                            order.PaymentToken = checkoutFormInitialize.Token;
                            var updateResult = _orderService.Update(order);
                            if (updateResult == null || !updateResult.Success)
                                return new ErrorDataResult<Object>();
                            return new SuccessDataResult<Object>(data: checkoutFormInitialize.PaymentPageUrl);
                        }
                    }
                }
                else
                    return new ErrorDataResult<Object>();
            }
            return new ErrorDataResult<Object>();
        }
    }
}

