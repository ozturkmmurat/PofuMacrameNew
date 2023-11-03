using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class City : IEntity
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
    }
}
