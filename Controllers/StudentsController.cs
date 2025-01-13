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
	
	[HttpGet]
	public async Task<IActionResult> GetStudents()
	{
		var students = await _context.Students.ToListAsync();
		return Ok(students);
	}
	
	[HttpPost("api/students")]
	public async Task<IActionResult> CreateStudent([FromBody] Student student)
	{
		if (ModelState.IsValid)
		{
			_context.Students.Add(student);
			await _context.SaveChangesAsync();
			return CreatedAtAction("GetStudent", new { id = student.Id }, student);
		}

		return BadRequest(ModelState);
	}
}
