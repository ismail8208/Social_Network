using MediaLink.Application.Common.Interfaces;
using MediaLink.Domain.Entities;
using MediatR;

namespace MediaLink.Application.CVService.ReceiveCVs;
public record ReceiveCV : IRequest<int>
{
    public int UserId { get; set; }
    public int CompanyId { get; set; }
    public int JobId { get; set; }
}
public class ReceiveCVHandler : IRequestHandler<ReceiveCV, int>
{
    private readonly IApplicationDbContext _context;

    public ReceiveCVHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<int> Handle(ReceiveCV request, CancellationToken cancellationToken)
    {
        var CvForAdded = new CV
        {
            Company = request.CompanyId,
            Position = request.JobId,
            UserId = request.UserId,
        };
        await _context.CVs.AddAsync(CvForAdded);
        await _context.SaveChangesAsync(cancellationToken);
        return CvForAdded.CvId;
    }
}
