
using Blazored.LocalStorage;
using Blazored.Modal;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using RowadSystem.Shard.Contract.Auth;
using RowadSystem.UI;
using RowadSystem.UI.Features;
using RowadSystem.UI.Features.Account;
using RowadSystem.UI.Features.AccountStatements;
using RowadSystem.UI.Features.Addresses;
using RowadSystem.UI.Features.Auth;
using RowadSystem.UI.Features.CashierHandover;
using RowadSystem.UI.Features.Categories;
using RowadSystem.UI.Features.Customers;
using RowadSystem.UI.Features.Expenses;
using RowadSystem.UI.Features.ExpensesCategory;
using RowadSystem.UI.Features.Invoices;
using RowadSystem.UI.Features.Payments;
using RowadSystem.UI.Features.Products;
using RowadSystem.UI.Features.Reports;
using RowadSystem.UI.Features.Roles;
using RowadSystem.UI.Features.ShoppingCart;
using RowadSystem.UI.Features.Suppliers;
using RowadSystem.UI.Features.Users;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// LocalStorage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredModal();
// Add Radzen services
builder.Services.AddRadzenComponents();
// In Program.cs or Startup.cs
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();


builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ContextMenuService>();





// Services
builder.Services
    .AddScoped<AuthenticatedHttpMessageHandler>()
    .AddScoped<IProductService, ProductService>()
    .AddScoped<ITokenStorageService, TokenStorageService>()
    .AddScoped<IInvoiceService, InvoiceService>()
    .AddScoped<ICartService, CartService>()
    .AddScoped<ISupplierService, SupplierService>()
    .AddScoped<ICustomerService, CustomerService>()
    .AddScoped<IAddressService, AddressService>()
    .AddScoped<IProfileService, ProfileService>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<IReportService, ReportService>()
    .AddScoped<ICategoryService, CategoryService>()
    .AddScoped<IExpenseService, ExpenseService>()
    .AddScoped<IExpenseCategoryService, ExpenseCategoryService>()
    .AddScoped<IAccountStatementService, AccountStatementService>()
    .AddScoped<ICashierHandoverService, CashierHandoverService>()
    .AddScoped<IRoleService, RoleService>()
    .AddScoped<IPaymentService, PaymentService>()
    .AddScoped<IOrderService, OrderService>()
    .AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<AlertService>();

builder.Services.AddValidatorsFromAssembly(typeof(LoginRequestValidator).Assembly);
builder.Services
    .AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();



AppContext.SetSwitch("System.Diagnostics.Activity.DisableActivitySource", true);

// ✅ Client بدون توكن (NoAuth)
builder.Services.AddHttpClient("NoAuth", client =>
{
    client.BaseAddress = new Uri("http://localhost:5190");
});

// ✅ Client مع توكن (AuthorizedClient)
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5190"); 
}).AddHttpMessageHandler<AuthenticatedHttpMessageHandler>();


await builder.Build().RunAsync();
