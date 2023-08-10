using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Users.Commands.UpdateUserCommand;
public record UpdateUserCommand : IRequest<InnerUser>
{
    public string? Username { get; set; }
    public string? Summary { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? specialization { get; set; }
    public string? ProfileImage { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, InnerUser>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InnerUser> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.InnerUsers.FirstOrDefaultAsync(u => u.UserName == request.Username && u.IsDeleted == false);

        if (user == null)
        {
            throw new NotFoundException();
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.ProfileImage = request.ProfileImage;
        user.Summary= request.Summary;
        user.specialization = request.specialization;


        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }
}
