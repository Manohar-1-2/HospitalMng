
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public static class PatientEndpoints
{
    public static void MapPatientRoutes(this WebApplication app)
    {
        app.MapGet("/patient", [Authorize(Roles = "patient")] async (ApplicationDbContext dbContext, HttpContext httpContext) =>
        {
            var userUid = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (userUid == null)
            {
                return Results.Unauthorized();
            }

            var patientDetails = await dbContext.Users
                .Where(u => u.UserUID == userUid)
                .Select(u => new
                {
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
                        .Where(r => r.UserUID == u.UserUID)
                        .Select(r => new
                        {
                            r.Title,
                            r.FileName
                        })
                        .ToList(),
                    Appointments = dbContext.Appointments
                        .Where(a => a.UserUID == u.UserUID)
                        .Select(a => new
                        {
                            a.AppointmentID,
                            a.BookedDateTime,
                            Slot = new
                            {
                                a.Slot.Date,
                                a.Slot.Time
                            },
                            Doctor = new
                            {
                                a.Doctor.FirstName,
                                a.Doctor.LastName,
                                a.Doctor.Speciality
                            }
                        })
                .ToList()
                })
                .FirstOrDefaultAsync();

            if (patientDetails == null)
            {
                return Results.NotFound("Patient details not found.");
            }

            return Results.Ok(patientDetails);
        });
    }
}
