using Microsoft.AspNetCore.Mvc;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClesDeRepartitionController : ControllerBase
    {
        private readonly IServiceClesDeRepartition _serviceClesDeRepartition;

        public ClesDeRepartitionController(IServiceClesDeRepartition serviceClesDeRepartition)
        {
            _serviceClesDeRepartition = serviceClesDeRepartition;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClesDeRepartition>> GetAll()
        {
            var items = _serviceClesDeRepartition.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<ClesDeRepartition> GetById(int id)
        {
            var item = _serviceClesDeRepartition.GetById(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<ClesDeRepartition> Create([FromBody] ClesDeRepartition item)
        {
            _serviceClesDeRepartition.Add(item);
            _serviceClesDeRepartition.Commit();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ClesDeRepartition item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceClesDeRepartition.Update(item);
            _serviceClesDeRepartition.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceClesDeRepartition.GetById(id);
            if (item == null)
                return NotFound();

            _serviceClesDeRepartition.Delete(item);
            _serviceClesDeRepartition.Commit();
            return NoContent();
        }
    }
}
