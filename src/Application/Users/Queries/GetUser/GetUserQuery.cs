using AutoMapper;
using MediaLink.Application.Common.Exceptions;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Users.Queries.FindUser;
using MediaLink.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Users.Queries.GetUser;

public record GetUserQuery(string username) : IRequest<UserDto>;
public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;
    private readonly IIdentityService _identityService;

    public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService userService, IIdentityService identityService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
        _identityService = identityService;
    }
    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.InnerUsers.Include(f => f.Followers).Include(f => f.Followings).Include(p =>p.Posts).FirstOrDefaultAsync(u => u.UserName == request.username && u.IsDeleted == false);
        if (user == null)
        {
            throw new NotFoundException(nameof(InnerUser), request.username);
        }

        var role = await _identityService.GetUserRole(_userService.UserId!);
        var entity = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfileImage = user.ProfileImage,
            Role = role,
            Summary = user.Summary,
            NumberOfFollowers = user.Followings!.Count(),
            NumberOfFollowings = user.Followers!.Count(),
            NumberOfPosts = user.Posts!.Count(),
            specialization = user.specialization,
        };

        return entity;

    }
}