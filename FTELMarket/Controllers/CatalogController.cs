using Microsoft.AspNetCore.Mvc;
using FTELMarket.Infrastructure.Entities;
using FTELMarket.Infrastructure.Requests;
using FTELMarket.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FTELMarket.Controllers;

[ApiController]
[Route("api")]
public class CatalogController : ControllerBase
{
    private readonly FtelMarketDbContext dbContext;

    public CatalogController(FtelMarketDbContext context)
    {
        dbContext = context;
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        var categories = await dbContext.Category.ToListAsync();
        return Ok(categories);
    }


    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await dbContext.Product.ToListAsync();
        return Ok(products);
    }


    [HttpPost("products")]
    public async Task<ActionResult<Product>> CreateProduct(ProductRequest productRequest)
    {
        var cat = await dbContext.Category.FindAsync(productRequest.CategoryId);
        if (cat is null)
        {
            return NotFound();
        }
        var product = new Product
        {
            Name = productRequest.Name,
            Description = productRequest.Description,
            Price = productRequest.Price,
            Stock = productRequest.Stock,
            CategoryId = cat.Id,
            CategoryName = cat.Name,
            CreatedAt = DateTime.UtcNow
        };
        dbContext.Product.Add(product);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }

    [HttpPut("products/{id:int}")]
    public async Task<ActionResult<Product>> PutProduct(int id, ProductRequest productRequest)
    {
        var product = await dbContext.Product.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        var cat = await dbContext.Category.FindAsync(productRequest.CategoryId);
        if (cat is null)
        {
            return NotFound();
        }
        product.Name = productRequest.Name;
        product.Description = productRequest.Description;
        product.Price = productRequest.Price;
        product.Stock = productRequest.Stock;
        product.CategoryId = cat.Id;
        product.CategoryName = cat.Name;
        await dbContext.SaveChangesAsync();
        return Ok(product);
    }

    [HttpDelete("products/{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await dbContext.Product.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        dbContext.Product.Remove(product);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}