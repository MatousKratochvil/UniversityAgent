using System.Collections.Concurrent;
using UniversityAgent.Features.CourseEnrollment.Models;

namespace UniversityAgent.Features.CourseEnrollment.Repositories;

/// <summary>
/// In-memory implementation of student repository for demo purposes
/// </summary>
public class InMemoryStudentRepository : IStudentRepository
{
    private readonly ConcurrentDictionary<Guid, Student> _students = new();

    public Task<Student?> GetByIdAsync(Guid id)
    {
        _students.TryGetValue(id, out var student);
        return Task.FromResult(student);
    }

    public Task<IReadOnlyCollection<Student>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyCollection<Student>>(_students.Values.ToList());
    }

    public Task<Student> AddAsync(Student student)
    {
        _students.TryAdd(student.Id, student);
        return Task.FromResult(student);
    }

    public Task<Student> UpdateAsync(Student student)
    {
        _students[student.Id] = student;
        return Task.FromResult(student);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(_students.TryRemove(id, out _));
    }
}
