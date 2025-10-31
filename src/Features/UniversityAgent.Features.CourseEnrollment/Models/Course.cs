namespace UniversityAgent.Features.CourseEnrollment.Models;

/// <summary>
/// Represents a course in the university system.
/// </summary>
/// <param name="Id">Unique identifier for the course</param>
/// <param name="Code">Course code (e.g., CS101)</param>
/// <param name="Name">Course name</param>
/// <param name="Description">Course description</param>
/// <param name="Credits">Number of credits for the course</param>
/// <param name="MaxStudents">Maximum number of students allowed</param>
/// <param name="TeacherId">ID of the teacher teaching this course</param>
/// <param name="EnrolledStudentIds">Collection of student IDs enrolled in this course</param>
public record Course(
    Guid Id,
    string Code,
    string Name,
    string Description,
    int Credits,
    int MaxStudents,
    Guid? TeacherId,
    IReadOnlyCollection<Guid> EnrolledStudentIds)
{
    /// <summary>
    /// Creates a new course with default values
    /// </summary>
    public static Course Create(
        string code,
        string name,
        string description,
        int credits,
        int maxStudents,
        Guid? teacherId = null) =>
        new(Guid.NewGuid(), code, name, description, credits, maxStudents, teacherId, Array.Empty<Guid>());

    /// <summary>
    /// Checks if the course has available seats
    /// </summary>
    public bool HasAvailableSeats => EnrolledStudentIds.Count < MaxStudents;

    /// <summary>
    /// Checks if a student is already enrolled
    /// </summary>
    public bool IsStudentEnrolled(Guid studentId) => EnrolledStudentIds.Contains(studentId);

    /// <summary>
    /// Adds a student enrollment to the course
    /// </summary>
    public Course WithStudentEnrollment(Guid studentId) =>
        this with { EnrolledStudentIds = EnrolledStudentIds.Append(studentId).ToList() };

    /// <summary>
    /// Removes a student enrollment from the course
    /// </summary>
    public Course WithoutStudentEnrollment(Guid studentId) =>
        this with { EnrolledStudentIds = EnrolledStudentIds.Where(id => id != studentId).ToList() };

    /// <summary>
    /// Assigns a teacher to the course
    /// </summary>
    public Course WithTeacher(Guid teacherId) =>
        this with { TeacherId = teacherId };
}
