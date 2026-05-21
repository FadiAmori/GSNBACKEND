namespace BudgetBackend.Controllers
{
    public class ForgotPasswordRequest
    {
        public string? Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
    }

    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class TokenResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public int Id { get; set; }
        public string? UserType { get; set; }
    }
}
