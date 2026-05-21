using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Utilities;

namespace HealthCare_Appointments_Portal.Models;

public class HealthRecord
{
    public Guid RecordId { get; set; } = Guid.NewGuid();

    public required Patient Patient { get; set; }

    public required Doctor Doctor { get; set; }

    public DateOnly VisitDate { get; set; }

    public string Diagnosis { get; set; } = string.Empty;

    public string Prescription { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    // Returns HealthRecord summary
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
