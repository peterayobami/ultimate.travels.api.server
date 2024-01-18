using Microsoft.EntityFrameworkCore;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The database representational model for the application
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options">The DbContext options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        #endregion

        #region Db Sets

        /// <summary>
        /// The flight request database table
        /// </summary>
        public DbSet<FlightRequestsDataModel> FlightRequests { get; set; }

        /// <summary>
        /// The flight bookings database table
        /// </summary>
        public DbSet<FlightBookingDataModel> FlightBookings { get; set; }

        /// <summary>
        /// The flight offers database table
        /// </summary>
        public DbSet<FlightOfferDataModel> FlightOffers { get; set; }

        /// <summary>
        /// The customers database table
        /// </summary>
        public DbSet<CustomersDataModel> Customers { get; set; }

        /// <summary>
        /// The travelers database table
        /// </summary>
        public DbSet<TravelerDataModel> Travelers { get; set; }

        /// <summary>
        /// The payments database table
        /// </summary>
        public DbSet<PaymentsDataModel> Payments { get; set; }

        /// <summary>
        /// The amadeus credentials database table
        /// </summary>
        public DbSet<AmadeusCredentialsDataModel> AmadeusCredentials { get; set; }

        #endregion

        public override int SaveChanges()
        {
            // For each entry...
            foreach (var entry in ChangeTracker.Entries<BaseDataModel>())
            {
                // If data is being created...
                if (entry.State == EntityState.Added)
                {
                    // Set date created
                    entry.Entity.DateCreated = DateTime.UtcNow;

                    // Set date modified
                    entry.Entity.DateModified = DateTime.UtcNow;
                }

                // If data is being modified...
                if (entry.State == EntityState.Modified)
                {
                    // Set date modified
                    entry.Entity.DateModified = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // For each entry...
            foreach (var entry in ChangeTracker.Entries<BaseDataModel>())
            {
                // If data is being created...
                if (entry.State == EntityState.Added)
                {
                    // Set date created
                    entry.Entity.DateCreated = DateTime.UtcNow;

                    // Set date modified
                    entry.Entity.DateModified = DateTime.UtcNow;
                }

                // If data is being modified...
                if (entry.State == EntityState.Modified)
                {
                    // Set date modified
                    entry.Entity.DateModified = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
