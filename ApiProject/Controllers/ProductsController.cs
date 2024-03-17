using ApiProject.DTO;
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
            var productsFromDatabase = await _context.Products.ToListAsync();

            var selectedDtoFields = productsFromDatabase.Select(x => new ProductDTO
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
            });

            return Ok(selectedDtoFields);
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
                    var dtoField = new ProductDTO
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        Price = p.Price,
                    };

                    return Ok(dtoField);
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDTO dtoEntity)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var productModel = new Product
                {
                    ProductName = dtoEntity.ProductName,
                    Price = dtoEntity.Price,
                };

                 _context.Products.Add(productModel);
                 await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProducts), new { id = productModel.ProductId }, dtoEntity);
            }
        }


        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int? id, ProductDTO entityDTO)
        {
            if(id != entityDTO.ProductId)
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
                    product.ProductName = entityDTO.ProductName;
                    product.Price = entityDTO.Price;

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
