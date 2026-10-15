using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomType { get; set; } = null!;

    public decimal Price { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
