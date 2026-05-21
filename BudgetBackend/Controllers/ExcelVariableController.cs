using Microsoft.AspNetCore.Mvc;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelVariableController : ControllerBase
    {
        private readonly IServiceExcelVariable _serviceExcelVariable;
        private readonly IServiceLigneFinanciere _serviceLigneFinanciere;

        public ExcelVariableController(IServiceExcelVariable serviceExcelVariable, IServiceLigneFinanciere serviceLigneFinanciere)
        {
            _serviceExcelVariable = serviceExcelVariable;
            _serviceLigneFinanciere = serviceLigneFinanciere;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ExcelVariable>> GetAll()
        {
            var items = _serviceExcelVariable.GetAll();
            // avoid JSON cycles by removing navigation backrefs
            foreach (var it in items)
            {
                if (it.LigneFinanciere != null)
                    it.LigneFinanciere.ExcelVariable = null;
            }
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<ExcelVariable> GetById(int id)
        {
            var item = _serviceExcelVariable.GetById(id);
            if (item == null)
                return NotFound();
            if (item.LigneFinanciere != null)
                item.LigneFinanciere.ExcelVariable = null;
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<ExcelVariable> Create([FromBody] ExcelVariable item)
        {
            if (item == null)
                return BadRequest();

            var ligne = _serviceLigneFinanciere.GetById(item.LigneFinanciereId);
            if (ligne == null)
                return BadRequest($"LigneFinanciere with id {item.LigneFinanciereId} does not exist.");

            _serviceExcelVariable.Add(item);
            _serviceExcelVariable.Commit();
            // avoid JSON object cycle when returning the created entity
            if (item.LigneFinanciere != null)
                item.LigneFinanciere.ExcelVariable = null;
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ExcelVariable item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceExcelVariable.Update(item);
            _serviceExcelVariable.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceExcelVariable.GetById(id);
            if (item == null)
                return NotFound();

            _serviceExcelVariable.Delete(item);
            _serviceExcelVariable.Commit();
            return NoContent();
        }
    }
}
