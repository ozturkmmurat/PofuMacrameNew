using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Dtos.Variant
{
    public class AddVariantDto : IDto
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string StockCode { get; set; }

        //Veritabanında Variant da boyle bir alan yok stok kodu olustururken gerekiyor.
        [NotMapped]
        public List<string> AttrCode { get; set; }
    }
}
