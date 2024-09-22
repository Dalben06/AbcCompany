using AbcCompany.Orders.Application.Models;
using AbcCompany.Orders.Domain.Entities;

namespace AbcCompany.Orders.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetAll(bool showCanceledItens = false);
        Task<OrderModel> GetById(int id, bool showCanceledItens = false);


    }
}
