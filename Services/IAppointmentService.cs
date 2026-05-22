using HealthcareApp.Models;
using System;
using System.Collections.Generic;
using HealthcareApp.Dtos;

namespace HealthcareApp.Services
{
    public interface IAppointmentService
    {
        Appointment BookAppointment(int patientId, int doctorId, DateTime date, TimeSpan slotStartTime);

        Appointment ConfirmAppointment(int appointmentId);

        Appointment CancelAppointment(int appointmentId, string reason);

        Appointment CompleteAppointment(int appointmentId);

        List<Appointment> GetAppointmentsByPatient(int patientId);

        List<Appointment> GetAppointmentsByDoctor(int doctorId);

        List<Appointment> GetUpcomingAppointments();

        List<TimeSpan> GetAvailableSlotsForDoctor(int doctorId, DateTime date);

        List<Appointment> GetPendingAppointmentsByPatient(int patientId);

        List<Appointment> GetTodayConfirmedAppointmentsByDoctor(int doctorId);

        List<AppointmentDto> GetAppointmentSummariesByPatient(int patientId);

        List<AppointmentDto> GetPendingAppointmentSummariesByPatient(int patientId);

        List<AppointmentDto> GetTodayConfirmedAppointmentSummariesByDoctor(int doctorId);

        AppointmentDto GetAppointmentSummaryById(int appointmentId);
    }
}