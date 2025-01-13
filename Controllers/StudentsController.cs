namespace StudentApi.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
	private readonly StudentDbContext _context;

	public StudentsController(StudentDbContext context)
	{
		_context = context;
	}
	
	// GET: api/students
	[HttpGet]
	public async Task<IActionResult> GetStudents()
	{
		var students = await _context.Students.ToListAsync();
		return Ok(students);
	}
		
	// GET: api/students/{id}
	[HttpGet("{id}")]
	public async Task<IActionResult> GetStudent(int id)
	{
		var student = await _context.Students.FindAsync(id);

		if (student == null)
		{
			return NotFound();
		}

		return Ok(student);
	}
	
	// POST: api/students
	[HttpPost]
	[Route("api/students")]
	public async Task<IActionResult> CreateStudent([FromBody] Student student)
	{
		if (ModelState.IsValid)
		{
			_context.Students.Add(student);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
		}

		return BadRequest(ModelState);
	}
}
