using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Entities.Concrete
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductPriceFactorId { get; set; }
        public decimal Price { get; set; }
        public decimal ExtraPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderStatus { get; set; } //0 -> Odeme alınmadı 1 -> Odeme Alındı  2 -> Siparişe Verildi 3 -> Sipariş Teslim edildi.
        public string PaymentResultJson { get; set; }
        public string CancelResultJson { get; set; }
        public string PaymentToken { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public Order()
        {
            OrderDate = DateTime.Now;
        }
    }

}
