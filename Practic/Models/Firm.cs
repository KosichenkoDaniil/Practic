using System;
using System.Collections.Generic;

namespace Practic.Models;

public partial class Firm
{
    public int Id { get; set; }

    public string СountryofFirm { get; set; } = null!;

    public string NameofFirm { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
