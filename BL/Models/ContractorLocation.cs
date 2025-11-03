namespace BL.Models;

public partial class ContractorLocation
{
    public int Id { get; set; }

    public int ContractorId { get; set; }

    public int LocationId { get; set; }

    public virtual Contractor Contractor { get; set; } = null!;

    public virtual ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();

    public virtual Location Location { get; set; } = null!;
}
