using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Image : IEntity
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public int EntityTypeId { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
