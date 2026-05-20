using System.ComponentModel.DataAnnotations;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.Models;

public class HealthRecord
{
    // Unique Health Record Identifier
    public Guid RecordId { get; set; }
        = Guid.NewGuid();

    // Patient Information
    [Required(
        ErrorMessage = Constants.PatientRequired)]
    public required Patient Patient { get; set; }

    // Doctor Information
    [Required(
        ErrorMessage = Constants.DoctorRequired)]
    public required Doctor Doctor { get; set; }

    // Visit Date
    [Required(
        ErrorMessage = Constants.VisitDateRequired)]
    public DateOnly VisitDate { get; set; }

    // Diagnosis Details
    [Required(
        ErrorMessage = Constants.DiagnosisRequired)]
    public string Diagnosis { get; set; }
        = string.Empty;

    // Prescription Details
    [Required(
        ErrorMessage = Constants.PrescriptionRequired)]
    public string Prescription { get; set; }
        = string.Empty;

    // Additional Notes
    public string Notes { get; set; }
        = string.Empty;

    // Return Health Record Summary
    public string GetSummary()
    {
        return string.Format(
            Constants.HealthRecordSummaryFormat,
            VisitDate,
            Patient.FullName,
            Doctor.FullName,
            Diagnosis,
            Prescription,
            Notes);
    }
}
