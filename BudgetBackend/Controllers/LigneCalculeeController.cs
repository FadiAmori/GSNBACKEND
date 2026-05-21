using Microsoft.AspNetCore.Mvc;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Globalization;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LigneCalculeeController : ControllerBase
    {
        private readonly IServiceLigneCalculee _service;
        private readonly IServiceLigneFinanciere _ligneService;

        public LigneCalculeeController(IServiceLigneCalculee service, IServiceLigneFinanciere ligneService)
        {
            _service = service;
            _ligneService = ligneService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<LigneCalculee>> GetAll()
        {
            var items = _service.GetAll()?.OrderBy(x => x.Position).ToList() ?? new List<LigneCalculee>();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<LigneCalculee> GetById(int id)
        {
            var item = _service.GetById(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<LigneCalculee> Create([FromBody] LigneCalculee item)
        {
            if (item == null)
                return BadRequest();

            // if position not set, append to end
            var all = _service.GetAll();
            if (all != null && !all.Any())
            {
                item.Position = 0;
            }
            else if (all != null)
            {
                item.Position = all.Max(x => x.Position) + 1;
            }

            _service.Add(item);
            try
            {
                _service.Commit();
            }
            catch
            {
                return StatusCode(500, "An error occurred while saving the item.");
            }

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] LigneCalculee item)
        {
            if (item == null || id != item.Id)
                return BadRequest();

            var existing = _service.GetById(id);
            if (existing == null)
                return NotFound();

            // FIX: also update Resultat and Couleur so frontend-calculated values
            // and display color are persisted to the database.
            existing.Nom = item.Nom;
            existing.Expression = item.Expression;
            existing.Couleur = item.Couleur;
            // Only overwrite Position if the incoming value is explicitly set (> 0),
            // otherwise keep the existing position to prevent it from being reset to 0
            // when the DTO doesn't include position (e.g. during Excel import).
            if (item.Position > 0)
                existing.Position = item.Position;
            existing.Resultat = item.Resultat;

            _service.Update(existing);
            try
            {
                _service.Commit();
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the item.");
            }

            return NoContent();
        }

        [HttpPost("order")]
        public IActionResult UpdateOrder([FromBody] List<OrderDto> list)
        {
            if (list == null || !list.Any())
                return BadRequest();

            foreach (var it in list)
            {
                var entity = _service.GetById(it.Id);
                if (entity == null)
                    continue;

                entity.Position = it.Position;
                _service.Update(entity);
            }

            try
            {
                _service.Commit();
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating order.");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _service.GetById(id);
            if (item == null)
                return NotFound();

            _service.Delete(item);
            try
            {
                _service.Commit();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the item.");
            }

            return NoContent();
        }

        public class OrderDto
        {
            public int Id { get; set; }
            public int Position { get; set; }
        }

        public class EvalDto
        {
            public string? Expression { get; set; }
            public int? SocieteId { get; set; }
            public int? Mois { get; set; }
            public int? Annee { get; set; }
        }

        [HttpPost("evaluer")]
        public ActionResult<object> Evaluer([FromBody] EvalDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Expression))
                return BadRequest("Expression is required.");

            var (ok, value, error) = EvaluateExpression(dto.Expression!, dto.SocieteId, dto.Mois, dto.Annee);
            if (!ok)
                return BadRequest(error);

            return Ok(new { result = value });
        }

        [HttpPost("{id}/evaluer")]
        public IActionResult EvaluerEtSauvegarder(int id, [FromBody] EvalDto? dto)
        {
            var item = _service.GetById(id);
            if (item == null)
                return NotFound();

            var expression = item.Expression;
            if (dto != null && !string.IsNullOrWhiteSpace(dto.Expression))
                expression = dto.Expression;

            if (string.IsNullOrWhiteSpace(expression))
                return BadRequest("No expression to evaluate.");

            var (ok, value, error) = EvaluateExpression(expression!, dto?.SocieteId, dto?.Mois, dto?.Annee);
            if (!ok)
                return BadRequest(error);

            item.Resultat = value;
            _service.Update(item);
            try
            {
                _service.Commit();
            }
            catch
            {
                return StatusCode(500, "Error saving result.");
            }

            return Ok(new { result = value });
        }

        private (bool ok, double value, string? error) EvaluateExpression(string expression, int? societeId, int? mois, int? annee)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return (false, 0, "Expression is empty.");

            string expr = expression;

            // load all lignes and optionally filter by mois/annee
            var lignes = _ligneService.GetAll()?.ToList() ?? new List<LigneFinanciere>();
            if (mois.HasValue)
                lignes = lignes.Where(l => l.Mois == mois.Value).ToList();
            if (annee.HasValue)
                lignes = lignes.Where(l => l.Annee == annee.Value).ToList();

            var matches = Regex.Matches(expr, "\\[([^\\]]+)\\]");
            foreach (Match m in matches)
            {
                var token = m.Groups[1].Value.Trim();
                double value = 0;

                var found = lignes.FirstOrDefault(l => string.Equals((l.Nom ?? string.Empty).Trim(), token, System.StringComparison.OrdinalIgnoreCase));
                if (found == null)
                    found = lignes.FirstOrDefault(l => (l.Nom ?? string.Empty).IndexOf(token, System.StringComparison.OrdinalIgnoreCase) >= 0);

                if (found != null)
                    value = found.Montant;

                expr = expr.Replace("[" + m.Groups[1].Value + "]", value.ToString(CultureInfo.InvariantCulture));
            }

            if (Regex.IsMatch(expr, "[^0-9\\.\\+\\-\\*\\/\\(\\)\\s]"))
                return (false, 0, "Expression contains invalid characters after token replacement.");

            try
            {
                var resultObj = new DataTable().Compute(expr, null);
                double result = Convert.ToDouble(resultObj, CultureInfo.InvariantCulture);
                return (true, result, null);
            }
            catch (System.Exception ex)
            {
                return (false, 0, "Error evaluating expression: " + ex.Message);
            }
        }
    }
}