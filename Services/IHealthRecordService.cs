using HealthcareApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Services
{

    public interface IHealthRecordService
    {
        HealthRecord AddRecord(int appointmentId, string diagnosis, string prescription, string? notes);

        List<HealthRecord> GetRecordsByPatient(int patientId);

        List<HealthRecord> GetRecordsByDoctor(int doctorId);

        List<HealthRecord> GetRecordsByAppointment(int appointmentId);
    }

}
