using System;
using System.Collections.Generic;

namespace _2._LostFoundProj.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public int? PostId { get; set; }

        public virtual Pending? PostNavigation { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
