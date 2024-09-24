using AbcCompany.Core.Domain.Communication.Mediator;
using AbcCompany.Orders.Application.Commands;
using AbcCompany.Orders.Application.Interfaces;
using AbcCompany.Orders.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace AbcCompany.WebApp.API.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMediatorHandler _mediatorHandler;

        public OrderController(IOrderService orderService, IMediatorHandler mediatorHandler)
        {
            _orderService = orderService;
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] bool showCanceled = false)
        {
            return CustomResponse(await _orderService.GetAll(showCanceled));
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CustomResponse(await _orderService.GetById(id));
        }

        [HttpPost("RegisterOrder")]
        public async Task<IActionResult> RegisterOrder(OrderModel model)
        {
            var res = await _mediatorHandler.SendCommand<RegisterNewOrderCommand, OrderModel>(new RegisterNewOrderCommand(
                model.ClientId,model.ClientName, model.BranchId, model.BranchName, model.Payments, model.Products));

            if(!res.Validation.IsValid) return CustomResponse(res.Validation);

            return CustomResponse(res.Model);

        }

        [HttpPost("CancelOrder/{id}")]
        public async Task<IActionResult> RegisterOrder(int id)
        {
            var res = await _mediatorHandler.SendCommand<CancelOrderCommand, OrderModel>(new CancelOrderCommand(id));

            if (!res.Validation.IsValid) return CustomResponse(res.Validation);

            return CustomResponse(res.Model);

        }


        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(ChangeOrderModel model)
        {
            var res = await _mediatorHandler.SendCommand<UpdateOrderAndProductsCommand, OrderModel>(new UpdateOrderAndProductsCommand(
                model.Id,model.ProductsToCancel,model.PaymentsToCancel,model.Products,model.Payments));

            if (!res.Validation.IsValid) return CustomResponse(res.Validation);

            return CustomResponse(res.Model);

        }

    }
}
