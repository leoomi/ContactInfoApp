using ContactInfo.App.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ContactInfo.Tests.Setup;

public class MemorySqLite : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ContactInfoContext _context;
    public MemorySqLite()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        var _contextOptions = new DbContextOptionsBuilder<ContactInfoContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        _context = new ContactInfoContext(_contextOptions);
        _context.Database.Migrate();
    }

    public ContactInfoContext GetContext()
    {
        return _context;
    }

    public void Dispose()
    {
        _connection.Dispose();
        _context.Dispose();
    }
}