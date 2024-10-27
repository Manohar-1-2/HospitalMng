using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Define your DbSet(s) here
    public DbSet<User> Users { get; set; }
    public DbSet<Report> Reports { get; set; }
     public DbSet<Doctor> Doctors { get; set; }
     public DbSet<Slot> Slots { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
}