namespace BL.Models;

public partial class JobApplication
{
    public int Id { get; set; }

    public int JobPostId { get; set; }

    public int PersonId { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual JobPost JobPost { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
