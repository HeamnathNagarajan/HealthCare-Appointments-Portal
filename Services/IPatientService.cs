using HealthcareApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Services
{

    public interface IPatientService
    {
        Patient RegisterPatient(Patient patient);

        Patient GetPatientById(int patientId);

        List<Patient> GetAllPatients();

        Patient UpdatePatient(Patient patient);
    }

}
