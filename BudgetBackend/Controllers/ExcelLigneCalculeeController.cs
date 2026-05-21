using Microsoft.AspNetCore.Mvc;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelLigneCalculeeController : ControllerBase
    {
        private readonly IServiceExcelLigneCalculee _serviceExcelLigneCalculee;
        private readonly IServiceLigneCalculee _serviceLigneCalculee;
        private readonly IServiceSociete _serviceSociete;

        public ExcelLigneCalculeeController(
            IServiceExcelLigneCalculee serviceExcelLigneCalculee,
            IServiceLigneCalculee serviceLigneCalculee,
            IServiceSociete serviceSociete)
        {
            _serviceExcelLigneCalculee = serviceExcelLigneCalculee;
            _serviceLigneCalculee = serviceLigneCalculee;
            _serviceSociete = serviceSociete;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ExcelLigneCalculee>> GetAll()
        {
            var items = _serviceExcelLigneCalculee.GetAll();
            foreach (var it in items)
            {
                if (it.LigneCalculee != null)
                    it.LigneCalculee.ExcelLigneCalculee = null;
            }
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<ExcelLigneCalculee> GetById(int id)
        {
            var item = _serviceExcelLigneCalculee.GetById(id);
            if (item == null)
                return NotFound();

            if (item.LigneCalculee != null)
                item.LigneCalculee.ExcelLigneCalculee = null;

            return Ok(item);
        }

        [HttpPost]
        public ActionResult<ExcelLigneCalculee> Create([FromBody] ExcelLigneCalculee item)
        {
            if (item == null)
                return BadRequest();

            var ligne = _serviceLigneCalculee.GetById(item.LigneCalculeeId);
            if (ligne == null)
                return BadRequest($"LigneCalculee with id {item.LigneCalculeeId} does not exist.");

            var societe = _serviceSociete.GetById(item.SocieteId);
            if (societe == null)
                return BadRequest($"Societe with id {item.SocieteId} does not exist.");

            _serviceExcelLigneCalculee.Add(item);
            _serviceExcelLigneCalculee.Commit();

            if (item.LigneCalculee != null)
                item.LigneCalculee.ExcelLigneCalculee = null;

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ExcelLigneCalculee item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceExcelLigneCalculee.Update(item);
            _serviceExcelLigneCalculee.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceExcelLigneCalculee.GetById(id);
            if (item == null)
                return NotFound();

            _serviceExcelLigneCalculee.Delete(item);
            _serviceExcelLigneCalculee.Commit();
            return NoContent();
        }
    }
}
