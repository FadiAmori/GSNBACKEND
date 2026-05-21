using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeClientController : ControllerBase
    {
        private readonly IServiceTypeClient _serviceTypeClient;

        public TypeClientController(IServiceTypeClient serviceTypeClient)
        {
            _serviceTypeClient = serviceTypeClient;
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpGet]
        public ActionResult<IEnumerable<TypeClient>> GetAll()
        {
            var items = _serviceTypeClient.GetAll();
            return Ok(items);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpGet("{id}")]
        public ActionResult<TypeClient> GetById(int id)
        {
            var item = _serviceTypeClient.GetById(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPost]
        public ActionResult<TypeClient> Create([FromBody] TypeClient item)
        {
            _serviceTypeClient.Add(item);
            _serviceTypeClient.Commit();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TypeClient item)
        {
            if (id != item.Id)
                return BadRequest();

            _serviceTypeClient.Update(item);
            _serviceTypeClient.Commit();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _serviceTypeClient.GetById(id);
            if (item == null)
                return NotFound();

            _serviceTypeClient.Delete(item);
            _serviceTypeClient.Commit();
            return NoContent();
        }
    }
}
