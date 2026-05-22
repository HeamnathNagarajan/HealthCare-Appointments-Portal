using HealthcareApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.Dtos;

namespace HealthcareApp.Services
{

    public interface IHealthRecordService
    {
        HealthRecord AddRecord(int appointmentId, string diagnosis, string prescription, string notes);

        List<HealthRecord> GetRecordsByPatient(int patientId);

        List<HealthRecord> GetRecordsByDoctor(int doctorId);

        List<HealthRecord> GetRecordsByAppointment(int appointmentId);
        List<HealthRecordDto> GetRecordSummariesByPatient(int patientId);
    }

}
