using System;
using System.Collections.Generic;

namespace _2._LostFoundProj.Models
{
    public partial class Account
    {
        public Account()
        {
            ChatUser1Navigations = new HashSet<Chat>();
            ChatUser2Navigations = new HashSet<Chat>();
            Cmessages = new HashSet<Cmessage>();
            Comments = new HashSet<Comment>();
            Pendings = new HashSet<Pending>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Pass { get; set; }
        public string? Phone { get; set; }
        public DateTime? Dob { get; set; }

        public virtual ICollection<Chat> ChatUser1Navigations { get; set; }
        public virtual ICollection<Chat> ChatUser2Navigations { get; set; }
        public virtual ICollection<Cmessage> Cmessages { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Pending> Pendings { get; set; }
    }
}
