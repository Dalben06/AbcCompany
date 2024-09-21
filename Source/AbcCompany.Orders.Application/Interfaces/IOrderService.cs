using AbcCompany.Orders.Application.Models;
using AbcCompany.Orders.Domain.Entities;

namespace AbcCompany.Orders.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetAll();
        Task<OrderModel> GetById(int id);


    }
}
