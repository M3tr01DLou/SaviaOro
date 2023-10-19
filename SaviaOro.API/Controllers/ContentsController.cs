using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SaviaOro.API.Helpers;
using SaviaOro.Shared.Data;
using SaviaOro.Shared.Entities;

namespace SaviaOro.API.Controllers
{
    [ApiController]
    [Route("/api/contents")]
    public class ContentsController : ControllerBase
    {
        private readonly DataContext _context;

        public ContentsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Contents.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var course = await _context.Contents
                .FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Content content)
        {
            try
            {
                _context.Add(content);
                await _context.SaveChangesAsync();
                return Ok(content);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest($"El contenido {content.Title} ya existe para el mismo curso en la base de datos.");
                }

                return BadRequest(dbUpdateException.InnerException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Content content)
        {
            try
            {
                _context.Update(content);
                await _context.SaveChangesAsync();
                return Ok(content);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest($"El contenido {content.Title} ya existe para el mismo curso en la base de datos.");
                }

                return BadRequest(dbUpdateException.InnerException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var content = await _context.Contents.FirstOrDefaultAsync(x => x.Id == id);
            if (content == null)
            {
                return NotFound();
            }
            _context.Remove(content);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
