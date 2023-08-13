using System.Data;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Application.Follows.Queries.GetFollowers;
using MediaLink.Application.Notification;
using MediaLink.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Jobs.Commands.CreateJob;
[Authorize(Roles = "company")]
[Authorize(Roles = "Administrator")]

public record CreateJobCommand : IRequest<int>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int UserId { get; set; }
}

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IClientNotificationService _clientNotificationService;
    private readonly IMediator _mediator;

    public CreateJobCommandHandler(IApplicationDbContext context, IClientNotificationService clientNotificationService, IMediator mediator)
    {
        _context = context;
        _clientNotificationService = clientNotificationService;
        _mediator = mediator;
    }
    public async Task<int> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var entity = new Job
        {
            Title = request.Title,
            Description = request.Description,
            UserId = request.UserId,
        };

        _context.Jobs.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);



        // signalR start
        var user = await _context.InnerUsers.FirstOrDefaultAsync(u => u.Id == request.UserId); // مشان اسم الشركة
        var commad = new GetFollowersWithPaginationQuery
        {
            UserId= request.UserId,
            PageNumber = 1,
            PageSize = 100,
        };
        var users = await _mediator.Send(commad); // المستخدمين الي متابعين الشركة
        foreach (var u in users.Items)
        {
            var notify = new Domain.Entities.Notification
            {
                Content = $".. {user.UserName}({user.FirstName} {user.LastName}) has announced a job vacancy titled {request.Title}", // قامت شركة ما بالاعلان عن شاغر وظيفي بعنوان كذا
                DistId = u.Id,
                Image = user.ProfileImage,
            };
            await _context.Notifications.AddAsync(notify);
            await _context.SaveChangesAsync(cancellationToken);
        }
        var not = new ClientNotificationDto
        {
            DistId =request.UserId, // مشان اخفاءه 
            Content = $"{user.FirstName} {user.LastName} has announced a job vacancy titled {request.Title}", // اسماعيل اضاف تعليق على مشنور  محمد
            Image = user.ProfileImage, // لوغو الشركة
        };
        await _clientNotificationService.SendToAll(not);
        //signalR end


        return entity.Id;
    }
}
