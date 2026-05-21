using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamilleProduitController : ControllerBase
    {
        private readonly IServiceFamilleProduit _serviceFamilleProduit;

        public FamilleProduitController(IServiceFamilleProduit serviceFamilleProduit)
        {
            _serviceFamilleProduit = serviceFamilleProduit;
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpGet]
        public ActionResult<IEnumerable<FamilleProduit>> GetAll()
        {
            var items = _serviceFamilleProduit.GetAll();
            return Ok(items);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpGet("{id}")]
        public ActionResult<FamilleProduit> GetById(int id)
        {
            var item = _serviceFamilleProduit.GetById(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPost]
        public ActionResult<FamilleProduit> Create([FromBody] FamilleProduit item)
        {
            _serviceFamilleProduit.Add(item);
            _serviceFamilleProduit.Commit();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] FamilleProduit item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceFamilleProduit.Update(item);
            _serviceFamilleProduit.Commit();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceFamilleProduit.GetById(id);
            if (item == null)
                return NotFound();

            _serviceFamilleProduit.Delete(item);
            _serviceFamilleProduit.Commit();
            return NoContent();
        }
    }
}
