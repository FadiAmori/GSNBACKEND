using Microsoft.AspNetCore.Mvc;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SousCategorieCRController : ControllerBase
    {
        private readonly IServiceSousCategorieCR _serviceSousCategorieCR;

        public SousCategorieCRController(IServiceSousCategorieCR serviceSousCategorieCR)
        {
            _serviceSousCategorieCR = serviceSousCategorieCR;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SousCategorieCR>> GetAll()
        {
            var items = _serviceSousCategorieCR.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<SousCategorieCR> GetById(int id)
        {
            var item = _serviceSousCategorieCR.GetById(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<SousCategorieCR> Create([FromBody] SousCategorieCR item)
        {
            _serviceSousCategorieCR.Add(item);
            _serviceSousCategorieCR.Commit();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SousCategorieCR item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceSousCategorieCR.Update(item);
            _serviceSousCategorieCR.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceSousCategorieCR.GetById(id);
            if (item == null)
                return NotFound();

            _serviceSousCategorieCR.Delete(item);
            _serviceSousCategorieCR.Commit();
            return NoContent();
        }
    }
}
