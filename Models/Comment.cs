using System;
using System.Collections.Generic;

namespace _2._LostFoundProj.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string? Com { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }

        public virtual Post? Post { get; set; }
        public virtual Account? User { get; set; }
    }
}
