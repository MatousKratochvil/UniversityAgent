using UniversityAgent.Features.CourseEnrollment;
using UniversityAgent.Features.CourseEnrollment.DTOs;
using UniversityAgent.Features.CourseEnrollment.Models;
using UniversityAgent.Features.CourseEnrollment.Repositories;
using UniversityAgent.Features.CourseEnrollment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddCourseEnrollmentFeature();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Initialize some sample data
await InitializeSampleDataAsync(app.Services);

// Course Enrollment Endpoints
app.MapPost("/api/enrollments", async (
    EnrollmentRequest request,
    IEnrollmentService enrollmentService) =>
{
    var result = await enrollmentService.EnrollStudentAsync(request);
    
    return result.IsSuccess
        ? Results.Ok(result.Value)
        : Results.BadRequest(new { error = result.Error });
})
.WithName("EnrollStudent")
.WithOpenApi();

app.MapDelete("/api/enrollments/{studentId:guid}/{courseId:guid}", async (
    Guid studentId,
    Guid courseId,
    IEnrollmentService enrollmentService) =>
{
    var request = new EnrollmentRequest(studentId, courseId);
    var result = await enrollmentService.UnenrollStudentAsync(request);
    
    return result.IsSuccess
        ? Results.Ok(result.Value)
        : Results.BadRequest(new { error = result.Error });
})
.WithName("UnenrollStudent")
.WithOpenApi();

// Student Management Endpoints
app.MapGet("/api/students", async (IStudentRepository studentRepository) =>
{
    var students = await studentRepository.GetAllAsync();
    return Results.Ok(students);
})
.WithName("GetAllStudents")
.WithOpenApi();

app.MapGet("/api/students/{id:guid}", async (
    Guid id,
    IStudentRepository studentRepository) =>
{
    var student = await studentRepository.GetByIdAsync(id);
    return student is not null ? Results.Ok(student) : Results.NotFound();
})
.WithName("GetStudentById")
.WithOpenApi();

app.MapPost("/api/students", async (
    CreateStudentRequest request,
    IStudentRepository studentRepository) =>
{
    var student = Student.Create(request.FirstName, request.LastName, request.Email);
    await studentRepository.AddAsync(student);
    return Results.Created($"/api/students/{student.Id}", student);
})
.WithName("CreateStudent")
.WithOpenApi();

// Course Management Endpoints
app.MapGet("/api/courses", async (ICourseRepository courseRepository) =>
{
    var courses = await courseRepository.GetAllAsync();
    return Results.Ok(courses);
})
.WithName("GetAllCourses")
.WithOpenApi();

app.MapGet("/api/courses/{id:guid}", async (
    Guid id,
    ICourseRepository courseRepository) =>
{
    var course = await courseRepository.GetByIdAsync(id);
    return course is not null ? Results.Ok(course) : Results.NotFound();
})
.WithName("GetCourseById")
.WithOpenApi();

app.MapPost("/api/courses", async (
    CreateCourseRequest request,
    ICourseRepository courseRepository) =>
{
    var course = Course.Create(
        request.Code,
        request.Name,
        request.Description,
        request.Credits,
        request.MaxStudents,
        request.TeacherId);
    await courseRepository.AddAsync(course);
    return Results.Created($"/api/courses/{course.Id}", course);
})
.WithName("CreateCourse")
.WithOpenApi();

app.Run();

// Helper method to initialize sample data
static async Task InitializeSampleDataAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var studentRepo = scope.ServiceProvider.GetRequiredService<IStudentRepository>();
    var courseRepo = scope.ServiceProvider.GetRequiredService<ICourseRepository>();

    // Add sample students
    var student1 = Student.Create("Jan", "Novák", "jan.novak@university.cz");
    var student2 = Student.Create("Anna", "Svobodová", "anna.svobodova@university.cz");
    var student3 = Student.Create("Petr", "Dvořák", "petr.dvorak@university.cz");

    await studentRepo.AddAsync(student1);
    await studentRepo.AddAsync(student2);
    await studentRepo.AddAsync(student3);

    // Add sample courses
    var course1 = Course.Create(
        "CS101",
        "Introduction to Computer Science",
        "Basic concepts of computer science and programming",
        5,
        30);
    var course2 = Course.Create(
        "MATH201",
        "Advanced Mathematics",
        "Advanced mathematical concepts for computer science",
        6,
        25);
    var course3 = Course.Create(
        "DB301",
        "Database Systems",
        "Design and implementation of database systems",
        5,
        20);

    await courseRepo.AddAsync(course1);
    await courseRepo.AddAsync(course2);
    await courseRepo.AddAsync(course3);
}

// Request DTOs
record CreateStudentRequest(string FirstName, string LastName, string Email);
record CreateCourseRequest(string Code, string Name, string Description, int Credits, int MaxStudents, Guid? TeacherId = null);
