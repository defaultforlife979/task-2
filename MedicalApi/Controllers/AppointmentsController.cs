using MedicalApi.Data;
using MedicalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalApi.Controllers;

[ApiController, Route("api/[controller]")]
public class AppointmentsController(MedicalDbContext db) : ControllerBase
{
    [HttpGet] public async Task<List<Appointment>> GetAll() => await db.Appointments.OrderBy(a => a.StartsAt).ToListAsync();
    [HttpGet("{id}")] public async Task<ActionResult<Appointment>> GetById(int id) => await db.Appointments.FindAsync(id) is { } a ? a : NotFound();

    [HttpPost]
    public async Task<ActionResult<Appointment>> Create(Appointment input)
    {
        input.Id = 0; var error = await Validate(input); if (error != null) return BadRequest(error);
        db.Appointments.Add(input); await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Appointment>> Update(int id, Appointment input)
    {
        var a = await db.Appointments.FindAsync(id); if (a == null) return NotFound();
        var error = await Validate(input); if (error != null) return BadRequest(error);
        a.PatientId = input.PatientId; a.DoctorName = input.DoctorName; a.Specialty = input.Specialty; a.StartsAt = input.StartsAt; a.Room = input.Room;
        await db.SaveChangesAsync(); return a;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var a = await db.Appointments.FindAsync(id); if (a == null) return NotFound();
        db.Appointments.Remove(a); await db.SaveChangesAsync(); return NoContent();
    }

    private async Task<string?> Validate(Appointment a) =>
        !await db.Patients.AnyAsync(p => p.Id == a.PatientId) ? "Пациент не найден" :
        string.IsNullOrWhiteSpace(a.DoctorName) || string.IsNullOrWhiteSpace(a.Specialty) ? "Укажите врача и специальность" :
        a.StartsAt == default || a.StartsAt <= DateTime.Now ? "Время приёма должно быть в будущем" :
        string.IsNullOrWhiteSpace(a.Room) ? "Укажите кабинет" : null;
}
