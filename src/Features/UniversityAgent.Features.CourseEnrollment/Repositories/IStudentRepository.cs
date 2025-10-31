using UniversityAgent.Features.CourseEnrollment.Models;

namespace UniversityAgent.Features.CourseEnrollment.Repositories;

/// <summary>
/// Repository interface for Student entities
/// </summary>
public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Student>> GetAllAsync();
    Task<Student> AddAsync(Student student);
    Task<Student> UpdateAsync(Student student);
    Task<bool> DeleteAsync(Guid id);
}
