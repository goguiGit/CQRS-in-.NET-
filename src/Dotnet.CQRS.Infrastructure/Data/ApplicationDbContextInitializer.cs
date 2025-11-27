using Dotnet.CQRS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dotnet.CQRS.Infrastructure.Data;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitializer> _logger;

    public ApplicationDbContextInitializer(
        ApplicationDbContext context,
        ILogger<ApplicationDbContextInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Ensure database is created
            await _context.Database.EnsureCreatedAsync();
            _logger.LogInformation("Database initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            _logger.LogInformation("Database seeded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Check if data already exists
        if (await _context.Employees.AnyAsync())
        {
            _logger.LogInformation("Database already contains seed data");
            return;
        }

        // Seed employees
        var employees = new List<Employee>
        {
            new Employee("John", "Smith", "john.smith@example.com") { Id = 1 },
            new Employee("Jane", "Doe", "jane.doe@example.com") { Id = 2 },
            new Employee("Alice", "Johnson", "alice.johnson@example.com") { Id = 3 },
            new Employee("Bob", "Wilson", "bob.wilson@example.com") { Id = 4 },
            new Employee("Charlie", "Brown", "charlie.brown@example.com") { Id = 5 }
        };

        _context.Employees.AddRange(employees);
        await _context.SaveChangesAsync();
    }
}
