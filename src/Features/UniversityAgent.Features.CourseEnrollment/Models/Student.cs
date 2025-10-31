namespace UniversityAgent.Features.CourseEnrollment.Models;

/// <summary>
/// Represents a student in the university system.
/// </summary>
/// <param name="Id">Unique identifier for the student</param>
/// <param name="FirstName">Student's first name</param>
/// <param name="LastName">Student's last name</param>
/// <param name="Email">Student's email address</param>
/// <param name="EnrolledCourses">Collection of course IDs the student is enrolled in</param>
public record Student(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    IReadOnlyCollection<Guid> EnrolledCourses)
{
    /// <summary>
    /// Creates a new student with default values
    /// </summary>
    public static Student Create(string firstName, string lastName, string email) =>
        new(Guid.NewGuid(), firstName, lastName, email, Array.Empty<Guid>());

    /// <summary>
    /// Adds a course enrollment to the student
    /// </summary>
    public Student WithCourseEnrollment(Guid courseId) =>
        this with { EnrolledCourses = EnrolledCourses.Append(courseId).ToList() };

    /// <summary>
    /// Removes a course enrollment from the student
    /// </summary>
    public Student WithoutCourseEnrollment(Guid courseId) =>
        this with { EnrolledCourses = EnrolledCourses.Where(id => id != courseId).ToList() };
}
