using MediaLink.Application.Common.Interfaces;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events;
using MediatR;

namespace MediaLink.Application.Users.Commands.CreateUserCommand;
public record CreateUserCommand : IRequest<InnerUser>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}


public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, InnerUser>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InnerUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new InnerUser
        {
            UserName = request.UserName,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
        };
        entity.AddDomainEvent(new UserCreatedEvent(entity));

        _context.InnerUsers.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}