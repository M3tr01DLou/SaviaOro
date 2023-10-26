using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaviaOro.API.Data;
using SaviaOro.Shared.Entities;

namespace SaviaOro.API.Controllers
{
    [ApiController]
    [Route("/api/contents")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            Content course = await _context.Contents
                .FirstOrDefaultAsync(x => x.Id == id);
            return course == null ? NotFound() : Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Content content)
        {
            try
            {
                _ = _context.Add(content);
                _ = await _context.SaveChangesAsync();
                return Ok(content);
            }
            catch (DbUpdateException dbUpdateException)
            {
                return dbUpdateException.InnerException.Message.Contains("duplicate")
                    ? BadRequest($"El contenido {content.Title} ya existe para el mismo curso en la base de datos.")
                    : (ActionResult)BadRequest(dbUpdateException.InnerException.Message);
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
                _ = _context.Update(content);
                _ = await _context.SaveChangesAsync();
                return Ok(content);
            }
            catch (DbUpdateException dbUpdateException)
            {
                return dbUpdateException.InnerException.Message.Contains("duplicate")
                    ? BadRequest($"El contenido {content.Title} ya existe para el mismo curso en la base de datos.")
                    : (ActionResult)BadRequest(dbUpdateException.InnerException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Content content = await _context.Contents.FirstOrDefaultAsync(x => x.Id == id);
            if (content == null)
            {
                return NotFound();
            }
            _ = _context.Remove(content);
            _ = await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
