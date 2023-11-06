using System;
using System.Collections.Generic;

namespace _2._LostFoundProj.Models
{
    public partial class Cmessage
    {
        public int Id { get; set; }
        public string? Mess { get; set; }
        public int? SenderId { get; set; }
        public DateTime? Sdate { get; set; }
        public int? RoomId { get; set; }

        public virtual Chat? Room { get; set; }
        public virtual Account? Sender { get; set; }
    }
}
