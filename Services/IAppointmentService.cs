using HealthcareApp.Models;
using System;
using System.Collections.Generic;

namespace HealthcareApp.Services
{
    public interface IAppointmentService
    {
        Appointment BookAppointment(int patientId, int doctorId, DateOnly date);

        Appointment ConfirmAppointment(int appointmentId);

        Appointment CancelAppointment(int appointmentId, string reason);

        Appointment CompleteAppointment(int appointmentId);

        List<Appointment> GetAppointmentsByPatient(int patientId);

        List<Appointment> GetAppointmentsByDoctor(int doctorId);

        List<Appointment> GetUpcomingAppointments();

        List<Appointment> GetPendingAppointmentsByPatient(int patientId);

        List<Appointment> GetTodayConfirmedAppointmentsByDoctor(int doctorId);

  
    }
}