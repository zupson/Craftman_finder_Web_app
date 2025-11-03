using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class Location
{
    public int Id { get; set; }

    public string PostalCode { get; set; } = null!;

    public int TownId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ContractorLocation> ContractorLocations { get; set; } = new List<ContractorLocation>();

    public virtual Town Town { get; set; } = null!;
}
