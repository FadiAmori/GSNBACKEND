using Microsoft.AspNetCore.Mvc;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategorieFinanciereController : ControllerBase
    {
        private readonly IServiceCategorieFinanciere _serviceCategorieFinanciere;

        public CategorieFinanciereController(IServiceCategorieFinanciere serviceCategorieFinanciere)
        {
            _serviceCategorieFinanciere = serviceCategorieFinanciere;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategorieFinanciere>> GetAll()
        {
            var items = _serviceCategorieFinanciere.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<CategorieFinanciere> GetById(int id)
        {
            var item = _serviceCategorieFinanciere.GetById(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<CategorieFinanciere> Create([FromBody] CategorieFinanciere item)
        {
            _serviceCategorieFinanciere.Add(item);
            _serviceCategorieFinanciere.Commit();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CategorieFinanciere item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceCategorieFinanciere.Update(item);
            _serviceCategorieFinanciere.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceCategorieFinanciere.GetById(id);
            if (item == null)
                return NotFound();

            _serviceCategorieFinanciere.Delete(item);
            _serviceCategorieFinanciere.Commit();
            return NoContent();
        }
    }
}
