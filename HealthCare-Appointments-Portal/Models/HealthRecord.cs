using HealthCare_Appointments_Portals.Utilities;

namespace HealthCare_Appointments_Portal.Models;

public class HealthRecord
{
    public Guid RecordId { get; set; } = Guid.NewGuid();

    public int DisplayId { get; set; }

    public required Patient Patient { get; set; }

    public required Doctor Doctor { get; set; }

    public Appointment? Appointment { get; set; }

    //Visit date 
    public DateOnly VisitDate { get; set; }

    public string Diagnosis { get; set; } = string.Empty;

    public string Prescription { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    //Created timestamp 
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    //Summary method
    public string GetSummary()
    {
        return string.Format(
            Constants.HealthRecordSummaryFormat,
            VisitDate,
            Patient.FullName,
            Doctor.FullName,
            Diagnosis,
            Prescription,
            Notes
        );
    }
}
