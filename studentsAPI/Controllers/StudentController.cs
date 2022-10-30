using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace studentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> Get()
        {
            return Ok(await _context.Students.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return BadRequest("Student not found..");
            return Ok(student);
        }
        [HttpGet("/api/Student/find")]
        public async Task<ActionResult<Student>> Get(string find ="")
        {
           var student =  _context.Students.AsQueryable();
            if (!string.IsNullOrEmpty(find))
            {
                student =  student.Where(c => c.name.Contains(find));
                student = student.OrderBy(c => c.name);
                await student.ToListAsync();
                if (student == null)
                    return BadRequest("Student not found..");
            }
            return Ok(student);
        }
        [HttpPost]
        public async Task<ActionResult<List<Student>>> Post(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return Ok(await _context.Students.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Student>>> Put(Student request)
        {
            var dbStudent = await _context.Students.FindAsync(request.Id);
            if (dbStudent == null)
                return BadRequest("Student not found.");

            dbStudent.name = request.name;
            dbStudent.email = request.email;
           

            await _context.SaveChangesAsync();

            return Ok(await _context.Students.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Student>>> Delete(int id)
        {
            var dbStudent = await _context.Students.FindAsync(id);
            if (dbStudent == null)
                return BadRequest("Student not found.");

            _context.Students.Remove(dbStudent);
            await _context.SaveChangesAsync();

            return Ok(await _context.Students.ToListAsync());
        }

    }
}
