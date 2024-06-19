using System;
using System.Collections.Generic;

namespace Practic.Models;

public partial class Application
{
    public int Id { get; set; }

    public int FabricId { get; set; }

    public string ShortDescription { get; set; } = null!;

    public int FirmId { get; set; }

    public int CurrencyId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual Fabric Fabric { get; set; } = null!;

    public virtual Firm Firm { get; set; } = null!;
}
