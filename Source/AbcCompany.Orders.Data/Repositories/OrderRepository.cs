using AbcCompany.Core.Domain.Data;
using AbcCompany.Orders.Domain.Entities;
using AbcCompany.Orders.Domain.IRepositories;
using Dapper;
using Dapper.Contrib.Extensions;

namespace AbcCompany.Orders.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContext _dbContext;

        public OrderRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Order Get(int id)
        {
            var dic = new Dictionary<int, Order>();
            _dbContext.DbConnection.Query<Order, OrderPayment, OrderProduct, Order>(DefaultSql() + " AND Orders.Id = @id", SQLMap(dic), new { id });
            return dic.FirstOrDefault().Value;
        }

        public IEnumerable<Order> GetAll()
        {
            var dic = new Dictionary<int, Order>();
            _dbContext.DbConnection.Query<Order, OrderPayment, OrderProduct, Order>(DefaultSql(), SQLMap(dic));
            return dic.Values;
        }

        public Order GetByOrderNumber(int orderNumber)
        {
            var dic = new Dictionary<int, Order>();
            _dbContext.DbConnection.Query<Order, OrderPayment, OrderProduct, Order>(DefaultSql() + " AND Orders.OrderNumber = @orderNumber", SQLMap(dic), new { orderNumber });
            return dic.FirstOrDefault().Value;
        }

        public int GetNextOrderNumber()
        {
            var sql = "SELECT  ISNULL(MAX(OrderNumber), 0) + 1 AS NextOrderNumber FROM  [Orders]";
            return _dbContext.DbConnection.QueryFirstOrDefault<int>(sql);
        }
        public Order Add(Order order)
        {
            order.Id = (int)_dbContext.DbConnection.Insert(order);
            order.SetOrderIdInProducts();
            order.SetOrderIdInPayments();

            _dbContext.DbConnection.Insert(order.Payments);
            _dbContext.DbConnection.Insert(order.Products);
            return order;
        }
        public bool Update(Order order)
        {
            return _dbContext.DbConnection.Update(order);
        }

        public bool Cancel(Order order)
        {
            var sql = "UPDATE Orders SET IdStatusOrder = @OrderStatusId WHERE Id = @Id";
            return _dbContext.DbConnection.Execute(sql, order) > 0;
        }


        private string DefaultSql()
        {
            return @"
                SELECT Orders.*, pay.*,  pr.*  FROM Orders
                 JOIN OrderPayments as pay
                 on pay.IdOrder = Orders.Id

                 JOIN OrderProducts as pr
                 on pr.IdOrder = Orders.Id
                  WHERE 1 = 1 
            ";
        }


        private Func<Order, OrderPayment, OrderProduct, Order> SQLMap(Dictionary<int, Order> dic)
        {
            return (order, payment, product) =>
            {
                if (!dic.TryGetValue(order.Id, out Order existingOrder))
                {
                    existingOrder = order;
                    dic.Add(order.Id, existingOrder);
                }

                // Adiciona o produto à lista de produtos do pedido
                if (product != null && !existingOrder.Products.Contains(product))
                {
                    existingOrder.Products.Add(product);
                }

                // Adiciona o pagamento à lista de pagamentos do pedido
                if (payment != null && !existingOrder.Payments.Contains(payment))
                {
                    existingOrder.Payments.Add(payment);
                }

                return existingOrder;
            };

        }

    }
}
