namespace UniversityAgent.Features.CourseEnrollment.DTOs;

/// <summary>
/// Request DTO for enrolling a student in a course
/// </summary>
/// <param name="StudentId">ID of the student to enroll</param>
/// <param name="CourseId">ID of the course to enroll in</param>
public record EnrollmentRequest(Guid StudentId, Guid CourseId);
