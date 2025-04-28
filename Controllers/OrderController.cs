using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController(OrderService orderService, ProductService productService) : ControllerBase
    {
        private readonly OrderService _orderService = orderService;
        private readonly ProductService _productService = productService;
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get() => Ok(await _orderService.GetAllAsync());

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Order order)
        {
            var existingProduct = await _productService.GetByIdAsync(order.ProductId!);
            if (order == null || existingProduct == null)
            {
                return BadRequest();
            }
            await _orderService.AddAsync(order);
            return Ok(order);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update(Order order)
        {
            var existingOrder = await _orderService.GetByIdAsync(order.Id!);
            var existingProduct = await _productService.GetByIdAsync(order.ProductId!);
            if (existingOrder == null || existingProduct == null) return BadRequest();
            await _orderService.UpdateAsync(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            await _orderService.DeleteAsync(id);
            return NoContent();
        }
    }
}
