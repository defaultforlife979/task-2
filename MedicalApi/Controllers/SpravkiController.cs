using MedicalApi.Data;
using MedicalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalApi.Controllers;

[ApiController, Route("api/[controller]")]
public class SpravkiController(MedicalDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<List<Spravka>> GetAll(int? patientId, decimal? minPrice, string? sort)
    {
        IQueryable<Spravka> query = db.Spravki;
        if (patientId.HasValue) query = query.Where(s => s.PatientId == patientId.Value);
        if (minPrice.HasValue) query = query.Where(s => s.Price >= minPrice.Value);
        query = sort?.ToLowerInvariant() switch { "price" => query.OrderBy(s => s.Price), "-price" => query.OrderByDescending(s => s.Price), _ => query.OrderByDescending(s => s.IssuedAt) };
        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Spravka>> GetById(int id) => await db.Spravki.FirstOrDefaultAsync(s => s.Id == id) is { } s ? s : NotFound();

    [HttpPost]
    public async Task<ActionResult<Spravka>> Create(Spravka input)
    {
        input.Id = 0;
        if (!await db.Patients.AnyAsync(p => p.Id == input.PatientId)) return BadRequest("Пациент не найден");
        var type = await db.SpravkaTypes.FindAsync(input.SpravkaTypeId);
        if (type == null) return BadRequest("Вид справки не найден");
        input.Name = type.Name; input.Price = type.Price; input.IssuedAt = DateOnly.FromDateTime(DateTime.Today);
        db.Spravki.Add(input);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Spravka>> Update(int id, Spravka input)
    {
        var existing = await db.Spravki.FindAsync(id);
        if (existing == null) return NotFound();
        if (!await db.Patients.AnyAsync(p => p.Id == input.PatientId)) return BadRequest("Пациент не найден");
        var type = await db.SpravkaTypes.FindAsync(input.SpravkaTypeId);
        if (type == null) return BadRequest("Вид справки не найден");
        existing.PatientId = input.PatientId; existing.SpravkaTypeId = input.SpravkaTypeId;
        existing.Name = type.Name; existing.Price = type.Price;
        await db.SaveChangesAsync();
        return existing;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.Spravki.FindAsync(id);
        if (item == null) return NotFound();
        db.Spravki.Remove(item); await db.SaveChangesAsync(); return NoContent();
    }
}
