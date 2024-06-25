﻿namespace WebApplication1.Models
{
    public class GroupUsers
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public int UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
