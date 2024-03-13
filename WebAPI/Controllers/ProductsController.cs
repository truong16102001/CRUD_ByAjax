using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MySaleDBContext _context = new();


        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                // Use Include to eagerly load the related Category
                var data = _context.Products.Include(p => p.Category).ToList();

                if (data == null)
                {
                    return NotFound();
                }

                return Ok(data);
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var c = _context.Products.FirstOrDefault(p => p.ProductId == id);
                if (c == null) return NotFound();

                return Ok(c);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProductAddUpdateDTO product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Invalid product data");
                }

                Product productAdd = new Product
                {
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    Image = product.Image,
                    CategoryId = product.CategoryId
                };


                _context.Add(productAdd);
                _context.SaveChanges();
                return StatusCode(200, _context.SaveChanges() > 0);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductAddUpdateDTO productModel)
        {

            try
            {
                if (id != productModel.ProductId) return BadRequest("Product Id mismatch");
                var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
                if (product != null)
                {

                    product.ProductName = productModel.ProductName;
                    product.UnitPrice = productModel.UnitPrice;
                    product.UnitsInStock = productModel.UnitsInStock;
                    product.Image = productModel.Image;
                    product.CategoryId = productModel.CategoryId;


                    _context.SaveChanges();

                    return Ok(product);
                }
                return NotFound();

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existingProduct = _context.Products.FirstOrDefault(c => c.ProductId == id);

                if (existingProduct == null)
                {
                    return NotFound();
                }

                _context.Remove(existingProduct);
                _context.SaveChanges();
                return Ok(_context.Products.ToList());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
