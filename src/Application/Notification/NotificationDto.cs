using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Posts.Queries;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Notification;
public class NotificationDto : IMapFrom<Domain.Entities.Notification>
{
    public int DistId { get; set; }
    public InnerUser? Dist { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
    public DateTime Created { get; set; }
    public int UserId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Notification, NotificationDto>()
            .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Dist.Id));
    }
}
