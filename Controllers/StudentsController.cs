namespace StudentApi.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
		_logger.LogInformation("Received a POST request to create a student.");

		if (ModelState.IsValid)
		{
			_context.Students.Add(student);
			await _context.SaveChangesAsync();
			_logger.LogInformation($"Student with ID {student.Id} created successfully.");
			return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
		}

		_logger.LogError("Model state is invalid. Could not create student.");
		return BadRequest(ModelState);
	}
}
