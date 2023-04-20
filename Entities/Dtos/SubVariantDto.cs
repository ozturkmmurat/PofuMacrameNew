using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class SubVariantDto : IDto
    {
        //Variant
        public int VariantId { get; set; }

        //Product
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        //Attribute
        public string AttrName { get; set; }

        //AttributeValue
        public string AttrValue { get; set; }
        public string AttrtCode { get; set; }

        //User
        public int UserId { get; set; }
    }
}
