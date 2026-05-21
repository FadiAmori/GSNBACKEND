using Microsoft.AspNetCore.Mvc;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CRController : ControllerBase
    {
        private readonly IServiceCR _serviceCR;

        public CRController(IServiceCR serviceCR)
        {
            _serviceCR = serviceCR;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CR>> GetAll()
        {
            var items = _serviceCR.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<CR> GetById(int id)
        {
            var item = _serviceCR.GetById(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<CR> Create([FromBody] CR item)
        {
            _serviceCR.Add(item);
            _serviceCR.Commit();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CR item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceCR.Update(item);
            _serviceCR.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceCR.GetById(id);
            if (item == null)
                return NotFound();

            _serviceCR.Delete(item);
            _serviceCR.Commit();
            return NoContent();
        }
    }
}
