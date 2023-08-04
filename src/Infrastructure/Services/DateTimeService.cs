using MediaLink.Application.Common.Interfaces;

namespace MediaLink.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
