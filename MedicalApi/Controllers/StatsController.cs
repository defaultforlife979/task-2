using MedicalApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalApi.Controllers;

[ApiController, Route("api/[controller]")]
public class StatsController(MedicalDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<object> Get() => new
    {
        patientsCount = await db.Patients.CountAsync(),
        spravkiCount = await db.Spravki.CountAsync(),
        total = await db.Spravki.SumAsync(s => (decimal?)s.Price) ?? 0m
    };
}
