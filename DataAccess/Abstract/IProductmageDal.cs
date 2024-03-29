﻿using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IProductmageDal : IEntityRepository<ProductImage>
    {
        List<string> GetFirstTwoPhotosNT(int productVariantId); //NT -> AsNoTracking
    }
}
