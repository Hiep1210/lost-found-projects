using System;
using System.Collections.Generic;

namespace _2._LostFoundProj.Models
{
    public partial class Pending
    {
        public Pending()
        {
            Imags = new HashSet<Imag>();
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public int? Poster { get; set; }
        public DateTime? Postdate { get; set; }
        public string? Caption { get; set; }
        public bool? Lostorfound { get; set; }
        public bool? Processed { get; set; }

        public virtual Account? PosterNavigation { get; set; }
        public virtual ICollection<Imag> Imags { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
