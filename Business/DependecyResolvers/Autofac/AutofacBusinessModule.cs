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
using DataAccess.Concrete.EntityFramework;
using DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Text;


namespace Business.DependecyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfAttributeDal>().As<IAttributeDal>().InstancePerLifetimeScope();
            builder.RegisterType<AttributeManager>().As<IAttributeService>().InstancePerLifetimeScope();

            builder.RegisterType<EfAttributeValueDal>().As<IAttributeValueDal>().InstancePerLifetimeScope();
            builder.RegisterType<AttributeValueManager>().As<IAttributeValueService>().InstancePerLifetimeScope();

            builder.RegisterType<EfOrderDal>().As<IOrderDal>().InstancePerLifetimeScope();
            builder.RegisterType<OrderManager>().As<IOrderService>().InstancePerLifetimeScope();

            builder.RegisterType<EfProductAttributeDal>().As<IProductAttributeDal>().InstancePerLifetimeScope();
            builder.RegisterType<ProductAttributeManager>().As<IProductAttributeService>().InstancePerLifetimeScope();

            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryManager>().As<ICategoryService>().InstancePerLifetimeScope();

            builder.RegisterType<EfCategoryAttributeDal>().As<ICategoryAttributeDal>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryAttributeManager>().As<ICategoryAttributeService>().InstancePerLifetimeScope();

            builder.RegisterType<EfCategoryImageDal>().As<ICategoryImageDal>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryImageManager>().As<ICategoryImageService>().InstancePerLifetimeScope();

            builder.RegisterType<EfProductDal>().As<IProductDal>().InstancePerLifetimeScope();
            builder.RegisterType<ProductManager>().As<IProductService>().InstancePerLifetimeScope();

            builder.RegisterType<EfProductStockDal>().As<IProductStockDal>().InstancePerLifetimeScope();
            builder.RegisterType<ProductStockManager>().As<IProductStockService>().InstancePerLifetimeScope();

            builder.RegisterType<EfSubOrderDal>().As<ISubOrderDal>().InstancePerLifetimeScope();
            builder.RegisterType<SubOrderManager>().As<ISubOrderService>().InstancePerLifetimeScope();

            builder.RegisterType<EfProductVariantDal>().As<IProductVariantDal>().InstancePerLifetimeScope();
            builder.RegisterType<ProductVariantManager>().As<IProductVariantService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductVariantAttributeCombinationManager>().As<IProductVariantAttributeCombinationService>().InstancePerLifetimeScope();


            builder.RegisterType<EfProductImageDal>().As<IProductmageDal>().InstancePerLifetimeScope();
            builder.RegisterType<ProductImageManager>().As<IProductImageService>().InstancePerLifetimeScope();

            builder.RegisterType<UserManager>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<EfUserDal>().As<IUserDal>().InstancePerLifetimeScope();

            builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>().InstancePerLifetimeScope();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>().InstancePerLifetimeScope();

            builder.RegisterType<UserOperationClaimManager>().As<IUserOperationClaimService>().InstancePerLifetimeScope();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>().InstancePerLifetimeScope();

            builder.RegisterType<EfUserAddressDal>().As<IUserAddressDal>().InstancePerLifetimeScope();
            builder.RegisterType<UserAddressManager>().As<IUserAddressService>().InstancePerLifetimeScope();

            builder.RegisterType<CityManager>().As<ICityService>().InstancePerLifetimeScope();
            builder.RegisterType<EfCityDal>().As<ICityDal>().InstancePerLifetimeScope();

            builder.RegisterType<PasswordResetManager>().As<IPasswordResetService>().InstancePerLifetimeScope();
            builder.RegisterType<EfPasswordResetDal>().As<IPasswordResetDal>().InstancePerLifetimeScope();

            builder.RegisterType<AuthManager>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>().InstancePerLifetimeScope();

            builder.RegisterType<IyzicoPaymentManager>().As<IIyzicoPaymentService>().InstancePerLifetimeScope();

            builder.RegisterType<FileHelperManager>().As<IFileHelper>().InstancePerLifetimeScope();

            builder.RegisterType<MailManager>().As<IMailService>().InstancePerLifetimeScope();
            builder.RegisterType<MailHelper>().As<IMailHelper>().InstancePerLifetimeScope();


            builder.RegisterType<PofuMacrameContext>().InstancePerLifetimeScope();


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).InstancePerLifetimeScope();
        }
    }
}
