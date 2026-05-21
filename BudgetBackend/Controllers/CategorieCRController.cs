using Microsoft.AspNetCore.Mvc;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategorieCRController : ControllerBase
    {
        private readonly IServiceCategorieCR _serviceCategorieCR;

        public CategorieCRController(IServiceCategorieCR serviceCategorieCR)
        {
            _serviceCategorieCR = serviceCategorieCR;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategorieCR>> GetAll()
        {
            var items = _serviceCategorieCR.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<CategorieCR> GetById(int id)
        {
            var item = _serviceCategorieCR.GetById(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<CategorieCR> Create([FromBody] CategorieCR item)
        {
            _serviceCategorieCR.Add(item);
            _serviceCategorieCR.Commit();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CategorieCR item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceCategorieCR.Update(item);
            _serviceCategorieCR.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceCategorieCR.GetById(id);
            if (item == null)
                return NotFound();

            _serviceCategorieCR.Delete(item);
            _serviceCategorieCR.Commit();
            return NoContent();
        }
    }
}
