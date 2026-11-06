using Microsoft.AspNetCore.Mvc; using MedicalApi.Models;
namespace MedicalApi.Controllers;
[ApiController,Route("api/[controller]")] public class SpravkaTypesController:ControllerBase {
 public static List<SpravkaType> Types=new(){new(){Id=1,Name="Справка 083",Price=100,ValidDays=30,RequiresDoctorExam=true,AgeGroup="Взрослые"},new(){Id=2,Name="Санитарная книжка",Price=750,ValidDays=365,RequiresDoctorExam=true,AgeGroup="Взрослые"},new(){Id=3,Name="Справка в бассейн",Price=250,ValidDays=90,RequiresDoctorExam=true},new(){Id=4,Name="Справка в спортивную секцию",Price=400,ValidDays=180,RequiresDoctorExam=true}};
 [HttpGet,ProducesResponseType(typeof(List<SpravkaType>),200)] public ActionResult<List<SpravkaType>> GetAll()=>Ok(Types);
 [HttpGet("{id}"),ProducesResponseType(typeof(SpravkaType),200),ProducesResponseType(404)] public ActionResult<SpravkaType> GetById(int id)=>Types.FirstOrDefault(x=>x.Id==id) is {} t?Ok(t):NotFound();
 string? Validate(SpravkaType t)=>string.IsNullOrWhiteSpace(t.Name)?"Название обязательно":t.Price<=0?"Цена должна быть больше 0":t.ValidDays<=0?"Срок действия должен быть больше 0":null;
 [HttpPost,ProducesResponseType(typeof(SpravkaType),201),ProducesResponseType(400)] public ActionResult<SpravkaType> Create(SpravkaType t){var e=Validate(t);if(e!=null)return BadRequest(e);t.Id=Types.Count==0?1:Types.Max(x=>x.Id)+1;Types.Add(t);return CreatedAtAction(nameof(GetById),new{id=t.Id},t);}
 [HttpPut("{id}"),ProducesResponseType(typeof(SpravkaType),200),ProducesResponseType(400),ProducesResponseType(404)] public ActionResult<SpravkaType> Update(int id,SpravkaType t){var old=Types.FirstOrDefault(x=>x.Id==id);if(old==null)return NotFound();var e=Validate(t);if(e!=null)return BadRequest(e);t.Id=id;Types[Types.IndexOf(old)]=t;return Ok(t);}
 [HttpDelete("{id}"),ProducesResponseType(204),ProducesResponseType(404)] public ActionResult Delete(int id){var t=Types.FirstOrDefault(x=>x.Id==id);if(t==null)return NotFound();Types.Remove(t);return NoContent();}
}
