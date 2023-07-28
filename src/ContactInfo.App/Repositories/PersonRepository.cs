using Microsoft.EntityFrameworkCore;
using ContactInfo.App.Models;

namespace ContactInfo.App.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly ContactInfoContext _context;

    public PersonRepository(ContactInfoContext context)
    {
        _context = context;
    }

    public IList<Person> GetPersonList(string username)
    {
        var user = _context?.Users?.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            return new List<Person>();
        }

        return user.People ?? new List<Person>();
    }
}