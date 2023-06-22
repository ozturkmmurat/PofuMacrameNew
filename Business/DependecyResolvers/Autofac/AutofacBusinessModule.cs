﻿using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Helpers.FileHelper;
using Core.Utilities.Helpers;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
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

            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();

            builder.RegisterType<EfProductStockDal>().As<IProductStockDal>().SingleInstance();
            builder.RegisterType<ProductStockManager>().As<IProductStockService>().SingleInstance();

            builder.RegisterType<EfSubOrderDal>().As<ISubOrderDal>().SingleInstance();
            builder.RegisterType<SubOrderManager>().As<ISubOrderService>().SingleInstance();

            builder.RegisterType<EfVariantDal>().As<IVariantDal>().SingleInstance();
            builder.RegisterType<VariantManager>().As<IVariantService>().SingleInstance();

            builder.RegisterType<EfEntityTypeDal>().As<IEntityTypeDal>().SingleInstance();
            builder.RegisterType<EntityTypeManager>().As<IEntityTypeService>().SingleInstance();

            builder.RegisterType<EfImageDal>().As<IImageDal>().SingleInstance();
            builder.RegisterType<ImageManager>().As<IImageService>().SingleInstance();
            builder.RegisterType<FileHelperManager>().As<IFileHelper>().SingleInstance();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}