namespace Imdb.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Models;
    using Imdb.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Actor> Actors { get; set; }

        public DbSet<Director> Directors { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieActor> MovieActors { get; set; }

        public DbSet<MovieGenre> MovieGenres { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<UserMovie> UserMovies { get; set; }

        public DbSet<MovieImage> MovieImages { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            builder.Entity<Actor>(act =>
            {
                act.HasMany(a => a.Movies)
                    .WithOne(m => m.Actor)
                    .HasForeignKey(m => m.ActorId);
            });

            builder.Entity<Director>(d =>
            {
                d.HasMany(d => d.Movies)
                    .WithOne(m => m.Director)
                    .HasForeignKey(m => m.DirectorId);
            });

            builder.Entity<Genre>(g =>
            {
                g.HasMany(gn => gn.Movies)
                    .WithOne(m => m.Genre)
                    .HasForeignKey(m => m.GenreId);
            });

            builder.Entity<Language>(l =>
            {
                l.HasMany(l => l.Movies)
                    .WithOne(m => m.Language)
                    .HasForeignKey(m => m.LanguageId);
            });

            builder.Entity<Movie>(m =>
            {
                m.HasMany(mo => mo.Actors)
                    .WithOne(a => a.Movie)
                    .HasForeignKey(a => a.MovieId);

                m.HasMany(mo => mo.Genres)
                    .WithOne(g => g.Movie)
                    .HasForeignKey(g => g.MovieId);

                m.HasMany(mo => mo.Reviews)
                    .WithOne(r => r.Movie)
                    .HasForeignKey(r => r.MovieId);
            });

            builder.Entity<Review>(r =>
            {
                r.HasOne(re => re.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(re => re.UserId);

                r.HasOne(re => re.Movie)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(re => re.MovieId);
            });

            builder.Entity<UserMovie>(um =>
            {
                um.HasKey(u => new { u.UserId, u.MovieId });

                um.HasOne(ums => ums.User)
                    .WithMany(u => u.WatchList)
                    .HasForeignKey(ums => ums.UserId);

                um.HasOne(ums => ums.Movie)
                    .WithMany(m => m.UsersWatchlists)
                    .HasForeignKey(ums => ums.MovieId);
            });

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
