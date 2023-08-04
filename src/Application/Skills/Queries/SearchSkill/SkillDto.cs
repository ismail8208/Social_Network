using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.Skills.Queries.SearchSkill;
public class SkillDto : IMapFrom<Skill>
{
    public int Id { get; set; }
    public string? Title { get; set; }
}
