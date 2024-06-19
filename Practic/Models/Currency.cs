using System;
using System.Collections.Generic;

namespace Practic.Models;

public partial class Currency
{
    public int Id { get; set; }

    public string NameofCurrency { get; set; } = null!;

    public string CountryofCurrency { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
