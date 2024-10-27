
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


public static class AdminEndpoints
{
    public static void MapAdminRoutes(this WebApplication app)
    {
        app.MapGet("/admin", [Authorize(Roles = "admin")] async (ApplicationDbContext dbContext) =>
        {
            var nonAdminUsers = await dbContext.Users
                .Where(u => u.Role != "admin")
                .Select(u => new
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
                        .Where(r => r.UserUID == u.UserUID)
                        .Select(r => new
                        {
                            r.Title,
                            r.UserUID,
                            r.FileName
                        })
                        .ToList()
                })
                .ToListAsync();

            return Results.Ok(nonAdminUsers);
        });
        app.MapGet("/admin/searchdoctors", [Authorize(Roles = "admin")] async (string location, string specialty, ApplicationDbContext dbContext) =>
        {
            var doctors = await dbContext.Doctors
                .Where(d => d.Location == location && d.Speciality == specialty)
                .ToListAsync();
            return Results.Ok(doctors);
        });

        app.MapGet("/admin/getslots", [Authorize(Roles = "admin")] async (string doctorId, DateTime date, ApplicationDbContext dbContext) =>
        {
            
            var startOfDay = date.Date; 
            var endOfDay = startOfDay.AddDays(1); 
            Console.WriteLine(date);
            var emptySlots = await dbContext.Slots
                .Where(slot => slot.DoctorID == doctorId && slot.Date.Date==startOfDay && !slot.IsBooked)
                .ToListAsync();
            
            

            Console.WriteLine(doctorId);
            if (emptySlots.Count == 0)
            {
                return Results.NotFound(new { Message = "No empty slots found for the specified doctor on this date." });
            }
            return Results.Ok(emptySlots);
        });
        app.MapGet("/admin/book", [Authorize(Roles = "admin")] async (string userId, int slotId, ApplicationDbContext dbContext) =>
        {
            // Check if the slot exists and is not already booked
            var slot = await dbContext.Slots.FirstOrDefaultAsync(s => s.SlotID == slotId && !s.IsBooked);
            if (slot == null)
            {
                return Results.NotFound("Slot not available or already booked.");
            }

            // Fetch doctor associated with the slot
            var doctor = await dbContext.Doctors.FindAsync(slot.DoctorID);
            if (doctor == null)
            {
                return Results.NotFound("Doctor not found for this slot.");
            }

            // Check if user exists (optional: depends on your database and setup)
            Console.WriteLine(userId);
            var user = await dbContext.Users.FirstOrDefaultAsync(s=>s.UserUID==userId);
            if (user == null)
            {
                return Results.NotFound("User not found.");
            }

            // Create the appointment
            var appointment = new Appointment
            {
                DoctorID = doctor.DoctorId,
                SlotID = slot.SlotID,
                UserUID = userId,
                BookedDateTime = DateTime.Now
            };

            // Update slot as booked
            slot.IsBooked = true;
            dbContext.Slots.Update(slot);
            // Add the new appointment and save changes
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            return Results.Ok("Appointment successfully booked.");
        });

        app.MapGet("/admin/searchpatients", [Authorize(Roles = "admin")] async (string useruid, ApplicationDbContext dbContext) =>
        {
            Console.WriteLine(useruid); 
            var patients = await dbContext.Users
                .Where(d => d.UserUID == useruid)
                .ToListAsync();
            return Results.Ok(patients);
        });
        app.MapGet("/admin/getdoctersDetails", [Authorize(Roles = "admin")] async (ApplicationDbContext dbContext) =>
        {
            var doctors = await dbContext.Doctors.ToListAsync();
            return Results.Ok(doctors);
        });

        app.MapPost("/admin/adddoctersDetails", [Authorize(Roles = "admin")] async (Doctor doctor, ApplicationDbContext dbContext) =>
        {
            await dbContext.Doctors.AddAsync(doctor);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Message = "Doctor added successfully", DoctorId = doctor.DoctorId });
        });

        app.MapPost("/admin/editpatientdetials", [Authorize(Roles = "admin")] async (ApplicationDbContext dbContext, User user) =>
        {
            var patient = await dbContext.Users.FirstOrDefaultAsync(p => p.UserUID == user.UserUID);

            if (patient == null)
            {
                return Results.NotFound("Patient not found.");
            }

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

            await dbContext.SaveChangesAsync();

            return Results.Ok("Patient details updated successfully.");
        });


    }
}
