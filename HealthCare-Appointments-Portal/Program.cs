using HealthCare_Appointments_Portal.Services;
using HealthCare_Appointments_Portal.Controller;
using HealthCare_Appointments_Portal.Controllers;
using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Interfaces;
using HealthCare_Appointments_Portal.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HealthCare_Appointments_Portal;

public static class Program
{
    public static void Main(string[] args)
    {
        ServiceProvider serviceProvider =
            ConfigureServices();

        SeedData(serviceProvider);

        RunApplication(serviceProvider);
    }

    // Configure Dependency Injection
    private static ServiceProvider ConfigureServices()
    {
        ServiceCollection services = new();

        // Register Data Store
        services.AddSingleton<DataStore>();

        // Register Repositories
        services.AddScoped<IPatientRepository,
            PatientRepository>();

        services.AddScoped<IDoctorRepository,
            DoctorRepository>();

        services.AddScoped<IAppointmentRepository,
            AppointmentRepository>();

        services.AddScoped<IHealthRecordRepository,
            HealthRecordRepository>();

        // Register Services
        services.AddScoped<IPatientService,
            PatientService>();

        services.AddScoped<IDoctorService,
            DoctorService>();

        services.AddScoped<IAppointmentService,
            AppointmentService>();

        services.AddScoped<IHealthRecordService,
            HealthRecordService>();

        // Register Controllers
        services.AddScoped<PatientController>();

        services.AddScoped<DoctorController>();

        services.AddScoped<AppointmentController>();

        services.AddScoped<HealthRecordController>();

        services.AddScoped<ManagementController>();

        return services.BuildServiceProvider();
    }

    // Seed Initial Dummy Data
    private static void SeedData(
        ServiceProvider serviceProvider)
    {
        DataStore dataStore =
            serviceProvider
            .GetRequiredService<DataStore>();

        DataSeeder.Seed(dataStore);
    }

    // Start Application
    private static void RunApplication(
        ServiceProvider serviceProvider)
    {
        ManagementController controller =
            serviceProvider
            .GetRequiredService<ManagementController>();

        controller.Run();
    }
}
