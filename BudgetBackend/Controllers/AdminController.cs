using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using BudgetBackend.Services;
using System.Collections.Generic;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IServiceAdmin _serviceAdmin;
        private readonly IJwtTokenService _jwtTokenService;

        public AdminController(IServiceAdmin serviceAdmin, IJwtTokenService jwtTokenService)
        {
            _serviceAdmin = serviceAdmin;
            _jwtTokenService = jwtTokenService;
        }

        // GET: api/Admin (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<Admin>> GetAll()
        {
            var admins = _serviceAdmin.GetAll();
            return Ok(admins);
        }

        // GET: api/Admin/5 (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public ActionResult<Admin> GetById(int id)
        {
            var admin = _serviceAdmin.GetById(id);
            if (admin == null)
                return NotFound();
            return Ok(admin);
        }

        // POST: api/Admin (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<Admin> Create([FromBody] Admin admin)
        {
            _serviceAdmin.Add(admin);
            _serviceAdmin.Commit();
            return CreatedAtAction(nameof(GetById), new { id = admin.Id }, admin);
        }

        // POST: api/Admin/login
        [HttpPost("login")]
        public ActionResult<TokenResponse> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Email et password requis" });

            var admin = _serviceAdmin.Get(a => a.Email == request.Email && a.Password == request.Password);
            if (admin == null)
                return Unauthorized(new { message = "Identifiants invalides" });

            var token = _jwtTokenService.GenerateToken(admin.Id, admin.Email, "Admin", admin.Email);
            var expiresIn = DateTime.UtcNow.AddHours(8);

            return Ok(new TokenResponse
            {
                Token = token,
                ExpiresIn = expiresIn,
                Id = admin.Id,
                UserType = "Admin"
            });
        }

        // PUT: api/Admin/5 (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Admin admin)
        {
            if (id != admin.Id)
                return BadRequest();

            _serviceAdmin.Update(admin);
            _serviceAdmin.Commit();
            return NoContent();
        }

        // DELETE: api/Admin/5 (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var admin = _serviceAdmin.GetById(id);
            if (admin == null)
                return NotFound();

            _serviceAdmin.Delete(admin);
            _serviceAdmin.Commit();
            return NoContent();
        }
    }
}
