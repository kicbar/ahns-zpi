public sealed record Student(int Id, string FirstName, string LastName, int Age, string Major);

public sealed record StudentRequest(string FirstName, string LastName, int Age, string Major);
