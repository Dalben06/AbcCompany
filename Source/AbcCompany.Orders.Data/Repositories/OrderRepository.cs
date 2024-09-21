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

        public async Task<Order> Get(int id)
        {
            var dic = new Dictionary<int, Order>();
            await _dbContext.DbConnection.QueryAsync<Order, OrderPayment, OrderProduct, Order>(DefaultSql() + " AND Orders.Id = @id", SQLMap(dic), new { id });
            return dic.FirstOrDefault().Value;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            var dic = new Dictionary<int, Order>();
            await _dbContext.DbConnection.QueryAsync<Order, OrderPayment, OrderProduct, Order>(DefaultSql(), SQLMap(dic));
            return dic.Values;
        }

        public async Task<Order> GetByOrderNumber(int orderNumber)
        {
            var dic = new Dictionary<int, Order>();
            await _dbContext.DbConnection.QueryAsync<Order, OrderPayment, OrderProduct, Order>(DefaultSql() + " AND Orders.OrderNumber = @orderNumber", SQLMap(dic), new { orderNumber });
            return dic.FirstOrDefault().Value;
        }

        public async Task<int> GetNextOrderNumber()
        {
            var sql = "SELECT  ISNULL(MAX(OrderNumber), 0) + 1 AS NextOrderNumber FROM  [Orders]";
            return await _dbContext.DbConnection.QueryFirstOrDefaultAsync<int>(sql);
        }
        public async Task<Order> Add(Order order)
        {
            order.Id = (int) await _dbContext.DbConnection.InsertAsync(order);
            order.SetOrderIdInProducts();
            order.SetOrderIdInPayments();

            await _dbContext.DbConnection.InsertAsync(order.Payments);
            await _dbContext.DbConnection.InsertAsync(order.Products);
            return order;
        }
        public bool Update(Order order)
        {
            return _dbContext.DbConnection.Update(order);
        }

        public async Task<bool> Cancel(Order order)
        {
            var sql = "UPDATE Orders SET IdStatusOrder = @OrderStatusId WHERE Id = @Id";
            return await _dbContext.DbConnection.ExecuteAsync(sql, order) > 0;
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
