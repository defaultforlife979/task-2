using Microsoft.AspNetCore.Mvc; using MedicalApi.Models;
namespace MedicalApi.Controllers;
[ApiController,Route("api/[controller]")] public class SpravkiController:ControllerBase {
 public static List<Spravka> Spravki=new();
 [HttpGet,ProducesResponseType(typeof(List<Spravka>),200)] public ActionResult<List<Spravka>> GetAll(int? patientId)=>Ok(patientId==null?Spravki:Spravki.Where(x=>x.PatientId==patientId).ToList());
 [HttpGet("{id}"),ProducesResponseType(typeof(Spravka),200),ProducesResponseType(404)] public ActionResult<Spravka> GetById(int id)=>Spravki.FirstOrDefault(x=>x.Id==id) is {} s?Ok(s):NotFound();
 string? Validate(Spravka s)=>!PatientsController.Patients.Any(x=>x.Id==s.PatientId)?"Пациент не найден":SpravkaTypesController.Types.All(x=>x.Id!=s.SpravkaTypeId)?"Вид справки не найден":null;
 [HttpPost,ProducesResponseType(typeof(Spravka),201),ProducesResponseType(400)] public ActionResult<Spravka> Create(Spravka s){var e=Validate(s);if(e!=null)return BadRequest(e);var t=SpravkaTypesController.Types.First(x=>x.Id==s.SpravkaTypeId);s.Id=Spravki.Count==0?1:Spravki.Max(x=>x.Id)+1;s.Name=t.Name;s.Price=t.Price;s.IssuedAt=DateTime.Today;Spravki.Add(s);return CreatedAtAction(nameof(GetById),new{id=s.Id},s);}
 [HttpPut("{id}"),ProducesResponseType(typeof(Spravka),200),ProducesResponseType(400),ProducesResponseType(404)] public ActionResult<Spravka> Update(int id,Spravka s){var old=Spravki.FirstOrDefault(x=>x.Id==id);if(old==null)return NotFound();var e=Validate(s);if(e!=null)return BadRequest(e);var t=SpravkaTypesController.Types.First(x=>x.Id==s.SpravkaTypeId);old.PatientId=s.PatientId;old.SpravkaTypeId=s.SpravkaTypeId;old.Name=t.Name;old.Price=t.Price;return Ok(old);}
 [HttpDelete("{id}"),ProducesResponseType(204),ProducesResponseType(404)] public ActionResult Delete(int id){var s=Spravki.FirstOrDefault(x=>x.Id==id);if(s==null)return NotFound();Spravki.Remove(s);return NoContent();}
}
