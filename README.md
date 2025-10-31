# UniversityAgent

Modern C# university management application with a focus on immutability and functional programming patterns.

## Project Structure

The solution is organized into feature-based modules with clean separation of concerns:

```
UniversityAgent/
├── src/
│   ├── Presentation/
│   │   └── UniversityAgent.Presentation.Web/  # ASP.NET Core Minimal API
│   └── Features/
│       └── UniversityAgent.Features.CourseEnrollment/  # Course enrollment feature
│           ├── Models/                # Immutable domain models
│           ├── DTOs/                  # Data transfer objects
│           ├── Services/              # Business logic services
│           ├── Repositories/          # Data access layer
│           └── Common/                # Shared utilities (Result type)
└── UniversityAgent.sln
```

## Architecture Principles

### Immutability
- **Records**: All domain models (Student, Teacher, Course) are implemented as C# records for immutability
- **Functional Updates**: Using `with` expressions for creating modified copies
- **Read-only Collections**: Using `IReadOnlyCollection<T>` to prevent modification

### Functional Programming
- **Result Type**: Functional error handling with `Result<T>` type
- **Pure Functions**: Methods that don't cause side effects
- **Method Chaining**: Fluent APIs with `Map`, `Bind`, `OnSuccess`, `OnFailure`

### Modern C# Features
- Primary constructors in records
- Init-only properties
- Pattern matching
- Target-typed new expressions
- Nullable reference types

## Domain Models

### Student
Immutable record representing a university student with enrolled courses.

### Teacher
Immutable record representing a teacher with department and taught courses.

### Course
Immutable record representing a course with enrollment limits and validation logic.

## Features

### Course Enrollment (v1.0)
Complete feature for managing student enrollment in courses:

- ✅ Student CRUD operations
- ✅ Course CRUD operations  
- ✅ Enroll student in course
- ✅ Unenroll student from course
- ✅ Validation (duplicate enrollment, seat availability)
- ✅ In-memory data storage (thread-safe with ConcurrentDictionary)

## API Endpoints

### Student Management
- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID
- `POST /api/students` - Create new student

### Course Management
- `GET /api/courses` - Get all courses
- `GET /api/courses/{id}` - Get course by ID
- `POST /api/courses` - Create new course

### Enrollment
- `POST /api/enrollments` - Enroll student in course
- `DELETE /api/enrollments/{studentId}/{courseId}` - Unenroll student from course

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later

### Build and Run

```bash
# Clone the repository
git clone https://github.com/MatousKratochvil/UniversityAgent.git
cd UniversityAgent

# Build the solution
dotnet build

# Run the Web API
cd src/Presentation/UniversityAgent.Presentation.Web
dotnet run
```

The API will be available at `http://localhost:5107` (or the port shown in console output).

### Sample Data
The application initializes with sample data:
- 3 students (Jan Novák, Anna Svobodová, Petr Dvořák)
- 3 courses (CS101, MATH201, DB301)

### Example Usage

```bash
# Get all students
curl http://localhost:5107/api/students

# Get all courses
curl http://localhost:5107/api/courses

# Enroll a student in a course
curl -X POST http://localhost:5107/api/enrollments \
  -H "Content-Type: application/json" \
  -d '{"studentId": "STUDENT_GUID", "courseId": "COURSE_GUID"}'

# Unenroll a student from a course
curl -X DELETE http://localhost:5107/api/enrollments/STUDENT_GUID/COURSE_GUID
```

## Technology Stack

- **Framework**: .NET 9.0
- **Web API**: ASP.NET Core Minimal APIs
- **Architecture**: Feature-based modular design
- **Storage**: In-memory (ConcurrentDictionary for thread-safety)
- **API Documentation**: OpenAPI/Swagger (available in Development mode)

## Future Enhancements

- Add Teacher management feature
- Implement persistent storage (Entity Framework Core)
- Add authentication and authorization
- Create Blazor Web UI
- Add MAUI mobile application
- Implement event sourcing for audit trail
- Add unit and integration tests