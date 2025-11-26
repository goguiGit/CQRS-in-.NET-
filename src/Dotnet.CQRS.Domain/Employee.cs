namespace Dotnet.CQRS.Domain;

public class Employee : EntityBase
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }

    private Employee() { } // EF Core or serialization

    public Employee(string firstName, string lastName, string email, DateTime hireDate, decimal salary)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        if (salary <= 0)
            throw new ArgumentException("Salary must be greater than zero.");

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        HireDate = hireDate;
        Salary = salary;
        IsActive = true;
    }

    public void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentException("Email cannot be empty.");

        Email = newEmail;
    }

    public void UpdateSalary(decimal newSalary)
    {
        if (newSalary <= 0)
            throw new ArgumentException("Salary must be greater than zero.");

        Salary = newSalary;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public string FullName => $"{FirstName} {LastName}";
}
