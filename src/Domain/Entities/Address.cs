namespace MediaLink.Domain.Entities;

public class Address : BaseEntity
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public int UserId { get; set; }
    public InnerUser? User { get; set; }
}

