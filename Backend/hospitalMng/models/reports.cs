public class Report
{

    public int Id { get; set; } // Primary key
    public string UserUID { get; set; } // User ID
    public string Title { get; set; } // Title of the report
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; } // Timestamp for when the report is created
}
