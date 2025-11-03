namespace BL.Models;

public partial class JobType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<Contractor> Contractors { get; set; } = new List<Contractor>();
}
