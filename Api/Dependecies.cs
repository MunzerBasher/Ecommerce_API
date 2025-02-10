using Microsoft.Extensions.Options;

namespace Api
{
    public static class Dependecies
    {

        public static IServiceCollection AddDependecies(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddHangfire(conf =>
            {

                conf.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSqlServerStorage(Configuration.GetConnectionString("BackgroundJobs"));
                
                
                

            });
            Services.AddControllers();
            Services.AddEndpointsApiExplorer();
            Services.InjectServices();
            Services.AddJwtDependecies(Configuration);
            Services.AddSwaggerGenDependecies();
            Services.AddRateLimitingConfig();
            Services.AddEfCoreConfiguration(Configuration);
            Services.AddHealthChecks().
                AddDbContextCheck<ApplicationDbContext>().
                AddHangfire(options => 
                {
                    options.MinimumAvailableServers = 1;

                });
            return Services;
        }

        public static IServiceCollection AddEfCoreConfiguration(this IServiceCollection Services, IConfiguration Configuration)
        {
            var ConnectionStrings = Configuration.GetConnectionString("DefualtConnection") ?? throw new InvalidOperationException("your ConnectionString is invalid ");
            Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionStrings));
            return Services;
        }


        private static IServiceCollection InjectServices(this IServiceCollection Services)
        {

            Services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            Services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            Services.AddProblemDetails();
            Services.AddExceptionHandler<ExceptionHandle>();
            Services.AddSingleton<ConnectionString>();
            Services.AddDistributedMemoryCache();
            Services.AddScoped<ICacheServices, CacheServices>();
            Services.AddScoped<IPaymentsServices, PaymentsServices>();
            Services.AddScoped<INotificationsServices, NotificationsServices>();
            Services.AddScoped<IShareItemsOrdersServices, ShareItemsOrdersServices>();
            Services.AddScoped<IReviewsRopesitry, ReviewsRopesitry>();
            Services.AddScoped<IReviewsServices, ReviewsServices>();
            Services.AddScoped<IShareItemsOrdersRopesitry, ShareItemsOrdersRopesitry>();
            Services.AddScoped<IAddressRopesitry, AddressRopesitry>();
            Services.AddScoped<IAddressServices, AddressServices>();
            Services.AddScoped<IFavorites, FavoriteServices>();
            Services.AddScoped<IEmailService, EmailService>();
            Services.AddScoped<IFavoritesRopesitry, FavoritesDataAccess>();
            Services.AddScoped<IProductRopesitry, ProductRopesitry>();
            Services.AddScoped<IProductServices, ProductServices>();
            Services.AddScoped<ICategoriesRopesitry, CategoriesData>();
            Services.AddScoped<ICategoriesServices, ProductCategoryLogic>();
            Services.AddScoped<IOrdersRepositry, OrdersRepositry>();
            Services.AddScoped<IOrdersServices, OrderServices>();
            Services.AddScoped<IOrderItemsRopesitry, OrderItemRopesitry>();
            Services.AddScoped<IItemsServices, ItemsServices>();
            Services.AddScoped<IShippingServices, ShippingServices>();
            Services.AddScoped<IShippingRopesitry, ShippingRopesitry>();
            Services.AddScoped<IJwtToken, JwtToken>();
            Services.AddScoped<IFileServices, FileServices>();
            Services.AddScoped<IRoleService, RoleService>();
            Services.AddScoped<IProductImagesRopesitry, ProductImagesRopesitry>();
            Services.AddScoped<IProductImagesServices, ProductImagesServices>();
            Services.AddScoped<IAccountService, AccountService>();
            Services.AddScoped<IAuthService, AuthService>();
            Services.AddIdentity<UserIdentity, UserRoles>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            return Services;
        }

        private static IServiceCollection AddJwtDependecies(this IServiceCollection Services, IConfiguration Configuration)
        {
            var jwt = Configuration.GetSection("Jwt").Get<Jwt>();
            Services.Configure<Jwt>(Configuration.GetSection(nameof(Jwt)));
            Services.Configure<MailSettings>(Configuration.GetSection(nameof(MailSettings)));
            Services.AddScoped(typeof(Jwt));
            Services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
                AddJwtBearer(
                optins =>
                {
                    optins.SaveToken = true;
                    optins.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidAudience = jwt!.Audience,
                        ValidIssuer = jwt.Issure,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    };
                });

            return Services;
        }

        private static IServiceCollection AddSwaggerGenDependecies(this IServiceCollection Services)
        {


            Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            return Services;
        }


        private static IServiceCollection AddRateLimitingConfig(this IServiceCollection services)
        {
            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                rateLimiterOptions.AddPolicy(RateLimiters.IpLimiter, httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 200,
                            Window = TimeSpan.FromSeconds(20),
                             QueueLimit =  10
                        }
                )
                );

                rateLimiterOptions.AddPolicy(RateLimiters.UserLimiter, httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.GetUserId(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 2,
                            Window = TimeSpan.FromSeconds(20)
                        }
                )
                );

                rateLimiterOptions.AddConcurrencyLimiter(RateLimiters.Concurrency, options =>
                {
                    options.PermitLimit = 1000;
                    options.QueueLimit = 100;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });

              
            });

            return services;
        }



    }
}
