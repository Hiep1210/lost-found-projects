using System;
using System.Collections.Generic;

namespace _2._LostFoundProj.Models
{
    public partial class Imag
    {
        public int Id { get; set; }
        public string? ImgName { get; set; }
        public int? PostId { get; set; }

        public virtual Pending? Post { get; set; }
    }
}
