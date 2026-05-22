using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Models
{
    public class HealthRecord : BaseEntity
    {
        public int RecordId
        {
            get => Id;
            set => Id = value;
        }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int AppointmentId { get; set; }

        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; }

        public string Prescription { get; set; }

        public string Notes { get; set; }

        // -------- Methods --------

        public string GetSummary()
        {
            return $"{VisitDate:yyyy-MM-dd} | Diagnosis: {Diagnosis} | Prescription: {Prescription}";
        }
    }
}
