using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MySaleDBContext _context = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _context.Categories.ToList();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var c = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (c != null) return Ok(c);
            return NotFound();
        }


        [HttpPost]
        public  IActionResult Create([FromBody] CategoryAddUpdateDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null)
                {
                    return BadRequest("Invalid category data");
                }
                Category c = new Category
                {
                    CategoryName = categoryDTO.CategoryName
                };
                _context.Add(c);
                return StatusCode(200,  _context.SaveChanges() > 0);
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

        [HttpPut("{id}")]
        public  IActionResult Update(int id, [FromBody] CategoryAddUpdateDTO categoryDTO)
        {
            try
            {
                if (id != categoryDTO.CategoryId) return BadRequest("Category ID mismatch");
                Category c = new Category
                {
                    CategoryId = categoryDTO.CategoryId,
                    CategoryName = categoryDTO.CategoryName
                };
                _context.Entry<Category>(c).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                 _context.SaveChanges();
                return StatusCode(200,  _context.Categories.SingleOrDefault(category => category.CategoryId == categoryDTO.CategoryId));
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var c = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
                if (c != null)
                {
                    _context.Remove(c);
                    _context.SaveChanges();
                    return StatusCode(204, "Delete successfully!");
                }
                return NotFound();
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
    }
}
