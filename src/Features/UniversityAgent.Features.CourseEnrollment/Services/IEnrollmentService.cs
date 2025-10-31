using UniversityAgent.Features.CourseEnrollment.Common;
using UniversityAgent.Features.CourseEnrollment.DTOs;

namespace UniversityAgent.Features.CourseEnrollment.Services;

/// <summary>
/// Service interface for course enrollment operations
/// </summary>
public interface IEnrollmentService
{
    Task<Result<EnrollmentResponse>> EnrollStudentAsync(EnrollmentRequest request);
    Task<Result<EnrollmentResponse>> UnenrollStudentAsync(EnrollmentRequest request);
}
