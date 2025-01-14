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
	private readonly ILogger<StudentsController> _logger;

	public StudentsController(StudentDbContext context, ILogger<StudentsController> logger)
	{
		_context = context;
		_logger = logger;
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
	public async Task<IActionResult> CreateStudent([FromBody] Student student)
	{
		if (ModelState.IsValid)
		{
			try
			{
				_context.Students.Add(student);
				var result = await _context.SaveChangesAsync();

				if (result > 0)
				{
					_logger.LogInformation("Successfully saved student: {StudentName}", student.Name);
					return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
				}
				else
				{
					_logger.LogWarning("SaveChangesAsync did not affect any rows.");
				}

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while saving student.");
				return StatusCode(500, "Internal server error while creating student.");
			}
		}

		_logger.LogError("Model state is invalid. Could not create student.");
		return BadRequest(ModelState);	
	}
}
