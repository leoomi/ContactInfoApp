using ContactInfo.App.Models;

namespace ContactInfo.App.Repositories;

public interface IPersonRepository
{
    Person CreatePerson(Person person);
    Person SavePerson(Person person);
    Person? GetPerson(int id);
    IList<Person> GetPersonList(int userId);
    Person? GetPersonWithContacts(int id);
    bool DeletePerson(int id);
}