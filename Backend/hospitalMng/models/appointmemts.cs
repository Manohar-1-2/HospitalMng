using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Appointment
{
    [Key]
    public int AppointmentID { get; set; }

    // Foreign key to Doctor
   public string DoctorID { get; set; }
    [ForeignKey("DoctorID")]
    public Doctor Doctor { get; set; }

    // Foreign key to Slot
    public int SlotID { get; set; }
    [ForeignKey("SlotID")]
    public Slot Slot { get; set; }

    [Required]
    public string UserUID { get; set; }  // ID of the patient

    [Required]
    public DateTime BookedDateTime { get; set; }
}

public class Slot
{
    [Key]
    public int SlotID { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Required]
    public bool IsBooked { get; set; } = false;

    // Foreign key to Doctor
    public string DoctorID { get; set; }
    [ForeignKey("DoctorID")]
    public Doctor Doctor { get; set; }

    // Navigation property for related appointment
    public Appointment Appointment { get; set; }
}
