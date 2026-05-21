using Microsoft.AspNetCore.Mvc;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CBDashController : ControllerBase
    {
        private readonly IServiceCBDash _service;

        public CBDashController(IServiceCBDash service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CBDash>> GetAll()
        {
            var items = _service.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<CBDash> GetById(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<CBDash> Create([FromBody] CBDash item)
        {
            _service.Add(item);
            _service.Commit();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CBDash item)
        {
            if (id != item.Id) return BadRequest();
            _service.Update(item);
            _service.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _service.GetById(id);
            if (item == null) return NotFound();
            _service.Delete(item);
            _service.Commit();
            return NoContent();
        }
    }
}
