using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Models
{
    public class HealthRecord 
    {
        public int RecordId { get; set; }
        

        public required Patient Patient { get; set; }

        public required Doctor Doctor { get; set; }

        public int AppointmentId { get; set; }

        public DateOnly VisitDate { get; set; }

        public string Diagnosis { get; set; }

        public string Prescription { get; set; }

        public string? Notes { get; set; }

        // -------- Methods --------

        public string GetSummary()
        {
            return $"{VisitDate:yyyy-MM-dd} | Diagnosis: {Diagnosis} | Prescription: {Prescription}";
        }
    }
}
