using System.ComponentModel.DataAnnotations.Schema;

namespace ContactInfo.App.Models;

public class Contact
{
    public int? Id { get; set; }
    [ForeignKey(nameof(Person))]
    public int? PersonId { get; set; }
    public ContactType Type { get; set; }
    public string? Info { get; set; }
}