using Microsoft.Extensions.DependencyInjection;
using UniversityAgent.Features.CourseEnrollment.Repositories;
using UniversityAgent.Features.CourseEnrollment.Services;

namespace UniversityAgent.Features.CourseEnrollment;

/// <summary>
/// Extension methods for configuring course enrollment services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCourseEnrollmentFeature(this IServiceCollection services)
    {
        // Register repositories as singletons for in-memory storage
        services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
        services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();

        // Register services
        services.AddScoped<IEnrollmentService, EnrollmentService>();

        return services;
    }
}
