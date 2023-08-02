﻿using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Concrete
{
    public class ProductAttributeImage : IEntity
    {
        public int Id { get; set; }
        public int ProductAttributeId { get; set; }
        public string Path { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
