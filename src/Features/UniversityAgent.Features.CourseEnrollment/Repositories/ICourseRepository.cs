using UniversityAgent.Features.CourseEnrollment.Models;

namespace UniversityAgent.Features.CourseEnrollment.Repositories;

/// <summary>
/// Repository interface for Course entities
/// </summary>
public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Course>> GetAllAsync();
    Task<Course> AddAsync(Course course);
    Task<Course> UpdateAsync(Course course);
    Task<bool> DeleteAsync(Guid id);
}
