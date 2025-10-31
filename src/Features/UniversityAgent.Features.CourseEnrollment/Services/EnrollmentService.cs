using UniversityAgent.Features.CourseEnrollment.Common;
using UniversityAgent.Features.CourseEnrollment.DTOs;
using UniversityAgent.Features.CourseEnrollment.Models;
using UniversityAgent.Features.CourseEnrollment.Repositories;

namespace UniversityAgent.Features.CourseEnrollment.Services;

/// <summary>
/// Service for handling course enrollment operations using functional approach
/// </summary>
public class EnrollmentService : IEnrollmentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;

    public EnrollmentService(
        IStudentRepository studentRepository,
        ICourseRepository courseRepository)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<Result<EnrollmentResponse>> EnrollStudentAsync(EnrollmentRequest request)
    {
        var studentResult = await GetStudentAsync(request.StudentId);
        if (studentResult.IsFailure)
            return Result<EnrollmentResponse>.Failure(studentResult.Error!);

        var courseResult = await GetCourseAsync(request.CourseId);
        if (courseResult.IsFailure)
            return Result<EnrollmentResponse>.Failure(courseResult.Error!);

        var validationResult = ValidateEnrollment(
            studentResult.Value!,
            courseResult.Value!);
        
        if (validationResult.IsFailure)
            return Result<EnrollmentResponse>.Failure(validationResult.Error!);

        return await PerformEnrollmentAsync(
            studentResult.Value!,
            courseResult.Value!);
    }

    public async Task<Result<EnrollmentResponse>> UnenrollStudentAsync(EnrollmentRequest request)
    {
        var studentResult = await GetStudentAsync(request.StudentId);
        if (studentResult.IsFailure)
            return Result<EnrollmentResponse>.Failure(studentResult.Error!);

        var courseResult = await GetCourseAsync(request.CourseId);
        if (courseResult.IsFailure)
            return Result<EnrollmentResponse>.Failure(courseResult.Error!);

        var validationResult = ValidateUnenrollment(
            studentResult.Value!,
            courseResult.Value!);
        
        if (validationResult.IsFailure)
            return Result<EnrollmentResponse>.Failure(validationResult.Error!);

        return await PerformUnenrollmentAsync(
            studentResult.Value!,
            courseResult.Value!);
    }

    private async Task<Result<Student>> GetStudentAsync(Guid studentId)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        return student is not null
            ? Result<Student>.Success(student)
            : Result<Student>.Failure($"Student with ID {studentId} not found");
    }

    private async Task<Result<Course>> GetCourseAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        return course is not null
            ? Result<Course>.Success(course)
            : Result<Course>.Failure($"Course with ID {courseId} not found");
    }

    private static Result<bool> ValidateEnrollment(Student student, Course course)
    {
        if (course.IsStudentEnrolled(student.Id))
            return Result<bool>.Failure("Student is already enrolled in this course");

        if (!course.HasAvailableSeats)
            return Result<bool>.Failure("Course has no available seats");

        return Result<bool>.Success(true);
    }

    private static Result<bool> ValidateUnenrollment(Student student, Course course)
    {
        if (!course.IsStudentEnrolled(student.Id))
            return Result<bool>.Failure("Student is not enrolled in this course");

        return Result<bool>.Success(true);
    }

    private async Task<Result<EnrollmentResponse>> PerformEnrollmentAsync(
        Student student,
        Course course)
    {
        var updatedStudent = student.WithCourseEnrollment(course.Id);
        var updatedCourse = course.WithStudentEnrollment(student.Id);

        await _studentRepository.UpdateAsync(updatedStudent);
        await _courseRepository.UpdateAsync(updatedCourse);

        return Result<EnrollmentResponse>.Success(
            new EnrollmentResponse(
                true,
                $"Student {student.FirstName} {student.LastName} successfully enrolled in {course.Name}",
                student.Id,
                course.Id));
    }

    private async Task<Result<EnrollmentResponse>> PerformUnenrollmentAsync(
        Student student,
        Course course)
    {
        var updatedStudent = student.WithoutCourseEnrollment(course.Id);
        var updatedCourse = course.WithoutStudentEnrollment(student.Id);

        await _studentRepository.UpdateAsync(updatedStudent);
        await _courseRepository.UpdateAsync(updatedCourse);

        return Result<EnrollmentResponse>.Success(
            new EnrollmentResponse(
                true,
                $"Student {student.FirstName} {student.LastName} successfully unenrolled from {course.Name}",
                student.Id,
                course.Id));
    }
}
