using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Town> Towns { get; set; } = new List<Town>();
}
