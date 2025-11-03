using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class JobType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Contractor> Contractors { get; set; } = new List<Contractor>();
}
