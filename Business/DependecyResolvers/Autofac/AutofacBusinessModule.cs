using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Abstract.ProductVariants;
using Business.Concrete;
using Business.Concrete.ProductVariants;
using Business.VirtualPos.Iyzico.Abstract;
using Business.VirtualPos.Iyzico.Concrete;
using Castle.DynamicProxy;
using Core.Helpers.FileHelper;
using Core.Utilities.Helpers;
using Core.Utilities.Helpers.MailHelper;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;


namespace Business.DependecyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfAttributeDal>().As<IAttributeDal>().SingleInstance();
            builder.RegisterType<AttributeManager>().As<IAttributeService>().SingleInstance();

            builder.RegisterType<EfAttributeValueDal>().As<IAttributeValueDal>().SingleInstance();
            builder.RegisterType<AttributeValueManager>().As<IAttributeValueService>().SingleInstance();

            builder.RegisterType<EfOrderDal>().As<IOrderDal>().SingleInstance();
            builder.RegisterType<OrderManager>().As<IOrderService>().SingleInstance();

            builder.RegisterType<EfProductAttributeDal>().As<IProductAttributeDal>().SingleInstance();
            builder.RegisterType<ProductAttributeManager>().As<IProductAttributeService>().SingleInstance();

            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();
            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();

            builder.RegisterType<EfCategoryAttributeDal>().As<ICategoryAttributeDal>().SingleInstance();
            builder.RegisterType<CategoryAttributeManager>().As<ICategoryAttributeService>().SingleInstance();

            builder.RegisterType<EfCategoryImageDal>().As<ICategoryImageDal>().SingleInstance();
            builder.RegisterType<CategoryImageManager>().As<ICategoryImageService>().SingleInstance();

            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();

            builder.RegisterType<EfProductStockDal>().As<IProductStockDal>().SingleInstance();
            builder.RegisterType<ProductStockManager>().As<IProductStockService>().SingleInstance();

            builder.RegisterType<EfSubOrderDal>().As<ISubOrderDal>().SingleInstance();
            builder.RegisterType<SubOrderManager>().As<ISubOrderService>().SingleInstance();

            builder.RegisterType<EfProductVariantDal>().As<IProductVariantDal>().SingleInstance();
            builder.RegisterType<ProductVariantManager>().As<IProductVariantService>().SingleInstance();
            builder.RegisterType<ProductVariantAttributeCombinationManager>().As<IProductVariantAttributeCombinationService>().SingleInstance();


            builder.RegisterType<EfProductImageDal>().As<IProductmageDal>().SingleInstance();
            builder.RegisterType<ProductImageManager>().As<IProductImageService>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserService>().SingleInstance();
            builder.RegisterType<EfUserDal>().As<IUserDal>().SingleInstance();

            builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>().SingleInstance();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>().SingleInstance();

            builder.RegisterType<UserOperationClaimManager>().As<IUserOperationClaimService>().SingleInstance();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>().SingleInstance();

            builder.RegisterType<EfUserAddressDal>().As<IUserAddressDal>().SingleInstance();
            builder.RegisterType<UserAddressManager>().As<IUserAddressService>().SingleInstance();

            builder.RegisterType<CityManager>().As<ICityService>().SingleInstance();
            builder.RegisterType<EfCityDal>().As<ICityDal>().SingleInstance();

            builder.RegisterType<PasswordResetManager>().As<IPasswordResetService>().SingleInstance();
            builder.RegisterType<EfPasswordResetDal>().As<IPasswordResetDal>().SingleInstance();

            builder.RegisterType<AuthManager>().As<IAuthService>().SingleInstance();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>().SingleInstance();



            builder.RegisterType<IyzicoPaymentManager>().As<IIyzicoPaymentService>().SingleInstance();

            builder.RegisterType<FileHelperManager>().As<IFileHelper>().SingleInstance();

            builder.RegisterType<MailManager>().As<IMailService>().SingleInstance();
            builder.RegisterType<MailHelper>().As<IMailHelper>().SingleInstance();


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
