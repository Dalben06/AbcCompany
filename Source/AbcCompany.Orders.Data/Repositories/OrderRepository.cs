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

        public async Task<Order> Get(int id, bool showItensCanceled = false)
        {
            var dic = new Dictionary<int, Order>();
            await _dbContext.DbConnection.QueryAsync<Order, OrderPayment, OrderProduct, Order>(DefaultSql(showItensCanceled) + " AND Orders.Id = @id", SQLMap(dic), new { id });
            return dic.FirstOrDefault().Value;
        }

        public async Task<IEnumerable<Order>> GetAll(bool showItensCanceled = false)
        {
            var dic = new Dictionary<int, Order>();
            await _dbContext.DbConnection.QueryAsync<Order, OrderPayment, OrderProduct, Order>(DefaultSql(showItensCanceled), SQLMap(dic));
            return dic.Values;
        }

        public async Task<Order> GetByOrderNumber(int orderNumber, bool showItensCanceled = false)
        {
            var dic = new Dictionary<int, Order>();
            await _dbContext.DbConnection.QueryAsync<Order, OrderPayment, OrderProduct, Order>(DefaultSql(showItensCanceled) + " AND Orders.OrderNumber = @orderNumber", SQLMap(dic), new { orderNumber });
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
        public async Task<bool> Update(Order order)
        {
            var updates = new List<bool>();
            updates.Add(await _dbContext.DbConnection.UpdateAsync(order));
            updates.Add(await _dbContext.DbConnection.UpdateAsync(order.Payments.Where(p => p.Id != 0)));
            updates.Add(await _dbContext.DbConnection.UpdateAsync(order.Products.Where(p => p.Id != 0)));

            await _dbContext.DbConnection.InsertAsync(order.Payments.Where(p => p.Id == 0));
            await _dbContext.DbConnection.InsertAsync(order.Products.Where(p => p.Id == 0));
            return !updates.Any(c => c == false);
        }

        public async Task<bool> Cancel(Order order)
        {
            await _dbContext.DbConnection.UpdateAsync(order);
            await _dbContext.DbConnection.UpdateAsync(order.Products);
            await _dbContext.DbConnection.UpdateAsync(order.Payments);
            return true;
        }


        private string DefaultSql(bool showItensCanceled = false)
        {
            var sql =  @"
                SELECT Orders.*, pay.*,  pr.*  FROM Orders
                 JOIN OrderPayments as pay
                 on pay.OrderId = Orders.Id

                 JOIN OrderProducts as pr
                 on pr.OrderId = Orders.Id
                  WHERE 1 = 1 
            ";

            if (!showItensCanceled)
                sql += @"
                    AND pay.OrderPaymentStatusId = 1 AND pr.OrderProductStatusId = 1  
                ";

            return sql;
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
