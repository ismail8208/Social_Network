using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Notification;
using Microsoft.AspNetCore.SignalR;
using WebUI.Hubs;

namespace WebUI.Services;

public class ClientNotificationService : IClientNotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public ClientNotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendToAll(ClientNotificationDto clientNotificationDto)
    {
        await _hubContext.Clients.All.SendAsync("MyNotification", clientNotificationDto);
    }
}
