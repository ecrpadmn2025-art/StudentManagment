using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagment.Data;
using StudentManagment.Models;

namespace StudentManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Departments
                .Where(x => x.IsActive)
                .OrderBy(x => x.DepartmentName)
                .ToListAsync();
        }

        // GET: api/Departments/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(x =>
                    x.DepartmentId == id &&
                    x.IsActive);

            if (department == null)
                return NotFound();

            return department;
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment(
            Department department)
        {
            if (string.IsNullOrWhiteSpace(department.DepartmentCode))
                return BadRequest("Department Code is required.");

            if (string.IsNullOrWhiteSpace(department.DepartmentName))
                return BadRequest("Department Name is required.");

            bool exists = await _context.Departments.AnyAsync(x =>
                x.DepartmentCode == department.DepartmentCode);

            if (exists)
                return BadRequest("Department Code already exists.");

            department.IsActive = true;
            department.CreatedDate = DateTime.Now;

            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDepartment),
                new { id = department.DepartmentId },
                department);
        }

        // PUT: api/Departments/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(
            int id,
            Department department)
        {
            if (id != department.DepartmentId)
                return BadRequest();

            var existingDepartment = await _context.Departments
                .FirstOrDefaultAsync(x => x.DepartmentId == id);

            if (existingDepartment == null)
                return NotFound();

            existingDepartment.DepartmentCode =
                department.DepartmentCode;

            existingDepartment.DepartmentName =
                department.DepartmentName;

            existingDepartment.IsActive =
                department.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Departments/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(x => x.DepartmentId == id);

            if (department == null)
                return NotFound();

            // Soft Delete
            department.IsActive = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}