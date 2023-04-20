using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class VariantDto : IDto
    {
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
