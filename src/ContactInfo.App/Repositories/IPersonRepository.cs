using ContactInfo.App.Models;

namespace ContactInfo.App.Repositories;

public interface IPersonRepository
{
    IList<Person> GetPersonList(int userId);
}