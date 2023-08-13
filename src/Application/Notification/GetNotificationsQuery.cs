using MediatR;
using MediaLink.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using MediaLink.Application.Common.Security;
using System.Collections.Generic;
using System.Text.Json;
using System.Runtime.CompilerServices;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Notification;


public record GetNotificationsQuery(int userId) : IRequest<List<NotificationDto>>;

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNotificationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    async Task<List<NotificationDto>> IRequestHandler<GetNotificationsQuery, List<NotificationDto>>.Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications
        .Include(u => u.Dist)
        .Where(n => n.DistId == request.userId && n.Dist.IsDeleted == false)
        .ProjectTo<NotificationDto>(_mapper.ConfigurationProvider).ToListAsync();

        return notifications;
    }
}
