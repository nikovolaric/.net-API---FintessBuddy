using Microsoft.EntityFrameworkCore;

namespace fitnessBudyApi.Models;

[Index(nameof(username), IsUnique = true)]
public class User
{
    public Guid id { get; set; }

    public required string username { get; set; }

    public string? password { get; set; }
    public Role role { get; set; }
}

public enum Role
{
    user = 0,
    admin = 1,
}
