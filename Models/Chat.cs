using System;
using System.Collections.Generic;

namespace _2._LostFoundProj.Models
{
    public partial class Chat
    {
        public Chat()
        {
            Cmessages = new HashSet<Cmessage>();
        }

        public int Id { get; set; }
        public int? User1 { get; set; }
        public int? User2 { get; set; }

        public virtual Account? User1Navigation { get; set; }
        public virtual Account? User2Navigation { get; set; }
        public virtual ICollection<Cmessage> Cmessages { get; set; }
    }
}
