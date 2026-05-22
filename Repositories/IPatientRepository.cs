using HealthcareApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Repositories
{
    public interface IPatientRepository
    {
        void Add(Patient patient);

        Patient GetById(int patientId);

        List<Patient> GetAll();

        void Update(Patient patient);
    }
}