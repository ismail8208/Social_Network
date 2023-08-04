using System.ComponentModel.DataAnnotations.Schema;
using MediaLink.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MediaLink.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public bool IsDeleted { get; set; } = false;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public InnerUser? User { get; set; }
}
