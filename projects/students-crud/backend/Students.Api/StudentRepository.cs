public sealed class StudentRepository
{
    private readonly List<Student> _students =
    [
        new(1, "Anna", "Smith", 21, "Computer Science"),
        new(2, "John", "Taylor", 23, "Management"),
        new(3, "Olivia", "Wilson", 20, "Computer Science")
    ];

    private int _nextId = 4;

    public IReadOnlyList<Student> GetAll()
    {
        return _students.OrderBy(student => student.Id).ToList();
    }

    public Student? GetById(int id)
    {
        return _students.FirstOrDefault(student => student.Id == id);
    }

    public Student Add(StudentRequest request)
    {
        var student = new Student(_nextId++, request.FirstName, request.LastName, request.Age, request.Major);
        _students.Add(student);
        return student;
    }

    public Student? Update(int id, StudentRequest request)
    {
        var index = _students.FindIndex(student => student.Id == id);
        if (index == -1)
        {
            return null;
        }

        var student = new Student(id, request.FirstName, request.LastName, request.Age, request.Major);
        _students[index] = student;
        return student;
    }

    public bool Delete(int id)
    {
        return _students.RemoveAll(student => student.Id == id) > 0;
    }
}
