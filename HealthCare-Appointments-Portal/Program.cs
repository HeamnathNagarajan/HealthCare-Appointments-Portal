using HealthCare_Appointments_Portal;
using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Intefaces;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Repositories;
using HealthCare_Appointments_Portal.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Data
services.AddSingleton<DataStore>();

// Repositories
services.AddScoped<IAppointmentRepository, AppointmentRepository>();
services.AddScoped<IDoctorRepository, DoctorRepository>();
services.AddScoped<IPatientRepository, PatientRepository>();
services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

// Services
services.AddScoped<IAppointmentService, AppointmentService>();
services.AddScoped<IDoctorService, DoctorService>();
services.AddScoped<IPatientService, PatientService>();
services.AddScoped<IHealthRecordService, HealthRecordService>();

// Controller
services.AddScoped<AppController>();

var provider = services.BuildServiceProvider();

// Seed
var appointmentService = provider.GetRequiredService<IAppointmentService>();
var doctorService = provider.GetRequiredService<IDoctorService>();
var patientService = provider.GetRequiredService<IPatientService>();
var healthService = provider.GetRequiredService<IHealthRecordService>();

DataSeeder.Seed(patientService, doctorService, appointmentService, healthService);

// Run App
var app = provider.GetRequiredService<AppController>();
app.Run();