using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Numerics;

// Define the DbContext and User class before the top-level statements


// This is where the top-level statements begin
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200") // Allow your Angular app's origin
            .AllowAnyHeader()
            .AllowAnyMethod());
});
// Configure SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Authentication and Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKeyyourSecretKeyyourSecretKey")),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use Authentication and Authorization middleware
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

// Map your endpoints or routes
app.MapPost("/register", async (ApplicationDbContext dbContext, RegistrationDto userDto) =>
{
    // Hash the password
    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
    
    var guid = Guid.NewGuid();
    BigInteger numericGuid = new BigInteger(guid.ToByteArray());
    string numericGuidString = numericGuid.ToString().Substring(1, 4);
    string userUID = $"{userDto.FirstName.ToLower()}-{numericGuidString}";
     var user = new User
    {
        UserUID = userUID, // Generate a unique UserUID
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

app.MapPost("/login", async (ApplicationDbContext dbContext, UserDtoLogin userDto) =>
{
    var user = await dbContext.Users.SingleOrDefaultAsync(u => u.UserUID == userDto.UserUID);
    Console.WriteLine(userDto.UserUID);

    if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
    {
        return Results.Unauthorized();
    }
    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes("yourSecretKeyyourSecretKeyyourSecretKey");  // Replace with your actual secret key
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserUID),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role)
        }),
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var tokenString = tokenHandler.WriteToken(token);
    Console.WriteLine(user.Role);
    return Results.Ok(new { Token = tokenString,Role=user.Role});
});
app.MapGet("/admin", [Authorize(Roles = "admin")] async (ApplicationDbContext dbContext) =>
{
    // Query to get all users who are not admins
    var nonAdminUsers = await dbContext.Users
        .Where(u => u.Role != "admin") // Assuming 'Role' is a column in your 'Users' table
        .Select(u => new // Create an anonymous object
        {
            u.UserUID,
            u.FirstName,
            u.LastName,
            u.Dob,
            u.Mobile,
            u.Address,
            u.State,
            u.Country,
            u.RelativeName,
            u.RelativeNumber,
            u.IllnessDetails,
            Reports = dbContext.Reports
                .Where(r => r.UserUID == u.UserUID) // Fetch reports for each user
                .Select(r => new // Select relevant fields for reports
                {
                    r.Title,
                    r.UserUID,
                    r.FileName
                })
                .ToList() 
        })
        .ToListAsync();

    // Return the list of non-admin users
    return Results.Ok(nonAdminUsers);
});
app.MapGet("/admin/getdoctersDetails", [Authorize(Roles = "admin")] async (ApplicationDbContext dbContext) =>
{
    var doctors = await dbContext.Doctors.ToListAsync();
    return Results.Ok(doctors);
});

app.MapPost("/admin/adddoctersDetails", [Authorize(Roles = "admin")] async (Doctor doctor, ApplicationDbContext dbContext) =>
{
    // Add the new doctor to the database
    await dbContext.Doctors.AddAsync(doctor);
    await dbContext.SaveChangesAsync();

    // Return a success message with the new DoctorId
    return Results.Ok(new { Message = "Doctor added successfully", DoctorId = doctor.DoctorId });
});
app.MapPost("/admin/editpatientdetials", [Authorize(Roles = "admin")] async (ApplicationDbContext dbContext,User user) =>
{
    var patient = await dbContext.Users.FirstOrDefaultAsync(p => p.UserUID == user.UserUID);
     if (patient == null)
    {
        return Results.NotFound("Patient not found.");
    }
    // Update patient details
    patient.FirstName = user.FirstName;
    patient.LastName = user.LastName;
    patient.Dob = user.Dob;
    patient.Mobile = user.Mobile;
    patient.Address = user.Address;
    patient.State = user.State;
    patient.Country = user.Country;
    patient.RelativeName = user.RelativeName;
    patient.RelativeNumber = user.RelativeNumber;
    patient.IllnessDetails = user.IllnessDetails;

    // Save changes to the database
    await dbContext.SaveChangesAsync();

    return Results.Ok("Patient details updated successfully.");
    
});

app.MapPost("/admin/uploadreports", [Authorize(Roles = "admin")] async (ApplicationDbContext dbContext, HttpContext httpContext) =>
{
    // Ensure the request contains multipart/form-data
    if (!httpContext.Request.HasFormContentType)
    {
        return Results.BadRequest("Invalid content type.");
    }

    var form = await httpContext.Request.ReadFormAsync();
    
    var userUID = form["UserUID"].ToString();
    var title = form["title"].ToString();
    var file = form.Files.GetFile("pdf");
    Console.WriteLine($"UserUID: {userUID}, Title: {title}, Files Count: {file==null}");
    if (string.IsNullOrEmpty(userUID) || string.IsNullOrEmpty(title) || file == null)
    {
        return Results.BadRequest("UserUID, title, and file are required.");
    }

    // Define the file path to save the uploaded file
    var filePath = Path.Combine("uploads", file.FileName); // Adjust the path as necessary

    // Save the uploaded file
    using (var fileStream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(fileStream);
    }

    // Create a new report entity
    var report = new Report
    {
        UserUID = userUID,
        Title = title,
        FileName = file.FileName, // Store the file path in the database
        CreatedAt = DateTime.UtcNow // Set the timestamp to the current time
    };

    // Save the report to the database
    await dbContext.Reports.AddAsync(report);
    await dbContext.SaveChangesAsync();

    return Results.Ok("Report uploaded successfully.");
});
app.MapGet("/admin/downloadreport/{fileName}", [Authorize] async (string fileName, ApplicationDbContext dbContext, HttpContext httpContext) =>
{
    // Get the UserUID of the currently logged-in user
    var currentUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    var filePath = Path.Combine("uploads", fileName); 

    Console.WriteLine("hai");
    if (!System.IO.File.Exists(filePath))
    {
        return Results.NotFound("File not found.");
    }

    // Get the report from the database
    var report = await dbContext.Reports.FirstOrDefaultAsync(r => r.FileName == fileName);

    if (report == null)
    {
        return Results.NotFound("Report not found.");
    }

    // Check if the user is an admin or the owner of the report
    var userRole = httpContext.User.IsInRole("admin");

    if (!userRole && report.UserUID != currentUserId)
    {
        return Results.Forbid();
    }

    // Return the file as a downloadable response
    var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
    return Results.File(fileBytes, "application/pdf", fileName); // Change MIME type as necessary
});

app.MapGet("/patient", [Authorize(Roles = "patient")] async (ApplicationDbContext dbContext, HttpContext httpContext) =>
{
    // Retrieve the user UID from claims
    
   var userUid = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
    Console.WriteLine("jjj");
    if (userUid == null)
    {
        return Results.Unauthorized();
    }

    // Assuming the claim value is a string
   

    // Fetch patient details from the database based on userUID
    var patientDetails = await dbContext.Users
        .FirstOrDefaultAsync(p => p.UserUID == userUid);

    if (patientDetails == null)
    {
        return Results.NotFound("Patient not found.");
    }

    // Return the patient details
    return Results.Ok(patientDetails);
});


app.Run();

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Define your DbSet(s) here
    public DbSet<User> Users { get; set; }
    public DbSet<Report> Reports { get; set; }
     public DbSet<Doctor> Doctors { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string UserUID { get; set; }
    public string PasswordHash { get; set; }
     public string FirstName { get; set; } // Corresponds to firstName
    public string LastName { get; set; }   // Corresponds to lastName
    public DateTime Dob { get; set; }      // Corresponds to dob
    public string Mobile { get; set; }     // Corresponds to mobile
    public string Address { get; set; }    // Corresponds to address
    public string State { get; set; }      // Corresponds to state
    public string Country { get; set; }     // Corresponds to country
    public string RelativeName { get; set; } // Corresponds to relativeName
    public string RelativeNumber { get; set; } // Corresponds to relativeNumber
    public string IllnessDetails { get; set; }
    public string Role { get; set; } // Corresponds to illnessDetails
}

public class Doctor
{
    public Guid DoctorId { get; set; } = Guid.NewGuid(); // Automatically generates a unique GUID
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Speciality { get; set; }
    public string Location { get; set; }
    public string EducationQualification { get; set; }
    public int YearsOfExperience { get; set; }
}

public class Report
{

    public int Id { get; set; } // Primary key
    public string UserUID { get; set; } // User ID
    public string Title { get; set; } // Title of the report
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; } // Timestamp for when the report is created
}

public class UserDtoLogin
{
    public string UserUID { get; set; }
    public string Password { get; set; }
}

public class RegistrationDto
{
    public string FirstName { get; set; } // Corresponds to firstName
    public string LastName { get; set; }   // Corresponds to lastName
    public DateTime Dob { get; set; }      // Corresponds to dob
    public string Mobile { get; set; }     // Corresponds to mobile
    public string Address { get; set; }    // Corresponds to address
    public string State { get; set; }      // Corresponds to state
    public string Country { get; set; }     // Corresponds to country
    public string RelativeName { get; set; } // Corresponds to relativeName
    public string RelativeNumber { get; set; } // Corresponds to relativeNumber
    public string IllnessDetails { get; set; } // Corresponds to illnessDetails
    public string Password { get; set; }    // Corresponds to password
}