using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class JobPost
{
    public int Id { get; set; }

    public int ContractorLocationId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ContractorLocation ContractorLocation { get; set; } = null!;

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}
