﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class SubCategory : IEntity
    {
        public int Id { get; set; }
        public string SubCategoryName { get; set; }
    }
}