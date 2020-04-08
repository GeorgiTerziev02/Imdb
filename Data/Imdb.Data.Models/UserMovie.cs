﻿// ReSharper disable VirtualMemberCallInConstructor
namespace Imdb.Data.Models
{
    public class UserMovie
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
