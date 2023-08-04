using System.Reflection;
using Duende.IdentityServer.EntityFramework.Options;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Domain.Entities;
using MediaLink.Infrastructure.Identity;
using MediaLink.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MediaLink.Infrastructure.Persistence;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options, operationalStoreOptions)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<Endorsement> Endorsements => Set<Endorsement>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Follow> Follows => Set<Follow>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Share> Shares => Set<Share>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<InnerUser> InnerUsers => Set<InnerUser>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<Follow>()
                .HasKey(f => new { f.FollowerID, f.FollowingID });

        builder.Entity<InnerUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

        builder.Entity<InnerUser>()
               .HasIndex(u => u.Email)
               .IsUnique();

        builder.Entity<ApplicationUser>()
               .HasIndex(u => u.Email)
               .IsUnique();

        builder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany(u => u.Followings)
            .HasForeignKey(f => f.FollowerID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Follow>()
            .HasOne(f => f.Followee)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowingID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Share>()
            .HasOne(s => s.User)
            .WithMany(u => u.SharedPosts)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Share>()
            .HasOne(s => s.Post)
            .WithMany(p => p.Shares)
            .HasForeignKey(s => s.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Project>()
            .HasOne<InnerUser>(u => u.User)
            .WithMany(p => p.Projects)
            .HasForeignKey(p => p.UserId) //..
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<InnerUser>()
            .HasMany<Project>(u => u.Projects)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId) //..
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Skill>()
            .HasOne<InnerUser>(u => u.User)
            .WithMany(s => s.Skills)
            .HasForeignKey(u => u.UserId) //..
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Education>()
            .HasOne<InnerUser>(u => u.User)
            .WithMany(p => p.Educations)
            .HasForeignKey(s => s.UserId) //..
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Experience>()
            .HasOne<InnerUser>(u => u.User)
            .WithMany(p => p.Experiences)
            .HasForeignKey(p => p.UserId) //..
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Project>()
            .HasOne<InnerUser>(u => u.User)
            .WithMany(p => p.Projects)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Endorsement>()
            .HasOne<InnerUser>(u => u.User)
            .WithMany(p => p.Endorsements)
            .HasForeignKey(p => p.UserId) //..
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Endorsement>()
            .HasOne<Skill>(u => u.Skill)
            .WithMany(s => s.Endorsements)
            .HasForeignKey(u => u.SkillId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<InnerUser>()
            .HasOne<Address>(u => u.Address)
            .WithOne(s => s.User)
            .HasForeignKey<Address>(u => u.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Experience>()
           .HasOne<Project>(p => p.Project)
           .WithOne(p => p.Experience)
           .HasForeignKey<Experience>(u => u.ProjectId);


        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
