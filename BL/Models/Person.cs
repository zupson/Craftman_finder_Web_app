namespace BL.Models;

public partial class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Contractor? Contractor { get; set; }

    public virtual IList<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    public virtual IList<Role> Roles { get; set; } = new List<Role>();
}
