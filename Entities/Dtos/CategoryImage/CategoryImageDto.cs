using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos.CategoryImage
{
    public class CategoryImageDto
    {
        public int CategoryId { get; set; }

        [NotMapped]
        public List<IFormFile> Files { get; set; }
    }
}
