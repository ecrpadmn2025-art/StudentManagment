using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagment.Data;
using StudentManagment.Models;

namespace StudentManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgrammesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProgrammesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Programmes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Programme>>> GetProgrammes()
        {
            return await _context.Programme
                .Where(x => x.IsActive)
                .OrderBy(x => x.ProgrammeName)
                .ToListAsync();
        }

        // GET: api/Programmes/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Programme>> GetProgramme(int id)
        {
            var programme = await _context.Programme
                .FirstOrDefaultAsync(x =>
                    x.ProgrammeId == id &&
                    x.IsActive);

            if (programme == null)
                return NotFound();

            return programme;
        }

        // POST: api/Programmes
        [HttpPost]
        public async Task<ActionResult<Programme>> CreateProgramme(
            Programme programme)
        {
            if (programme.DepartmentId <= 0)
                return BadRequest("Department is required.");

            if (string.IsNullOrWhiteSpace(programme.ProgrammeCode))
                return BadRequest("Programme Code is required.");

            if (string.IsNullOrWhiteSpace(programme.ProgrammeName))
                return BadRequest("Programme Name is required.");

            if (string.IsNullOrWhiteSpace(programme.Duration))
                return BadRequest("Duration is required.");

            bool departmentExists = await _context.Departments
                .AnyAsync(x =>
                    x.DepartmentId == programme.DepartmentId &&
                    x.IsActive);

            if (!departmentExists)
                return BadRequest("Selected Department does not exist.");

            bool exists = await _context.Programme
                .AnyAsync(x =>
                    x.ProgrammeCode == programme.ProgrammeCode &&
                    x.IsActive);

            if (exists)
                return BadRequest("Programme Code already exists.");

            programme.IsActive = true;
            programme.CreatedDate = DateTime.Now;

            _context.Programme.Add(programme);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProgramme),
                new { id = programme.ProgrammeId },
                programme);
        }

        // PUT: api/Programmes/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgramme(
            int id,
            Programme programme)
        {
            if (id != programme.ProgrammeId)
                return BadRequest();

            var existingProgramme = await _context.Programme
                .FirstOrDefaultAsync(x => x.ProgrammeId == id);

            if (existingProgramme == null)
                return NotFound();

            if (programme.DepartmentId <= 0)
                return BadRequest("Department is required.");

            if (string.IsNullOrWhiteSpace(programme.ProgrammeCode))
                return BadRequest("Programme Code is required.");

            if (string.IsNullOrWhiteSpace(programme.ProgrammeName))
                return BadRequest("Programme Name is required.");

            if (string.IsNullOrWhiteSpace(programme.Duration))
                return BadRequest("Duration is required.");

            bool departmentExists = await _context.Departments
                .AnyAsync(x =>
                    x.DepartmentId == programme.DepartmentId &&
                    x.IsActive);

            if (!departmentExists)
                return BadRequest("Selected Department does not exist.");

            bool duplicate = await _context.Programme
                .AnyAsync(x =>
                    x.ProgrammeCode == programme.ProgrammeCode &&
                    x.ProgrammeId != id &&
                    x.IsActive);

            if (duplicate)
                return BadRequest("Programme Code already exists.");

            existingProgramme.DepartmentId =
                programme.DepartmentId;

            existingProgramme.ProgrammeCode =
                programme.ProgrammeCode;

            existingProgramme.ProgrammeName =
                programme.ProgrammeName;

            existingProgramme.Duration =
                programme.Duration;

            existingProgramme.IsActive =
                programme.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Programmes/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgramme(int id)
        {
            var programme = await _context.Programme
                .FirstOrDefaultAsync(x => x.ProgrammeId == id);

            if (programme == null)
                return NotFound();

            // Soft Delete
            programme.IsActive = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}