using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Services;

[ApiController]
[Route("[controller]")]
public class ProductsController(ProductService productService, OrderService orderService) : ControllerBase
{
    private readonly ProductService _productService = productService;
    private readonly OrderService _orderService = orderService;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var products = await _productService.GetAllAsync();
        if (products == null || products.Count == 0)
        {
            return NotFound("No products found.");
        }
        products.ForEach(async p => p.Order = await _orderService.GetOrdersByProductIdAsync(p.Id!));

        return Ok(products);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(string id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }


    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(Product product)
    {
        if (product == null)
        {
            return BadRequest("Product cannot be null.");
        }
        await _productService.AddAsync(product);
        return Ok(product);
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Product updatedProduct)
    {
        var existingProduct = await _productService.GetByIdAsync(updatedProduct.Id!);
        if (existingProduct == null) return BadRequest();

        existingProduct.Code = updatedProduct.Code ?? existingProduct.Code;
        existingProduct.Name = updatedProduct.Name ?? existingProduct.Name;
        existingProduct.Description = updatedProduct.Description ?? existingProduct.Description;
        existingProduct.Image = updatedProduct.Image ?? existingProduct.Image;
        existingProduct.Price = updatedProduct.Price;
        existingProduct.Category = updatedProduct.Category ?? existingProduct.Category;
        existingProduct.Quantity = updatedProduct.Quantity ?? existingProduct.Quantity;
        existingProduct.InventoryStatus = updatedProduct.InventoryStatus ?? existingProduct.InventoryStatus;
        existingProduct.Rating = updatedProduct.Rating ?? existingProduct.Rating;

        await _productService.UpdateAsync(existingProduct);
        return Ok(existingProduct);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound("No products found.");
        }
        var orders = await _orderService.GetOrdersByProductIdAsync(id);
        orders.ForEach(async o => await _orderService.DeleteAsync(o.Id!));
        await _productService.DeleteAsync(id);
        return NoContent();
    }
}
