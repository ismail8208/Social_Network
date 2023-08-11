using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLink.Application.Notification;

namespace MediaLink.Application.Common.Interfaces;
public interface IClientNotificationService
{
    public Task SendToAll(ClientNotificationDto clientNotificationDto);
}
