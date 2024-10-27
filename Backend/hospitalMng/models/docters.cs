public class Doctor
{
    public string DoctorId { get; set; } = Guid.NewGuid().ToString(); // Automatically generates a unique GUID
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Speciality { get; set; }
    public string Location { get; set; }
    public string EducationQualification { get; set; }
    public int YearsOfExperience { get; set; }
}
