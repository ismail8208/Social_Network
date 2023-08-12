using MediatR;
using MediaLink.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        var notifications = await _context.Notifications.Where(n => n.DistId == request.userId).ToListAsync();
        if (notifications == null)
        {
            return new List<Domain.Entities.Notification>();
        }
        
        return notifications;
    }
}
