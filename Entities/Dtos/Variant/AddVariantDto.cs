using Core.Entities;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos.Variant
{
    public class AddVariantDto : IDto
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public List<string> AttrCode { get; set; }
        public ProductStock ProductStock { get; set; }
    }
}
