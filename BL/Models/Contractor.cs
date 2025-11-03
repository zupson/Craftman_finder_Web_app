namespace BL.Models;

public partial class Contractor
{
    public int Id { get; set; }

    public int? PersonId { get; set; }

    public string? CompanyName { get; set; }

    public int JobTypeId { get; set; }

    public bool? IsFreelancer { get; set; }

    public bool IsDeleted { get; set; }

    public virtual IList<ContractorLocation> ContractorLocations { get; set; } = new List<ContractorLocation>();

    public virtual JobType JobType { get; set; } = null!;

    public virtual Person? Person { get; set; }
}
