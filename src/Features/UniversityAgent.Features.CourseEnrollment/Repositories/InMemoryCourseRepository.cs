using System.Collections.Concurrent;
using UniversityAgent.Features.CourseEnrollment.Models;

namespace UniversityAgent.Features.CourseEnrollment.Repositories;

/// <summary>
/// In-memory implementation of course repository for demo purposes
/// </summary>
public class InMemoryCourseRepository : ICourseRepository
{
    private readonly ConcurrentDictionary<Guid, Course> _courses = new();

    public Task<Course?> GetByIdAsync(Guid id)
    {
        _courses.TryGetValue(id, out var course);
        return Task.FromResult(course);
    }

    public Task<IReadOnlyCollection<Course>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyCollection<Course>>(_courses.Values.ToList());
    }

    public Task<Course> AddAsync(Course course)
    {
        _courses.TryAdd(course.Id, course);
        return Task.FromResult(course);
    }

    public Task<Course> UpdateAsync(Course course)
    {
        _courses[course.Id] = course;
        return Task.FromResult(course);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(_courses.TryRemove(id, out _));
    }
}
