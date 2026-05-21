using Microsoft.AspNetCore.Mvc;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SousCategorieFinanciereController : ControllerBase
    {
        private readonly IServiceSousCategorieFinanciere _serviceSousCategorieFinanciere;

        public SousCategorieFinanciereController(IServiceSousCategorieFinanciere serviceSousCategorieFinanciere)
        {
            _serviceSousCategorieFinanciere = serviceSousCategorieFinanciere;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SousCategorieFinanciere>> GetAll()
        {
            var items = _serviceSousCategorieFinanciere.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<SousCategorieFinanciere> GetById(int id)
        {
            var item = _serviceSousCategorieFinanciere.GetById(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<SousCategorieFinanciere> Create([FromBody] SousCategorieFinanciere item)
        {
            _serviceSousCategorieFinanciere.Add(item);
            _serviceSousCategorieFinanciere.Commit();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SousCategorieFinanciere item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceSousCategorieFinanciere.Update(item);
            _serviceSousCategorieFinanciere.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceSousCategorieFinanciere.GetById(id);
            if (item == null)
                return NotFound();

            _serviceSousCategorieFinanciere.Delete(item);
            _serviceSousCategorieFinanciere.Commit();
            return NoContent();
        }
    }
}
