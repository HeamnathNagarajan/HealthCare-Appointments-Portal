using System;

namespace HealthcareApp.Dtos
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; }

        public string Prescription { get; set; }

        public string Notes { get; set; }

        public string GetSummary()
        {
            return $"Record ID: {RecordId} | Patient: {PatientName} | Doctor: Dr. {DoctorName} | " +
                   $"Visit Date: {VisitDate:yyyy-MM-dd} | Diagnosis: {Diagnosis} | " +
                   $"Prescription: {Prescription} | Notes: {Notes}";
        }
    }
}