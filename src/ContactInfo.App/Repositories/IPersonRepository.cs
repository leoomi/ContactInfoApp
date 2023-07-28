using ContactInfo.App.Models;

namespace ContactInfo.App.Repositories;

public interface IPersonRepository
{
    Person CreatePerson(Person person);
    Person SavePerson(Person person);
    Person? GetPerson(int personId);
    IList<Person> GetPersonList(int userId);
    bool DeletePerson(int id);
}