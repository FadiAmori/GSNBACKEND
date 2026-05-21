using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSocieteController : ControllerBase
    {
        private readonly IServiceUserSociete _serviceUserSociete;

        public UserSocieteController(IServiceUserSociete serviceUserSociete)
        {
            _serviceUserSociete = serviceUserSociete;
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpGet]
        public ActionResult<IEnumerable<UserSociete>> GetAll()
        {
            var userType = User.FindFirst("userType")?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userType == "Societe")
            {
                var users = _serviceUserSociete.GetAll();
                var filtered = new List<UserSociete>();
                foreach (var user in users)
                {
                    if (user.SocieteId == int.Parse(userId))
                        filtered.Add(user);
                }
                return Ok(filtered);
            }

            return Ok(_serviceUserSociete.GetAll());
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpGet("{id}")]
        public ActionResult<UserSociete> GetById(int id)
        {
            var user = _serviceUserSociete.GetById(id);
            if (user == null)
                return NotFound();

            var userType = User.FindFirst("userType")?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userType == "Societe" && user.SocieteId != int.Parse(userId))
                return Forbid();

            return Ok(user);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPost]
        public ActionResult<UserSociete> Create([FromBody] UserSociete user)
        {
            var userType = User.FindFirst("userType")?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userType == "Societe" && user.SocieteId != int.Parse(userId))
                return Forbid();

            _serviceUserSociete.Add(user);
            _serviceUserSociete.Commit();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserSociete user)
        {
            if (id != user.Id)
                return BadRequest();

            var userType = User.FindFirst("userType")?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingUser = _serviceUserSociete.GetById(id);

            if (existingUser == null)
                return NotFound();

            if (userType == "Societe" && existingUser.SocieteId != int.Parse(userId))
                return Forbid();

            _serviceUserSociete.Update(user);
            _serviceUserSociete.Commit();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _serviceUserSociete.GetById(id);
            if (user == null)
                return NotFound();

            _serviceUserSociete.Delete(user);
            _serviceUserSociete.Commit();
            return NoContent();
        }
    }
}
