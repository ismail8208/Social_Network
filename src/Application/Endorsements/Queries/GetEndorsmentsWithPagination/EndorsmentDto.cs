using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Educations.Queries;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Endorsements.Queries.GetEndorsmentsWithPagination;

public class EndorsmentDto : IMapFrom<Endorsement>
{
    public int Id { get; set; }
    public int SkillId { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Endorsement, EndorsmentDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName));
    }
}
