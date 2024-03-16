using ApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ProductsContext _context;

        public ProductsController(ProductsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
           var products = await _context.Products.ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducts(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var p = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

                if (p == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(p);
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                 _context.Products.Add(entity);
                 await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProducts), new { id = entity.ProductId }, entity);
            }
        }


        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int? id, Product entity)
        {
            if(id != entity.ProductId)
            {
                return BadRequest();
            }
            else
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

                if(product == null)
                {
                    return NotFound();
                }
                else
                {
                    product.ProductName = entity.ProductName;
                    product.Price = entity.Price;
                    product.IsActive = entity.IsActive;

                    _context.Products.Update(product);

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        return NotFound();
                    }

                    return NoContent();
                }
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            else
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

                if(product == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Products.Remove(product);

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        return NotFound();
                    }

                    return NoContent();
                }
            }
        }
    }
}
