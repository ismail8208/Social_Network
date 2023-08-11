using Microsoft.AspNetCore.SignalR;

namespace WebUI.Hubs;

public class NotificationHub : Hub
{
    public NotificationHub()
    {

    }
    public static int TotalViews { get;  set; } = 0;
    public async Task NewWindowLoaded()
    {
        TotalViews++;
        await Clients.All.SendAsync("updateTotalViews", TotalViews);
    }

}
