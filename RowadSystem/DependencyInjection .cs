using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using RowadSystem.API.Report;
using RowadSystem.API.Services;
using RowadSystem.API.Settings;
using RowadSystem.Authentication;
using RowadSystem.Middleware;
using RowadSystem.Settings;
using Serilog;
using System.Reflection;


namespace RowadSystem;

public static class DependencyInjection
{

    public static IServiceCollection Dependencies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
        services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
        services.Configure<CloudinarySettings>(builder.Configuration.GetSection(nameof(CloudinarySettings)));
        services.Configure<PaymobOptions>(builder.Configuration.GetSection(nameof(PaymobOptions)));
        builder.Services.AddHttpClient();
      

        services.DbContextConfig(builder)
                .MapsterConfig()
                .FluentValidationConfig()
                .AuthConfig(builder);

        services.AddProblemDetails()
               .AddExceptionHandler<GlobalExceptionHandler>();
        services.AddSerilog();
        builder.Host.UseSerilog((context, configration) =>
        configration.ReadFrom.Configuration(context.Configuration)
        );


        //services.AddSignalR();
        //services.AddScoped<NotificationService>();
        //services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
        // خدمات SignalR
        builder.Services.AddSignalR();
        builder.Services.AddScoped< NotificationService>();

        // تسجيل CustomUserIdProvider
        //builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        services.AddScoped<IAuthService, AuthService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<ISendOTPService, SendOTPService>()
                .AddScoped<IJwtProvider, JwtProvider>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IBarcodeService, BarcodeService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IImageService, ImageService>()
                .AddScoped<ISupplierService, SupplierService>()
                .AddScoped<ICategoryService, CategoryService>()
                .AddScoped<IBrandService, BrandService>()
                .AddScoped<IDiscountService, DiscountService>()
                .AddScoped<ICustomerService, CustomerService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IInvoicesService, InvoicesService>()
                .AddScoped<ICouponService, CouponService>()
                .AddScoped<IShoppingCartService, ShoppingCartService>()
                .AddScoped<IPaymobService, PaymobService>()
                .AddScoped<IPaymentService, PaymentService>()
                .AddScoped<IUnitService, UnitService>()
                .AddScoped<IAddressService, AddressService>()
                .AddSingleton<IPdfInvoiceService, PdfInvoiceService>()
                .AddScoped<IReportService, ReportService>()
                .AddScoped<IExpenseService, ExpenseService>()
                .AddScoped<IExpenseCategoryService, ExpenseCategoryService>()
                .AddScoped<IAccountStatementService, AccountStatementService>()
                .AddScoped<ICashierHandoverService, CashierHandoverService>()
                .AddScoped<IInventoryReportService, InventoryReportService>()
                .AddScoped<IPurchaseReportService, PurchaseReportService>()
                .AddScoped<ISalesReporService, SalesReportService>()
                .AddScoped<ISalesReturnsReportService, SalesReturnsReportService>()
                .AddScoped<IPurchaseReturnsReportService, PurchaseReturnsReportService>()
                .AddScoped<IProfitLossReportService, ProfitLossReportService>()
                .AddScoped<ISalesByEmployeeReport, SalesByEmployeeReport>()
                .AddScoped<ISalesByCategoryReport, SalesByCategoryReport>()
                .AddScoped<ICustomerPaymentsReportService, CustomerPaymentsReportService>()
                .AddScoped<IExcelService, ExcelService>()

        ;

        // Register services
        //builder.Services.AddSingleton<PdfInvoiceService>();

        // Configure QuestPDF license
        QuestPDF.Settings.License = LicenseType.Community;

        //services.AddHttpClient<IPaymobService, PaymobService>();

        return services;
    }
    public static IServiceCollection DbContextConfig(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


        return services;
    }
    public static IServiceCollection MapsterConfig(this IServiceCollection services)
    {
        services.AddMapster();

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());


        return services;
    }
    public static IServiceCollection FluentValidationConfig(this IServiceCollection services)
    {

        //services
        // .AddFluentValidationAutoValidation()
        // .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services
       .AddFluentValidationAutoValidation()
       .AddFluentValidationClientsideAdapters()
       .AddValidatorsFromAssembly(typeof(LoginRequestValidator).Assembly);

        return services;
    }
    public static IServiceCollection AuthConfig(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            };
            // ✨ عشان SignalR يعرف يقرا التوكن من الـ query
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/notificationHub"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });


        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 6;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });


        return services;
    }
}
