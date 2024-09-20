using AbcCompany.Orders.Domain.Entities;

namespace AbcCompany.Orders.Domain.IRepositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order Get(int id);
        Order GetByOrderNumber(int orderNumber);
        int GetNextOrderNumber();
        Order Add(Order order);
        //bool Update(Order order);
        bool Cancel(Order order);

    }
}
