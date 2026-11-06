using Microsoft.AspNetCore.Mvc; using MedicalApi.Models;
namespace MedicalApi.Controllers;
[ApiController,Route("api/[controller]")] public class AppointmentsController:ControllerBase {
 public static List<Appointment> Appointments=new();
 [HttpGet,ProducesResponseType(typeof(List<Appointment>),200)] public ActionResult<List<Appointment>> GetAll()=>Ok(Appointments);
 [HttpGet("{id}"),ProducesResponseType(typeof(Appointment),200),ProducesResponseType(404)] public ActionResult<Appointment> GetById(int id)=>Appointments.FirstOrDefault(x=>x.Id==id) is {} a?Ok(a):NotFound();
 string? Validate(Appointment a)=>!PatientsController.Patients.Any(x=>x.Id==a.PatientId)?"ѕациент не найден":string.IsNullOrWhiteSpace(a.DoctorName)||string.IsNullOrWhiteSpace(a.Specialty)?"”кажите врача и специальность":a.StartsAt<=DateTime.Now?"¬рем€ приЄма должно быть в будущем":string.IsNullOrWhiteSpace(a.Room)?"”кажите кабинет":null;
 [HttpPost,ProducesResponseType(typeof(Appointment),201),ProducesResponseType(400)] public ActionResult<Appointment> Create(Appointment a){var e=Validate(a);if(e!=null)return BadRequest(e);a.Id=Appointments.Count==0?1:Appointments.Max(x=>x.Id)+1;Appointments.Add(a);return CreatedAtAction(nameof(GetById),new{id=a.Id},a);}
 [HttpPut("{id}"),ProducesResponseType(typeof(Appointment),200),ProducesResponseType(400),ProducesResponseType(404)] public ActionResult<Appointment> Update(int id,Appointment input){var a=Appointments.FirstOrDefault(x=>x.Id==id);if(a==null)return NotFound();var e=Validate(input);if(e!=null)return BadRequest(e);input.Id=id;Appointments[Appointments.IndexOf(a)]=input;return Ok(input);}
 [HttpDelete("{id}"),ProducesResponseType(204),ProducesResponseType(404)] public ActionResult Delete(int id){var a=Appointments.FirstOrDefault(x=>x.Id==id);if(a==null)return NotFound();Appointments.Remove(a);return NoContent();}
}
