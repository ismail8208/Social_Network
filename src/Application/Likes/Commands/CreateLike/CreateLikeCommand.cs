using System.Data;
using MediaLink.Application.Common.Interfaces;
using MediaLink.Application.Common.Security;
using MediaLink.Application.Notification;
using MediaLink.Domain.Entities;
using MediaLink.Domain.Events.LikeEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediaLink.Application.Likes.Commands.CreateLike;
[Authorize(Roles = "member")]
[Authorize(Roles = "Administrator")]

public record CreateLikeCommand : IRequest<int>
{
    public int UserId { get; set; }
    public int PostId { get; set; }
}

public class CreateLikeCommandHandler : IRequestHandler<CreateLikeCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IClientNotificationService _clientNotificationService;

    public CreateLikeCommandHandler(IApplicationDbContext context, IClientNotificationService clientNotificationService)
    {
        _context = context;
        _clientNotificationService = clientNotificationService;
    }

    public async Task<int> Handle(CreateLikeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Like
        {
            UserId = request.UserId,
            PostId = request.PostId
        };

        entity.AddDomainEvent(new LikeCreatedEvent(entity));

        _context.Likes.Add(entity);
        
        await _context.SaveChangesAsync(cancellationToken);


        // signalR start
        var user = await _context.InnerUsers.FirstOrDefaultAsync(u => u.Id == request.UserId);
        var postWho = await _context.Posts.Include(u => u.User).FirstOrDefaultAsync(p => p.Id == request.PostId);
        var users = await _context.Likes.Include(u => u.User).Include(p => p.Post).Where(c => (c.PostId == request.PostId && c.UserId != request.UserId) || c.Post.UserId == postWho.Id).Select(u => u.User).ToListAsync();
        foreach (var u in users)
        {
            var notify = new Domain.Entities.Notification
            {
                Content = $".. {user.UserName}({user.FirstName} {user.LastName}) added a like to {postWho.User.FirstName} {postWho.User.FirstName} post",
                DistId = u.Id,
                Image = user.ProfileImage,
            };
            await _context.Notifications.AddAsync(notify);
            await _context.SaveChangesAsync(cancellationToken);
        }
        var not = new ClientNotificationDto
        {
            DistId = postWho.UserId, // مشان اخفاءه 
            Content = $"{user.FirstName} {user.LastName} added a like to {postWho.User.FirstName} {postWho.User.FirstName} post", // اسماعيل اضاف تعليق على مشنور  محمد
            Image = user.ProfileImage, // صورة المعلق
        };
        await _clientNotificationService.SendToAll(not);
        //signalR end


        return entity.Id;
    }

}
