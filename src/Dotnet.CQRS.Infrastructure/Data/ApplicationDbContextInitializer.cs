using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dotnet.CQRS.Infrastructure.Data;

public class ApplicationDbContextInitializer(
    ApplicationDbContext context,
    ILogger<ApplicationDbContextInitializer> logger)
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<ApplicationDbContextInitializer> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
            new("John", "Smith", "john.smith@example.com") { Id = 1 },
            new("Jane", "Doe", "jane.doe@example.com") { Id = 2 },
            new("Alice", "Johnson", "alice.johnson@example.com") { Id = 3 },
            new("Bob", "Wilson", "bob.wilson@example.com") { Id = 4 },
            new("Charlie", "Brown", "charlie.brown@example.com") { Id = 5 }
        };

        _context.Employees.AddRange(employees);
        await _context.SaveChangesAsync();
    }
}
