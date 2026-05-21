using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DataStore _dataStore;

        // Dependency Injection
       public AppointmentRepository(
             DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public void AddAppointment(
            Appointment appointment)
        {
            _dataStore.Appointments.Add(appointment);
        }

        public Appointment? GetAppointmentById(
            Guid appointmentId)
        {
            return _dataStore.Appointments
                .FirstOrDefault(a =>
                    a.AppointmentId == appointmentId);
        }
        public List<Appointment> GetAllAppointments()
        {
            return _dataStore.Appointments;
        }
        public void UpdateAppointment(Appointment updatedAppointment)
        {
            Appointment? existingAppointment = _dataStore.Appointments
                .FirstOrDefault(a =>
                    a.AppointmentId == updatedAppointment.AppointmentId);

            if (existingAppointment != null)
            {
                existingAppointment.Patient =
                    updatedAppointment.Patient ?? existingAppointment.Patient;

                existingAppointment.Doctor =
                    updatedAppointment.Doctor ?? existingAppointment.Doctor;

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
                    string.IsNullOrWhiteSpace(updatedAppointment.CancellationReason)
                    ? existingAppointment.CancellationReason
                    : updatedAppointment.CancellationReason;
            }
  
        }
        public void DeleteAppointmentById(Guid appointmentid)
        {

            Appointment? appointment = _dataStore.Appointments
                .FirstOrDefault(a=>
                    a.AppointmentId == appointmentid);

            if (appointment != null)
            {

                _dataStore.Appointments.Remove(appointment);
            }
        }

    }
}
