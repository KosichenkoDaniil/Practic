using System;
using System.Collections.Generic;

namespace Practic.Models;

public partial class Fabric
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int SetviceNameId { get; set; }

    public int ForWhatId { get; set; }

    public string CodeTnved { get; set; } = null!;

    public string CodeOkrb { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ForWhat ForWhat { get; set; } = null!;

    public virtual ServiceName SetviceName { get; set; } = null!;
}
