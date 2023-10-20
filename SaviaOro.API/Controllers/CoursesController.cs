using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaviaOro.API.Data;
using SaviaOro.API.Helpers;
using SaviaOro.Shared.Entities;

namespace SaviaOro.API.Controllers
{
    [ApiController]
    [Route("/api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;
        private readonly string _container;

        public CoursesController(DataContext context, IImageHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
            _container = "courses";
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Courses.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            Course course = await _context.Courses
                .Include(c => c.Contents)
                .FirstOrDefaultAsync(x => x.Id == id);
            return course == null ? NotFound() : Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Course course)
        {
            try
            {
                if (!string.IsNullOrEmpty(course.Photo))
                {
                    byte[] photoUser = Convert.FromBase64String(course.Photo);
                    course.Photo = _imageHelper.UploadImageAsync(photoUser, _container);
                }
                _ = _context.Add(course);
                _ = await _context.SaveChangesAsync();
                return Ok(course);
            }
            catch (DbUpdateException dbUpdateException)
            {
                return dbUpdateException.InnerException.Message.Contains("duplicate")
                    ? BadRequest($"El curso {course.Title} ya existe en la base de datos.")
                    : (ActionResult)BadRequest(dbUpdateException.InnerException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Course course)
        {
            try
            {
                if (!string.IsNullOrEmpty(course.Photo))
                {
                    byte[] photoUser = Convert.FromBase64String(course.Photo);
                    course.Photo = _imageHelper.UploadImageAsync(photoUser, _container);
                }
                _ = _context.Update(course);
                _ = await _context.SaveChangesAsync();
                return Ok(course);
            }
            catch (DbUpdateException dbUpdateException)
            {
                return dbUpdateException.InnerException.Message.Contains("duplicate")
                    ? BadRequest($"El curso {course.Title} ya existe en la base de datos.")
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
            Course course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            _ = _context.Remove(course);
            _ = await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
