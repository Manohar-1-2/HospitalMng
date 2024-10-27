
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using System.Security.Claims;


public static class LoginEndpoints
{
    public static void MapLoginRoutes(this WebApplication app)
    {
        app.MapPost("/login", async (ApplicationDbContext dbContext, UserDtoLogin userDto) =>
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(u => u.UserUID == userDto.UserUID);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return Results.Unauthorized();
            }

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("yourSecretKeyyourSecretKeyyourSecretKey");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserUID),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Results.Ok(new { Token = tokenString, Role = user.Role });
        });
    }
}
