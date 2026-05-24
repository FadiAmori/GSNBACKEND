using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using Am.ApplicationCore.DTOs;
using System.Collections.Generic;
using System.Security.Claims;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RapportFinancierController : ControllerBase
    {
        private readonly IServiceRapportFinancier _serviceRapportFinancier;
        private readonly IServiceSociete _serviceSociete;

        public RapportFinancierController(IServiceRapportFinancier serviceRapportFinancier, IServiceSociete serviceSociete)
        {
            _serviceRapportFinancier = serviceRapportFinancier;
            _serviceSociete = serviceSociete;
        }

        // GET: api/RapportFinancier (Admin only - view all)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<RapportFinancier>> GetAll()
        {
            return Ok(_serviceRapportFinancier.GetAll());
        }

        // GET: api/RapportFinancier/societe/5 (Get all reports for a Societe)
        [Authorize(Roles = "Admin,Societe")]
        [HttpGet("societe/{societeId}")]
        public ActionResult<IEnumerable<RapportFinancier>> GetBySocieteId(int societeId)
        {
            var userType = User.FindFirst("userType")?.Value;
            if (userType == "Societe")
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId) && userId != societeId)
                    return Forbid();
            }

            var rapports = _serviceRapportFinancier.GetMany(r => r.SocieteId == societeId);
            return Ok(rapports);
        }

        // GET: api/RapportFinancier/5 (Admin or own Societe)
        [Authorize(Roles = "Admin,Societe")]
        [HttpGet("{id}")]
        public ActionResult<RapportFinancier> GetById(int id)
        {
            var rapport = _serviceRapportFinancier.GetById(id);
            if (rapport == null)
                return NotFound();

            var userType = User.FindFirst("userType")?.Value;
            if (userType == "Societe")
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int societeId) && rapport.SocieteId != societeId)
                    return Forbid();
            }

            return Ok(rapport);
        }

        // POST: api/RapportFinancier
        // Used by Societe users to create a report for their own company
        [Authorize(Roles = "Societe")]
        [HttpPost]
        public ActionResult Create([FromBody] RapportFinancierCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int societeId))
                return Unauthorized();

            var rapport = new RapportFinancier
            {
                Annee = dto.Annee,
                Type = dto.Type,
                SocieteId = societeId
            };

            try
            {
                _serviceRapportFinancier.Add(rapport);
                _serviceRapportFinancier.Commit();
                return CreatedAtAction(nameof(GetById), new { id = rapport.Id }, new { id = rapport.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { error = "Erreur serveur lors de la création du rapport.", detail = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        // POST: api/RapportFinancier/societe/5  (Admin only)
        // Admin-only: create a report FOR the given societeId
        [Authorize(Roles = "Admin")]
        [HttpPost("societe/{societeId:int}")]
        public ActionResult CreateForSociete(int societeId, [FromBody] RapportFinancierCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var societe = _serviceSociete.GetById(societeId);
            if (societe == null)
                return BadRequest(new { error = "Société introuvable." });

            var rapport = new RapportFinancier
            {
                Annee = dto.Annee,
                Type = dto.Type,
                SocieteId = societeId
            };

            try
            {
                _serviceRapportFinancier.Add(rapport);
                _serviceRapportFinancier.Commit();
                return CreatedAtAction(nameof(GetById), new { id = rapport.Id }, new { id = rapport.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { error = "Erreur serveur lors de la création du rapport.", detail = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] RapportFinancier rapport)
        {
            if (id != rapport.Id)
                return BadRequest();

            var existingRapport = _serviceRapportFinancier.GetById(id);
            if (existingRapport == null)
                return NotFound();

            var userType = User.FindFirst("userType")?.Value;
            if (userType == "Societe")
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int societeId) && existingRapport.SocieteId != societeId)
                    return Forbid();
            }

            _serviceRapportFinancier.Update(rapport);
            _serviceRapportFinancier.Commit();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var rapport = _serviceRapportFinancier.GetById(id);
            if (rapport == null)
                return NotFound();

            var userType = User.FindFirst("userType")?.Value;
            if (userType == "Societe")
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int societeId) && rapport.SocieteId != societeId)
                    return Forbid();
            }

            _serviceRapportFinancier.Delete(rapport);
            _serviceRapportFinancier.Commit();
            return NoContent();
        }
    }
}
