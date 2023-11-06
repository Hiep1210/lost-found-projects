using System;
using System.Collections.Generic;

namespace _2._LostFoundProj.Models
{
    public partial class Img
    {
        public Img()
        {
            Pendings = new HashSet<Pending>();
        }

        public int Id { get; set; }
        public string? ImgName { get; set; }

        public virtual ICollection<Pending> Pendings { get; set; }
    }
}
