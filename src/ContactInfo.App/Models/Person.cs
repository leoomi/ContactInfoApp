using System.ComponentModel.DataAnnotations.Schema;

namespace ContactInfo.App.Models;

public class Person {
    public int Id { get; set; }
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<Contact>? Contacts { get; set; }
}