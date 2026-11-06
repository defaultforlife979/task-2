using Microsoft.AspNetCore.Mvc; using MedicalApi.Models;
namespace MedicalApi.Controllers;
[ApiController,Route("api/[controller]")] public class PatientsController:ControllerBase {
 public static List<Patient> Patients=new();
 [HttpGet,ProducesResponseType(typeof(List<Patient>),200)] public ActionResult<List<Patient>> GetAll()=>Ok(Patients);
 [HttpGet("{id}"),ProducesResponseType(typeof(Patient),200),ProducesResponseType(404)] public ActionResult<Patient> GetById(int id)=>Patients.FirstOrDefault(x=>x.Id==id) is {} p?Ok(p):NotFound();
 [HttpGet("{id}/spravki"),ProducesResponseType(typeof(List<Spravka>),200),ProducesResponseType(404)] public ActionResult<List<Spravka>> GetSpravki(int id)=>Patients.Any(x=>x.Id==id)?Ok(SpravkiController.Spravki.Where(x=>x.PatientId==id).ToList()):NotFound();
 string? Validate(Patient p)=>string.IsNullOrWhiteSpace(p.Name)?"Имя обязательно":p.BirthDate==default||p.BirthDate>DateTime.Today?"Укажите корректную дату рождения":string.IsNullOrWhiteSpace(p.Phone)?"Телефон обязателен":null;
 [HttpPost,ProducesResponseType(typeof(Patient),201),ProducesResponseType(400)] public ActionResult<Patient> Create(Patient p){var e=Validate(p);if(e!=null)return BadRequest(e);p.Id=Patients.Count==0?1:Patients.Max(x=>x.Id)+1;Patients.Add(p);return CreatedAtAction(nameof(GetById),new{id=p.Id},p);}
 [HttpPut("{id}"),ProducesResponseType(typeof(Patient),200),ProducesResponseType(400),ProducesResponseType(404)] public ActionResult<Patient> Update(int id,Patient input){var p=Patients.FirstOrDefault(x=>x.Id==id);if(p==null)return NotFound();var e=Validate(input);if(e!=null)return BadRequest(e);input.Id=id;Patients[Patients.IndexOf(p)]=input;return Ok(input);}
 [HttpDelete("{id}"),ProducesResponseType(204),ProducesResponseType(404)] public ActionResult Delete(int id){var p=Patients.FirstOrDefault(x=>x.Id==id);if(p==null)return NotFound();Patients.Remove(p);SpravkiController.Spravki.RemoveAll(x=>x.PatientId==id);AppointmentsController.Appointments.RemoveAll(x=>x.PatientId==id);return NoContent();}
}
