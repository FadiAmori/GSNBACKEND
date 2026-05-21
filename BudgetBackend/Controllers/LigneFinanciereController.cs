using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LigneFinanciereController : ControllerBase
    {
        private readonly IServiceLigneFinanciere _serviceLigneFinanciere;

        public LigneFinanciereController(IServiceLigneFinanciere serviceLigneFinanciere)
        {
            _serviceLigneFinanciere = serviceLigneFinanciere;
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpGet]
        public ActionResult<IEnumerable<LigneFinanciere>> GetAll()
        {
            var items = _serviceLigneFinanciere.GetAll();
            return Ok(items);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpGet("{id}")]
        public ActionResult<LigneFinanciere> GetById(int id)
        {
            var item = _serviceLigneFinanciere.GetById(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPost]
        public ActionResult<LigneFinanciere> Create([FromBody] LigneFinanciere item)
        {
            _serviceLigneFinanciere.Add(item);
            _serviceLigneFinanciere.Commit();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] LigneFinanciere item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceLigneFinanciere.Update(item);
            _serviceLigneFinanciere.Commit();
            return NoContent();
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPatch("{id}/montant")]
        public IActionResult UpdateMontant(int id, [FromBody] double montant)
        {
            var item = _serviceLigneFinanciere.GetById(id);
            if (item == null)
                return NotFound();

            item.Montant = montant;
            _serviceLigneFinanciere.Update(item);
            _serviceLigneFinanciere.Commit();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceLigneFinanciere.GetById(id);
            if (item == null)
                return NotFound();

            _serviceLigneFinanciere.Delete(item);
            _serviceLigneFinanciere.Commit();
            return NoContent();
        }
    }
}
