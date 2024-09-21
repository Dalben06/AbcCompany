using AbcCompany.Orders.Domain.Entities;
using AbcCompany.Orders.Domain.Enums;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcCompany.Orders.Application.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }
        public decimal Total { get; set; }
        public decimal DiscountTotal { get; set; }
        public List<OrderPaymentModel> Payments { get; set; }
        public List<OrderProductModel> Products { get; set; }
        public OrderModel()
        {
            Payments = new List<OrderPaymentModel>();
            Products = new List<OrderProductModel>();
        }
    }
}
