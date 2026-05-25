using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.Services
{

    public class PatientService : IPatientService
    {

        private readonly IPatientRepository _patientRepository;

        private readonly IAppointmentRepository _appointmentRepository;

        // Dependency Injection
        public PatientService(
            IPatientRepository patientRepository,
            IAppointmentRepository appointmentRepository)
        {

            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
        }

        // Add New Patient
        public void AddPatient(Patient patient)
        {

            Patient? existingPatient =
                _patientRepository
                .GetAllPatients()
                .FirstOrDefault(p =>
                    p.Email == patient.Email);

            if (existingPatient != null)
            {

                throw new DuplicatePatientException();
            }

            _patientRepository.AddPatient(patient);
        }

        // Get Patient By Id
        public Patient? GetPatientById(int patientId)
        {

            Patient? patient =
                _patientRepository
                .GetPatientById(patientId);

            if (patient == null)
            {

                throw new PatientNotFoundException();
            }

            return patient;
        }

        // Get All Patients
        public List<Patient> GetAllPatients()
        {

            return _patientRepository
                .GetAllPatients();
        }

        // Get Patient By Email
        public Patient GetPatientByEmail(
            string email)
        {
            Patient? patient =
                _patientRepository
                .GetAllPatients()
                .FirstOrDefault(p =>
                    p.Email.Equals(
                        email,
                        StringComparison.OrdinalIgnoreCase));

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            return patient;
        }

        // Update Existing Patient
        public void UpdatePatient(
            Patient updatedPatient)
        {

            Patient? existingPatient =
                _patientRepository
                .GetPatientById(
                    updatedPatient.PatientId);

            if (existingPatient == null)
            {

                throw new PatientNotFoundException();
            }

            _patientRepository
                .UpdatePatient(updatedPatient);
        }

        // Delete Patient By Id
        public void DeletePatientById(
            int patientId)
        {

            Patient? patient =
                _patientRepository
                .GetPatientById(patientId);

            if (patient == null)
            {

                throw new PatientNotFoundException();
            }

            List<Appointment> patientAppointments =
                _appointmentRepository
                .GetAllAppointments()
                .Where(a =>
                a.Patient.PatientId ==
                patientId).
                ToList();

            // Check Confirmed Appointments
            bool hasConfirmedAppointments =
                patientAppointments 
                .Any(a =>
                a.Status ==
                AppointmentStatus.Confirmed);

            if (hasConfirmedAppointments) 
            {

                throw new PatientDeletionException(); 
            }

            List<Appointment> pendingAppointments =
                patientAppointments
                .Where(a =>
                a.Status == 
                AppointmentStatus.Pending)
                .ToList();

            foreach (Appointment appointment in pendingAppointments) 
            {

                appointment.Cancel(Constants.PatientRemovedFromSystem);
                _appointmentRepository.UpdateAppointment(appointment);
            }


            _patientRepository
                .DeletePatientById(patientId);
        }
    }
}
