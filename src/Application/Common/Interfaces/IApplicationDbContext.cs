using MediaLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }
    DbSet<TodoItem> TodoItems { get; }

    DbSet<Address> Addresses { get; }
    DbSet<Comment> Comments { get; }
    DbSet<Education> Educations { get; }
    DbSet<Endorsement> Endorsements { get; }
    DbSet<Experience> Experiences { get; }
    DbSet<Follow> Follows { get; }
    DbSet<Like> Likes { get; }
    DbSet<Post> Posts { get; }
    DbSet<Project> Projects { get; }
    DbSet<Share> Shares { get; }
    DbSet<Skill> Skills { get; }
    DbSet<InnerUser> InnerUsers { get; }
    DbSet<Job> Jobs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
