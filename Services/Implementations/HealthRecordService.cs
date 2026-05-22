using HealthcareApp.Dtos;
using HealthcareApp.Enums;
using HealthcareApp.Exceptions;
using HealthcareApp.Models;
using HealthcareApp.Repositories;

namespace HealthcareApp.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public HealthRecordService(
            IHealthRecordRepository healthRecordRepository,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository)
            {
                _healthRecordRepository = healthRecordRepository;
                _appointmentRepository = appointmentRepository;
                _patientRepository = patientRepository;
                _doctorRepository = doctorRepository;
            }

        public HealthRecord AddRecord(int appointmentId, string diagnosis, string prescription, string notes)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
                throw new InvalidHealthRecordException("Diagnosis is required.");

            if (string.IsNullOrWhiteSpace(prescription))
                throw new InvalidHealthRecordException("Prescription is required.");

            if (string.IsNullOrWhiteSpace(notes))
                notes = "No additional notes.";

            var appointment = _appointmentRepository.GetById(appointmentId);

            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new InvalidAppointmentStatusException(
                    "Health record can only be added after an appointment is completed.");
            }

            bool recordAlreadyExists = _healthRecordRepository
                .GetByAppointmentId(appointmentId)
                .Any();

            if (recordAlreadyExists)
            {
                throw new DuplicateHealthRecordException(
                    "A health record already exists for this appointment.");
            }

            var record = new HealthRecord
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentId = appointment.AppointmentId,
                VisitDate = appointment.ScheduledDate,
                Diagnosis = diagnosis,
                Prescription = prescription,
                Notes = notes
            };

            _healthRecordRepository.Add(record);

            return record;
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            _patientRepository.GetById(patientId);

            var records = _healthRecordRepository
                .GetByPatientId(patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();

            if (!records.Any())
            {
                throw new NoHealthRecordsFoundException(
                    $"No health records found for patient ID: {patientId}.");
            }

            return records;
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            var records = _healthRecordRepository
                .GetByDoctorId(doctorId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();

            if (!records.Any())
            {
                throw new NoHealthRecordsFoundException(
                    $"No health records found for doctor ID: {doctorId}.");
            }

            return records;
        }

        public List<HealthRecord> GetRecordsByAppointment(int appointmentId)
        {
            _appointmentRepository.GetById(appointmentId);

            var records = _healthRecordRepository
                .GetByAppointmentId(appointmentId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();

            if (!records.Any())
            {
                throw new NoHealthRecordsFoundException(
                    $"No health records found for appointment ID: {appointmentId}.");
            }

            return records;
        }
        private HealthRecordDto BuildHealthRecordDto(HealthRecord record)
        {
            var patient = _patientRepository.GetById(record.PatientId);
            var doctor = _doctorRepository.GetById(record.DoctorId);

            return new HealthRecordDto
            {
                RecordId = record.RecordId,
                PatientName = patient.FullName,
                DoctorName = doctor.FullName,
                VisitDate = record.VisitDate,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes
            };
        }
        public List<HealthRecordDto> GetRecordSummariesByPatient(int patientId)
        {
            _patientRepository.GetById(patientId);

            var records = _healthRecordRepository
                .GetByPatientId(patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();

            if (!records.Any())
            {
                throw new NoHealthRecordsFoundException(
                    $"No health records found for patient ID: {patientId}");
            }

            return records
                .Select(BuildHealthRecordDto)
                .ToList();
        }
    }
}