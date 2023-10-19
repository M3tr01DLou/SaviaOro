using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaviaOro.API.Helpers;
using SaviaOro.Shared.Data;
using SaviaOro.Shared.Entities;
using System;

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
            var course = await _context.Courses
                .FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Course course)
        {
            try
            {
                if (!string.IsNullOrEmpty(course.Photo))
                {
                    var photoUser = Convert.FromBase64String(course.Photo);
                    course.Photo = _imageHelper.UploadImageAsync(photoUser, _container);
                }
                _context.Add(course);
                await _context.SaveChangesAsync();
                return Ok(course);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest($"El curso {course.Title} ya existe en la base de datos.");
                }

                return BadRequest(dbUpdateException.InnerException.Message);
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
                    var photoUser = Convert.FromBase64String(course.Photo);
                    course.Photo = _imageHelper.UploadImageAsync(photoUser, _container);
                }
                _context.Update(course);
                await _context.SaveChangesAsync();
                return Ok(course);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest($"El curso {course.Title} ya existe en la base de datos.");
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
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            _context.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
