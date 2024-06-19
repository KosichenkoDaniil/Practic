using System;
using System.Collections.Generic;

namespace Practic.Models;

public partial class ForWhat
{
    public int Id { get; set; }

    public string TypeofWork { get; set; } = null!;

    public virtual ICollection<Fabric> Fabrics { get; set; } = new List<Fabric>();
}
