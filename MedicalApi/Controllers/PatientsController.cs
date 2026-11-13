using MedicalApi.Data;
using MedicalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalApi.Controllers;

[ApiController, Route("api/[controller]")]
public class PatientsController(MedicalDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<List<Patient>> GetAll(string? name)
    {
        IQueryable<Patient> query = db.Patients;
        if (!string.IsNullOrWhiteSpace(name)) query = query.Where(p => EF.Functions.ILike(p.Name, $"%{name}%"));
        return await query.OrderBy(p => p.Name).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(int id) => await db.Patients.FirstOrDefaultAsync(p => p.Id == id) is { } p ? p : NotFound();

    [HttpGet("{id}/spravki")]
    public async Task<ActionResult<List<Spravka>>> GetSpravki(int id)
    {
        if (!await db.Patients.AnyAsync(p => p.Id == id)) return NotFound();
        return await db.Spravki.Where(s => s.PatientId == id).OrderByDescending(s => s.IssuedAt).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        patient.Id = 0;
        var error = Validate(patient);
        if (error != null) return BadRequest(error);
        db.Patients.Add(patient);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Patient>> Update(int id, Patient input)
    {
        var patient = await db.Patients.FindAsync(id);
        if (patient == null) return NotFound();
        input.Id = id;
        var error = Validate(input);
        if (error != null) return BadRequest(error);
        patient.Name = input.Name; patient.BirthDate = input.BirthDate; patient.Phone = input.Phone;
        patient.Address = input.Address; patient.HasAllergies = input.HasAllergies; patient.Email = input.Email;
        await db.SaveChangesAsync();
        return patient;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await db.Patients.FindAsync(id);
        if (patient == null) return NotFound();
        if (await db.Spravki.AnyAsync(s => s.PatientId == id) || await db.Appointments.AnyAsync(a => a.PatientId == id))
            return BadRequest("Нельзя удалить пациента: есть справки или записи на приём.");
        db.Patients.Remove(patient);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static string? Validate(Patient p) =>
        string.IsNullOrWhiteSpace(p.Name) ? "Имя обязательно" :
        p.BirthDate == default || p.BirthDate > DateOnly.FromDateTime(DateTime.Today) ? "Укажите корректную дату рождения" :
        string.IsNullOrWhiteSpace(p.Phone) ? "Телефон обязателен" : null;
}




