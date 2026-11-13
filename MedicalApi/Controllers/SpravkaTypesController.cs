using MedicalApi.Data;
using MedicalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalApi.Controllers;

[ApiController, Route("api/[controller]")]
public class SpravkaTypesController(MedicalDbContext db) : ControllerBase
{
    [HttpGet] public async Task<List<SpravkaType>> GetAll() => await db.SpravkaTypes.OrderBy(t => t.Id).ToListAsync();
    [HttpGet("{id}")] public async Task<ActionResult<SpravkaType>> GetById(int id) => await db.SpravkaTypes.FindAsync(id) is { } t ? t : NotFound();

    [HttpPost]
    public async Task<ActionResult<SpravkaType>> Create(SpravkaType input)
    {
        input.Id = 0; var error = Validate(input); if (error != null) return BadRequest(error);
        db.SpravkaTypes.Add(input); await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SpravkaType>> Update(int id, SpravkaType input)
    {
        var t = await db.SpravkaTypes.FindAsync(id); if (t == null) return NotFound();
        var error = Validate(input); if (error != null) return BadRequest(error);
        t.Name = input.Name; t.Price = input.Price; t.ValidDays = input.ValidDays; t.RequiresDoctorExam = input.RequiresDoctorExam; t.AgeGroup = input.AgeGroup;
        await db.SaveChangesAsync(); return t;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await db.SpravkaTypes.FindAsync(id); if (t == null) return NotFound();
        if (await db.Spravki.AnyAsync(s => s.SpravkaTypeId == id)) return BadRequest("Нельзя удалить вид справки: по нему уже выданы справки.");
        db.SpravkaTypes.Remove(t); await db.SaveChangesAsync(); return NoContent();
    }

    private static string? Validate(SpravkaType t) => string.IsNullOrWhiteSpace(t.Name) ? "Название обязательно" : t.Price <= 0 ? "Цена должна быть больше 0" : t.ValidDays <= 0 ? "Срок действия должен быть больше 0" : null;
}
