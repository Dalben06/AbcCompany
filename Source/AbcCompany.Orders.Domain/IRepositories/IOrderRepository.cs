using AbcCompany.Orders.Domain.Entities;

namespace AbcCompany.Orders.Domain.IRepositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order> Get(int id);
        Task<Order> GetByOrderNumber(int orderNumber);
        Task<int> GetNextOrderNumber();
        Task<Order> Add(Order order);
        //bool Update(Order order);
        Task<bool> Cancel(Order order);

    }
}
