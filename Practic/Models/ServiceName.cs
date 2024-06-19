using System;
using System.Collections.Generic;

namespace Practic.Models;

public partial class ServiceName
{
    public int Id { get; set; }

    public string NameofService { get; set; } = null!;

    public string Department { get; set; } = null!;

    public virtual ICollection<Fabric> Fabrics { get; set; } = new List<Fabric>();
}
