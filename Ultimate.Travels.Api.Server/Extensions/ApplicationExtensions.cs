using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// Extension methods for DI services configuration
    /// </summary>
    public static class ApplicationExtensions
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder builder)
        {
            // Create a scoped service
            using var scope = builder.ApplicationServices.CreateScope();

            // Get the scoped instance of the ApplicationDbContext
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            // Create migrate the database
            dbContext.Database.Migrate();

            // Return builder for further chaining
            return builder;
        }

        /// <summary>
        /// Registers specified recurring jobs
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/></param>
        /// <returns></returns>
        public static IApplicationBuilder RegisterRecurringJobs(this IApplicationBuilder builder)
        {
            // Create service scope
            using var scope = builder.ApplicationServices.CreateScope();

            // Get scoped instance of RecurringJobs
            var recurringJobs = scope.ServiceProvider.GetService<RecurringJobs>();

            // Register Amadeus authorization service
            recurringJobs.RegisterAmadeusAuthorization();

            // Register Flight Requests Manager
            recurringJobs.RegisterFlightRequestsManager();

            // Register Flight Offers Manager
            recurringJobs.RegisterFlightOffersManager();

            // Return builder for further chaining
            return builder;
        }

        /// <summary>
        /// Configures the database context for the application
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="configuration">The <see cref="IConfiguration"/></param>
        /// <returns>Return the <see cref="IServiceCollection"/> for further chaining</returns>
        //public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        //{
        //    // Add ApplicationDbContext to DI container
        //    services.AddDbContext<ApplicationDbContext>(options =>
        //        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
        //        sqlOptions => sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

        //    // Return services for further chaining
        //    return services;
        //}

        /// <summary>
        /// Configures the database context for the application
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="configuration">The <see cref="IConfiguration"/></param>
        /// <returns>Return the <see cref="IServiceCollection"/> for further chaining</returns>
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Add ApplicationDbContext to DI container
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Registers the application's domain services to the DI container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ShoppingOperations>()
                .AddScoped<BookingOperations>();

            // Return services
            return services;
        }

        /// <summary>
        /// Configures hangfire for PostgreSql
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="configuration">The <see cref="IConfiguration"/></param>
        /// <returns></returns>
        //public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
        //{
        //    // Configure hangfire
        //    services.AddHangfire(config => config
        //        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        //        .UseSimpleAssemblyNameTypeSerializer()
        //        .UseRecommendedSerializerSettings()
        //        .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(configuration.GetConnectionString("HangfireConnection"))));

        //    // Add Hangfire server
        //    services.AddHangfireServer();

        //    // Return services for further chaining
        //    return services;
        //}

        /// <summary>
        /// Configures hangfire for PostgreSql
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="configuration">The <see cref="IConfiguration"/></param>
        /// <returns></returns>
        public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure hangfire
            services.AddHangfire(config => config
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            // Add Hangfire server
            services.AddHangfireServer();

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Registers Amadeus service to DI
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <returns></returns>
        public static IServiceCollection AddAmadeusService(this IServiceCollection services)
        {
            // Add Amadeus client to DI
            services.AddScoped<AmadeusClient>();

            // Add Amadeus services to DI
            services.AddScoped(provider => new AmadeusService(provider.GetService<AmadeusClient>()));

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Registers the <see cref="AmadeusAuthorizationService"/> to DI
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <returns></returns>
        public static IServiceCollection AddAmadeusAuthorizationService(this IServiceCollection services)
        {
            // Add Amadeus authorization service to DI
            services.AddScoped<AmadeusAuthorizationService>();

            // Return services
            return services;
        }

        /// <summary>
        /// Registers <see cref="BackgroundServices"/> to the DI container
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance</param>
        /// <returns>Returns <see cref="IServiceCollection"/> for further chaining</returns>
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            // Add the background service
            services.AddScoped<BackgroundServices>();

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Registers RecurringJobs to DI
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <returns>Returns <see cref="IServiceCollection"/> for further chaining</returns>
        public static IServiceCollection AddRecurringJobService(this IServiceCollection services)
        {
            // Add RecurringJobs to DI
            services.AddScoped<RecurringJobs>();

            // Return services for chaining
            return services;
        }

        /// <summary>
        /// Registers a singleton instance of the <see cref="AmadeusConfiguration"/>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="configuration">The <see cref="IConfiguration"/></param>
        /// <returns></returns>
        public static IServiceCollection AddAmadeusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Register singleton instance of Amadeus configuration
            services.AddSingleton(configuration.GetSection("Amadeus").Get<AmadeusConfiguration>());

            // Return services for further chaining
            return services;
        }
    }
}