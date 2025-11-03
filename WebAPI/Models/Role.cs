using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
