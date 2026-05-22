using HealthcareApp.Enums;
using HealthcareApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Repositories
{
    public interface IDoctorRepository
    {
        void Add(Doctor doctor);

        Doctor GetById(int doctorId);

        List<Doctor> GetAll();

        List<Doctor> GetBySpecialisation(Specialisation specialisation);

        void Update(Doctor doctor);
    }
}
