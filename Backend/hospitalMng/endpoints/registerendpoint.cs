using System.Numerics;
public static class RegisterEndpoints
{
    public static void MapRegisterRoutes(this WebApplication app)
    {
        app.MapPost("/register", async (ApplicationDbContext dbContext, RegistrationDto userDto) =>
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            var guid = Guid.NewGuid();
            var numericGuid = new BigInteger(guid.ToByteArray());
            var numericGuidString = numericGuid.ToString().Substring(1, 4);
            var userUID = $"{userDto.FirstName.ToLower()}-{numericGuidString}";

            var user = new User
            {
                UserUID = userUID,
                PasswordHash = hashedPassword,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Dob = userDto.Dob,
                Mobile = userDto.Mobile,
                Address = userDto.Address,
                State = userDto.State,
                Country = userDto.Country,
                RelativeName = userDto.RelativeName,
                RelativeNumber = userDto.RelativeNumber,
                IllnessDetails = userDto.IllnessDetails,
                Role = "patient",
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { userid = user.UserUID });
        });
    }
}
