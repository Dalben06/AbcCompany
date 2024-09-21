using AbcCompany.Orders.Domain.Entities;

namespace AbcCompany.Orders.Domain.IRepositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll(bool showItensCanceled = false);
        Task<Order> Get(int id, bool showItensCanceled = false);
        Task<Order> GetByOrderNumber(int orderNumber, bool showItensCanceled = false);
        Task<int> GetNextOrderNumber();
        Task<Order> Add(Order order);
        Task<bool> Update(Order order);
        Task<bool> Cancel(Order order);

    }
}
