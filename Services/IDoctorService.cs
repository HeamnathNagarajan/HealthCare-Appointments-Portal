using HealthcareApp.Enums;
using HealthcareApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Services
{

    public interface IDoctorService
    {
        Doctor AddDoctor(Doctor doctor);

        Doctor GetDoctorById(int doctorId);

        List<Doctor> GetAllDoctors();

        List<Doctor> SearchDoctorsBySpecialisation(Specialisation specialisation);

        Doctor UpdateDoctor(Doctor doctor);
        List<DateOnly> GetOffDays(int doctorId);

        Doctor AddOffDay(int doctorId, DateOnly offDay);

        Doctor RemoveOffDay(int doctorId, DateOnly offDay);
    }


}
