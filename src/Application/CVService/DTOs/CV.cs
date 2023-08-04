namespace MediaLink.Application.CVService.DTOs;
public class CV
{
    public UserCV? User { get; set; }
    public List<SkillCV>? SkillCVs { get; set; }
    public List<ExperienceCV>? ExperienceCVs { get; set; }
    public List<ProjectCV>? ProjectCVs { get; set; }
    public List<EducationCV>? EducationCVs { get; set; }

    public CV(UserCV? user, List<SkillCV>? skillCVs, List<ExperienceCV>? experienceCVs, List<ProjectCV>? projectCVs, List<EducationCV>? educationCVs)
    {
        User = user;
        SkillCVs = skillCVs;
        ExperienceCVs = experienceCVs;
        ProjectCVs = projectCVs;
        EducationCVs = educationCVs;
    }
}
