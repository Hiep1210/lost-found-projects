using System;
using System.Collections.Generic;

namespace _2._LostFoundProj.Models
{
    public partial class FoundItem
    {
        public FoundItem()
        {
            Pendings = new HashSet<Pending>();
        }

        public int Id { get; set; }
        public int? ItemType { get; set; }
        public string? Descrip { get; set; }
        public string? Location { get; set; }

        public virtual ItemType? ItemTypeNavigation { get; set; }
        public virtual ICollection<Pending> Pendings { get; set; }
    }
}
