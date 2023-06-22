using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Variant : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
    }
}
