using Core.Entities;
using System;

namespace Entities.Concrete
{
    public class SiteContent : IEntity
    {
        public int Id { get; set; }
        public string ContentKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { get; set; }
    }
}
