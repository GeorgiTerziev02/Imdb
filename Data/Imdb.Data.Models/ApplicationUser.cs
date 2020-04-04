namespace Imdb.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Imdb.Data.Common.Models;
    using Imdb.Data.Models.Enumerations;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.MovieWatchList = new HashSet<UserMovie>();
            this.TvShowsWatchList = new HashSet<UserTvShow>();
            this.Reviews = new HashSet<Review>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public virtual IEnumerable<UserMovie> MovieWatchList { get; set; }

        public virtual IEnumerable<UserTvShow> TvShowsWatchList { get; set; }

        public virtual IEnumerable<Review> Reviews { get; set; }
    }
}
