namespace UniversityAgent.Features.CourseEnrollment.DTOs;

/// <summary>
/// Response DTO for enrollment operations
/// </summary>
/// <param name="Success">Indicates whether the enrollment was successful</param>
/// <param name="Message">Message describing the result</param>
/// <param name="StudentId">ID of the student</param>
/// <param name="CourseId">ID of the course</param>
public record EnrollmentResponse(
    bool Success,
    string Message,
    Guid StudentId,
    Guid CourseId);
