namespace MediaLink.Domain.Entities;
public class CV
{
    public int CvId { get; set; }
    public int UserId { get; set; }
    public InnerUser? User { get; set; }
    public int Position { get; set; }
    public int Company { get; set; }
}
