using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Location
{
    public int Id { get; set; }

    public int PostalCode { get; set; }

    public int TownId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<ContractorLocation> ContractorLocations { get; set; } = new List<ContractorLocation>();

    public virtual Town Town { get; set; } = null!;
}
