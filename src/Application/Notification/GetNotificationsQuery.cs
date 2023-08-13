using MediatR;
using MediaLink.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using MediaLink.Application.Common.Security;

namespace MediaLink.Application.Notification;


public record GetNotificationsQuery(int userId): IRequest<List<Domain.Entities.Notification>>;

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<Domain.Entities.Notification>>
{
    private readonly IApplicationDbContext _context;

    public GetNotificationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Domain.Entities.Notification>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications.Include(u => u.Dist).Where(n => n.DistId == request.userId && n.Dist.IsDeleted == false).ToListAsync();
        if (notifications == null)
        {
            return new List<Domain.Entities.Notification>();
        }
        
        return notifications;
    }
}
