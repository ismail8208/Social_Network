using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLink.Domain.Entities;
public class Notification : BaseAuditableEntity
{
    public int DistId { get; set; }
    public InnerUser? Dist { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
}
