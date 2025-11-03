using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public string? SecurityToken { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Contractor? Contractor { get; set; }

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
