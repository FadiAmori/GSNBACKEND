# JWT Token Structure & Claims Reference

## Token Anatomy

A JWT token has 3 parts separated by dots:

```
header.payload.signature
```

Example token (shortened):
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIn0.dozjgNryP4J3jVmNHl0w5N_XgL0n3I9PlFUP0THsR8U
```

---

## ?? Token Payload

### Admin Token Payload
```json
{
  "nameid": "1",                 // Admin ID
  "email": "admin@example.com",  // Admin email
  "userType": "Admin",           // User type claim
  "role": "Admin",               // Role for authorization
  "aud": "BudgetApp",            // Audience
  "iss": "BudgetBackend",        // Issuer
  "exp": 1672531200,             // Expiration timestamp
  "iat": 1672502400              // Issued at timestamp
}
```

### Societe Token Payload
```json
{
  "nameid": "5",                    // Societe ID
  "email": "company@example.com",   // Societe email
  "userType": "Societe",            // User type claim
  "role": "Societe",                // Role for authorization
  "aud": "BudgetApp",               // Audience
  "iss": "BudgetBackend",           // Issuer
  "exp": 1672531200,                // Expiration timestamp
  "iat": 1672502400                 // Issued at timestamp
}
```

---

## ?? Claims Used for Authorization

### Token Claims Structure
```csharp
claims.Add(new Claim(ClaimTypes.NameIdentifier, id));           // User ID
claims.Add(new Claim(ClaimTypes.Email, email));                 // Email
claims.Add(new Claim("userType", userType));                    // Admin or Societe
claims.Add(new Claim(ClaimTypes.Role, role));                   // Role for [Authorize]
```

### Claims Retrieved in Controllers
```csharp
// Get user ID
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
// Result: "1" or "5"

// Get user type
var userType = User.FindFirst("userType")?.Value;
// Result: "Admin" or "Societe"

// Get email
var email = User.FindFirst(ClaimTypes.Email)?.Value;
// Result: "admin@example.com"
```

---

## ??? Authorization Attributes

### By Role
```csharp
[Authorize(Roles = "Admin")]
// Only admins can access

[Authorize(Roles = "Admin,Societe")]
// Both roles can access

[Authorize(Roles = "Societe")]
// Only societe users can access
```

### Combined with Logic
```csharp
[Authorize(Roles = "Admin,Societe")]
[HttpPut("{id}")]
public IActionResult Update(int id, [FromBody] Societe data)
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    var userType = User.FindFirst("userType")?.Value;

    // Societe can only modify their own data
    if (userType == "Societe" && userId != id)
        return Forbid();

    // Rest of logic...
}
```

---

## ?? Token Validation Flow

### 1?? Request Arrives
```
GET /api/lignefinanciere
Authorization: Bearer eyJhbGciOiJIUzI1Ni...
```

### 2?? Token Extracted
```csharp
// JwtBearer middleware extracts token from header
var token = "eyJhbGciOiJIUzI1Ni...";
```

### 3?? Signature Verified
```csharp
// Check signature using SecretKey
var secretKey = "your-super-secret-key-...";
// If signature invalid ? 401 Unauthorized
```

### 4?? Claims Validated
```csharp
// Verify Issuer
ValidIssuer = "BudgetBackend" ?

// Verify Audience
ValidAudience = "BudgetApp" ?

// Check Expiration
exp > now ?
```

### 5?? Claims Extracted
```csharp
ClaimsPrincipal user = new ClaimsPrincipal(identity);
// Now available in User property of controller
```

### 6?? Authorization Checked
```csharp
// Check [Authorize(Roles = "Admin")]
// User.IsInRole("Admin") ? OR ?
```

### 7?? Access Granted or Denied
```
? 200 OK - Access granted, proceed with controller logic
? 403 Forbidden - Role not sufficient
? 401 Unauthorized - Invalid/expired token
```

---

## ?? Token Lifecycle

```
Time: 0:00
?? User logs in
?? Credentials validated
?? JWT token generated (exp: +8 hours)
?
Time: 0:01 - 7:59
?? Token valid
?? Each request validates token
?? Claims available in controller
?
Time: 8:00
?? Token EXPIRED
?? New requests return 401
?? Client must login again to get new token
?
Time: 8:01+
?? User logs in again
?? New token generated
?? Cycle repeats
```

---

## ?? Integration Points

### In Angular HTTP Interceptor
```typescript
// Extract token from localStorage
const token = localStorage.getItem('token');

// Add to every request
req.setHeaders({
  'Authorization': `Bearer ${token}`
});
```

### In Backend Middleware
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "BudgetBackend",
            ValidAudience = "BudgetApp",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
```

---

## ?? Common Claims Patterns

### Check if Admin
```csharp
bool isAdmin = User.IsInRole("Admin");
// Or
string userType = User.FindFirst("userType")?.Value;
bool isAdmin = userType == "Admin";
```

### Check if Own Resource
```csharp
int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
int resourceOwnerId = GetResourceOwnerId(id);

if (userId != resourceOwnerId && !User.IsInRole("Admin"))
    return Forbid();
```

### Check Company Access
```csharp
int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
string userType = User.FindFirst("userType")?.Value;

// Get user's company
var company = GetSocieteById(userId); // For Societe users

// Check if accessing own company
if (userType == "Societe" && resourceCompanyId != company.Id)
    return Forbid();
```

---

## ?? Security Best Practices

### ? DO
```csharp
// ? Store in localStorage (for SPAs)
localStorage.setItem('token', response.token);

// ? Include in every request
Authorization: Bearer ${token}

// ? Check expiration
if (new Date(expiresIn) < new Date())
    logout();

// ? Validate server-side
[Authorize]
public IActionResult ProtectedEndpoint() { }

// ? Use HTTPS in production
```

### ? DON'T
```javascript
// ? Don't store in cookies (XSS vulnerability)
document.cookie = `token=${response.token}`;

// ? Don't include sensitive data in token
// (tokens can be decoded client-side)
claims.Add(new Claim("password", password));

// ? Don't trust only client-side roles
// Always validate server-side

// ? Don't use weak secret key
secretKey = "password";  // ? BAD

// ? Don't use HTTP in production
http://api.com  // ? INSECURE
```

---

## ?? Token Refresh Strategy (Optional)

```csharp
// If implementing refresh tokens:

public class RefreshTokenResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresIn { get; set; }
}

// Frontend:
// 1. On 401 response ? Send refreshToken to /api/auth/refresh
// 2. Get new token
// 3. Retry original request
// 4. If refresh fails ? Logout
```

---

## ?? Testing Token Claims

### Decode Token in Postman
```
1. Copy token from login response
2. Go to https://jwt.io
3. Paste token in "Encoded" section
4. View decoded payload
5. Verify claims are correct
```

### Test Authorization
```bash
# ? Should work (Admin)
curl -H "Authorization: Bearer ${ADMIN_TOKEN}" \
  http://localhost:5000/api/admin

# ? Should fail (Societe accessing Admin endpoint)
curl -H "Authorization: Bearer ${SOCIETE_TOKEN}" \
  http://localhost:5000/api/admin
# Response: 403 Forbidden
```

---

## ?? Token Anatomy Breakdown

### Header (Base64 Decoded)
```json
{
  "alg": "HS256",    // Algorithm
  "typ": "JWT"       // Type
}
```

### Payload (Base64 Decoded)
```json
{
  "nameid": "1",
  "email": "admin@example.com",
  "userType": "Admin",
  "role": "Admin",
  "aud": "BudgetApp",
  "iss": "BudgetBackend",
  "exp": 1672531200,
  "iat": 1672502400
}
```

### Signature
```
HMACSHA256(
  base64UrlEncode(header) + "." +
  base64UrlEncode(payload),
  secretKey
)
```

---

## ?? Reference Guide

| Property | Value | Purpose |
|----------|-------|---------|
| `alg` | HS256 | Encryption algorithm |
| `typ` | JWT | Token type |
| `nameid` | User ID | Identify user |
| `email` | Email | User email |
| `userType` | Admin/Societe | User classification |
| `role` | Admin/Societe | Authorization role |
| `aud` | BudgetApp | Token audience |
| `iss` | BudgetBackend | Token issuer |
| `exp` | Timestamp | Expiration time |
| `iat` | Timestamp | Issued at time |

---

All claims are available in `User.Claims` collection within controller actions! ??
