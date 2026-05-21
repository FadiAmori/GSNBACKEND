using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProduitController : ControllerBase
    {
        private readonly IServiceProduit _serviceProduit;

        public ProduitController(IServiceProduit serviceProduit)
        {
            _serviceProduit = serviceProduit;
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpGet]
        public ActionResult<IEnumerable<Produit>> GetAll()
        {
            return Ok(_serviceProduit.GetAll());
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpGet("{id}")]
        public ActionResult<Produit> GetById(int id)
        {
            var produit = _serviceProduit.GetById(id);
            if (produit == null)
                return NotFound();
            return Ok(produit);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPost]
        public ActionResult<Produit> Create([FromBody] Produit produit)
        {
            _serviceProduit.Add(produit);
            _serviceProduit.Commit();
            return CreatedAtAction(nameof(GetById), new { id = produit.Id }, produit);
        }

        [Authorize(Roles = "Admin,Societe")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Produit produit)
        {
            if (id != produit.Id)
                return BadRequest();

            _serviceProduit.Update(produit);
            _serviceProduit.Commit();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var produit = _serviceProduit.GetById(id);
            if (produit == null)
                return NotFound();

            _serviceProduit.Delete(produit);
            _serviceProduit.Commit();
            return NoContent();
        }
    }
}
