using ContactInfo.App.Models;
using Microsoft.EntityFrameworkCore;

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

    public Person? GetPerson(int id)
    {
        return _context.People?.FirstOrDefault(p => p.Id == id);
    }

    public IList<Person> GetPersonList(int userId)
    {
        var people = _context?.People?.Where(p => p.UserId == userId).ToList();;
        return people ?? new List<Person>();
    }

    public Person? GetPersonWithContacts(int id)
    {
        return _context.People?
            .Where(p => p.Id == id)
            .Include(p => p.Contacts)
            .FirstOrDefault();
    }

    public bool DeletePerson(int id)
    {
        var person = GetPerson(id);
        if (person == null)
        {
            return false;
        }

        _context.People?.Remove(person);
        _context.SaveChanges();
        return true;
    }
}