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

    public Person? GetPerson(int id)
    {
        return _context.People?.FirstOrDefault(p => p.Id == id);
    }

    public IList<Person> GetPersonList(int userId)
    {
        var people = _context?.People?.Where(p => p.UserId == userId).ToList();;
        return people ?? new List<Person>();
    }

    public bool DeletePerson(int id)
    {
        var person = GetPerson(id);
        if (person == null)
        {
            return false;
        }

        _context.People?.Remove(person);
        return true;
    }
}