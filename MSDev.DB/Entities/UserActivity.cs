using System;

namespace MSDev.DB.Entities
{
    public class UserActivity
    {
        public Guid  UserId{ get; set; }
        public Guid AcitvityId { get; set; }


        public User User { get; set; }
        public Activity Activity { get; set; }
    }
}