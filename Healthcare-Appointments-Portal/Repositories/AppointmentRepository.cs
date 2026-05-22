using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Repositories
{

    public class AppointmentRepository : IAppointmentRepository
    {

        private readonly DataStore _dataStore;

        // Dependency Injection
        public AppointmentRepository(DataStore dataStore)
        {

            _dataStore = dataStore;
        }

        // Add New Appointment
        public void AddAppointment(Appointment appointment)
        {

            _dataStore.Appointments
                .Add(appointment);
        }

        // Get Appointment By Id
        public Appointment? GetAppointmentById(int appointmentId)
        {

            return _dataStore.Appointments
                .FirstOrDefault(a =>
                    a.AppointmentId ==
                    appointmentId);
        }

        // Get All Appointments
        public List<Appointment> GetAllAppointments()
        {

            return _dataStore.Appointments.ToList();
        }

        // Update Existing Appointment
        public void UpdateAppointment(Appointment updatedAppointment)
        {

            Appointment? existingAppointment =
                _dataStore.Appointments
                .FirstOrDefault(a =>
                    a.AppointmentId ==
                    updatedAppointment.AppointmentId);

            if (existingAppointment != null)
            {

                existingAppointment.Patient =
                    updatedAppointment.Patient
                    ?? existingAppointment.Patient;

                existingAppointment.Doctor =
                    updatedAppointment.Doctor
                    ?? existingAppointment.Doctor;

                existingAppointment.ScheduledDate =
                    updatedAppointment.ScheduledDate == default
                    ? existingAppointment.ScheduledDate
                    : updatedAppointment.ScheduledDate;

                existingAppointment.TimeSlot =
                    updatedAppointment.TimeSlot == default
                    ? existingAppointment.TimeSlot
                    : updatedAppointment.TimeSlot;

                existingAppointment.Status =
                    updatedAppointment.Status;

                existingAppointment.CancellationReason =
                    string.IsNullOrWhiteSpace(
                        updatedAppointment.CancellationReason)
                    ? existingAppointment.CancellationReason
                    : updatedAppointment.CancellationReason;
            }
        }

        // Delete Appointment By Id
        public void DeleteAppointmentById(int appointmentId)
        {

            Appointment? appointment =
                _dataStore.Appointments
                .FirstOrDefault(a =>
                    a.AppointmentId ==
                    appointmentId);

            if (appointment != null)
            {

                _dataStore.Appointments
                    .Remove(appointment);
            }
        }
    }
}