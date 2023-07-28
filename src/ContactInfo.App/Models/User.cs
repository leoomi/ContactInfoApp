using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContactInfo.App.Models;

[Index(nameof(Username), IsUnique = true)]
public class User {
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public List<Person>? People { get; set; }

}