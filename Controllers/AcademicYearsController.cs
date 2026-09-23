using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagment.Data;
using StudentManagment.Models;

namespace StudentManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicYearsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AcademicYearsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AcademicYear>>> GetAcademicYears()
        {
            return await _context.AcademicYears
                .OrderByDescending(x => x.AcademicYearName)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AcademicYear>> GetAcademicYear(int id)
        {
            var academicYear = await _context.AcademicYears
                .FirstOrDefaultAsync(x => x.AcademicYearId == id);

            if (academicYear == null)
                return NotFound();

            return academicYear;
        }

        [HttpPost]
        public async Task<ActionResult<AcademicYear>> CreateAcademicYear(
            AcademicYear academicYear)
        {
            academicYear.CreatedDate = DateTime.Now;
            academicYear.IsActive = true;

            _context.AcademicYears.Add(academicYear);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAcademicYear),
                new { id = academicYear.AcademicYearId },
                academicYear);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAcademicYear(
            int id,
            AcademicYear academicYear)
        {
            if (id != academicYear.AcademicYearId)
                return BadRequest();

            _context.Entry(academicYear).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAcademicYear(int id)
        {
            var academicYear = await _context.AcademicYears
                .FindAsync(id);

            if (academicYear == null)
                return NotFound();

            academicYear.IsActive = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}