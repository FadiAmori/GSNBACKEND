using Am.ApplicationCore.Domain;
using Am.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using BudgetBackend.Services;

namespace BudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocieteController : ControllerBase
    {
        private readonly IServiceSociete _serviceSociete;
        private readonly IJwtTokenService _jwtTokenService;

        public SocieteController(IServiceSociete serviceSociete, IJwtTokenService jwtTokenService)
        {
            _serviceSociete = serviceSociete;
            _jwtTokenService = jwtTokenService;
        }

        // GET: api/Societe (Admin only - view all)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<Societe>> GetAll()
        {
            var societes = _serviceSociete.GetAll();
            return Ok(societes);
        }

        // GET: api/Societe/{id} (Admin only or own account)
        [Authorize(Roles = "Admin,Societe")]
        [HttpGet("{id}")]
        public ActionResult<Societe> GetById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userType = User.FindFirst("userType")?.Value;

            if (userType == "Societe" && int.Parse(userIdClaim) != id)
                return Forbid();

            var societe = _serviceSociete.GetById(id);
            if (societe == null)
                return NotFound();
            
            societe.Password = null;
            return Ok(societe);
        }

        // POST: api/Societe (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<Societe> Create([FromBody] Societe societe)
        {
            _serviceSociete.Add(societe);
            _serviceSociete.Commit();
            return CreatedAtAction(nameof(GetById), new { id = societe.Id }, societe);
        }

        // POST: api/Societe/login
        [HttpPost("login")]
        public ActionResult<TokenResponse> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Email et password requis" });

            var societe = _serviceSociete.Get(s => s.Email == request.Email && s.Password == request.Password);
            if (societe == null)
                return Unauthorized(new { message = "Identifiants invalides" });

            var token = _jwtTokenService.GenerateToken(societe.Id, societe.Email, "Societe", societe.Email);
            var expiresIn = DateTime.UtcNow.AddHours(8);

            return Ok(new TokenResponse
            {
                Token = token,
                ExpiresIn = expiresIn,
                Id = societe.Id,
                UserType = "Societe"
            });
        }

        // POST: api/Societe/logout/{id}
        [Authorize(Roles = "Societe")]
        [HttpPost("logout/{id}")]
        public IActionResult Logout(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.Parse(userIdClaim) != id)
                return Forbid();

            var societe = _serviceSociete.GetById(id);
            if (societe == null)
                return NotFound();

            societe.Active = false;
            _serviceSociete.Update(societe);
            _serviceSociete.Commit();
            return NoContent();
        }

        // PUT: api/Societe/{id} (Admin only or own account)
        [Authorize(Roles = "Admin,Societe")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Societe societe)
        {
            if (id != societe.Id)
                return BadRequest();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userType = User.FindFirst("userType")?.Value;

            if (userType == "Societe" && int.Parse(userIdClaim) != id)
                return Forbid();

            _serviceSociete.Update(societe);
            _serviceSociete.Commit();
            return NoContent();
        }

        // DELETE: api/Societe/{id} (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var societe = _serviceSociete.GetById(id);
            if (societe == null)
                return NotFound();

            _serviceSociete.Delete(societe);
            _serviceSociete.Commit();
            return NoContent();
        }

        private async Task<bool> SendEmailAsync(string to, string subject, string plainBody, string htmlBody)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("fadiamorri2002@gmail.com"));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;

                var builder = new BodyBuilder { TextBody = plainBody, HtmlBody = htmlBody };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("fadiamorri2002@gmail.com", "ayjj urxn zauy bopb");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { message = "Email requis" });

            var societe = _serviceSociete.Get(s => s.Email == request.Email);

            if (societe == null)
                return Ok(new { message = "Si cet email existe, vous recevrez un message" });

            var resetLink = $"http://localhost:4200/reset-password?email={request.Email}";
            var plainBody = $"Cliquez ici pour changer votre mot de passe : {resetLink}";
            var htmlBody = $"Cliquez ici pour changer votre mot de passe : <a href=\"{resetLink}\">resetpassword</a>";

            await SendEmailAsync(request.Email, "Reset Password", plainBody, htmlBody);

            return Ok(new { message = "Email envoyé si le compte existe" });
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.NewPassword))
                return BadRequest(new { message = "Email et NewPassword sont requis" });

            var societe = _serviceSociete.Get(s => s.Email == request.Email);

            if (societe == null)
                return BadRequest(new { message = "Email invalide" });

            societe.Password = request.NewPassword;

            _serviceSociete.Update(societe);
            _serviceSociete.Commit();

            return Ok(new { message = "Mot de passe réinitialisé" });
        }
    }
}
