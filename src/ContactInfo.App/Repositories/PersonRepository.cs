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

    public Person CreatePerson(Person person)
    {
        _context.Add(person);
        _context.SaveChanges();

        return person;
    }

    public Person SavePerson(Person person)
    {
        _context.Update(person);
        _context.SaveChanges();

        return person;
    }

    public IList<Person> GetPersonList(int userId)
    {
        var people = _context?.People?.Where(p => p.UserId == userId).ToList();;
        return people ?? new List<Person>();
    }
}