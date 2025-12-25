using System;
using System.Collections.Generic;

namespace ShopDbLibrary.Models;

public partial class Shoe
{
    public int ShoeId { get; set; }

    public int SupplierId { get; set; }

    public int CategoryId { get; set; }

    public int ManufacturerId { get; set; }

    public string Article { get; set; } = null!;

    public int Price { get; set; }

    public byte Discount { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public byte? Size { get; set; }

    public string? Color { get; set; }

    public string Gender { get; set; } = null!;

    public string? Photo { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<ShoeOrder> ShoeOrders { get; set; } = new List<ShoeOrder>();

    public virtual Supplier Supplier { get; set; } = null!;
}
