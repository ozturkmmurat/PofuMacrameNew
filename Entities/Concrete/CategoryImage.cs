﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class CategoryImage : IEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Path { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
