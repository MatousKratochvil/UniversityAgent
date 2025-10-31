namespace UniversityAgent.Features.CourseEnrollment.Models;

/// <summary>
/// Represents a teacher in the university system.
/// </summary>
/// <param name="Id">Unique identifier for the teacher</param>
/// <param name="FirstName">Teacher's first name</param>
/// <param name="LastName">Teacher's last name</param>
/// <param name="Email">Teacher's email address</param>
/// <param name="Department">Department the teacher belongs to</param>
/// <param name="TaughtCourses">Collection of course IDs the teacher teaches</param>
public record Teacher(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Department,
    IReadOnlyCollection<Guid> TaughtCourses)
{
    /// <summary>
    /// Creates a new teacher with default values
    /// </summary>
    public static Teacher Create(string firstName, string lastName, string email, string department) =>
        new(Guid.NewGuid(), firstName, lastName, email, department, Array.Empty<Guid>());

    /// <summary>
    /// Adds a course to the teacher's taught courses
    /// </summary>
    public Teacher WithCourse(Guid courseId) =>
        this with { TaughtCourses = TaughtCourses.Append(courseId).ToList() };

    /// <summary>
    /// Removes a course from the teacher's taught courses
    /// </summary>
    public Teacher WithoutCourse(Guid courseId) =>
        this with { TaughtCourses = TaughtCourses.Where(id => id != courseId).ToList() };
}
