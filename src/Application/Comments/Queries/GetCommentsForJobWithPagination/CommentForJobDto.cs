﻿using System.Data;
using AutoMapper;
using MediaLink.Application.Common.Mappings;
using MediaLink.Application.Common.Security;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Comments.Queries.GetCommentsWithPagination;
[Authorize(Roles = "member")]
[Authorize(Roles = "Administrator")]
public class CommentForJobDto : IMapFrom<Comment>
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public int? JobId { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Comment, CommentForJobDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName));
    }
}
