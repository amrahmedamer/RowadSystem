using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RowadSystem.API.Entity;
using System.Reflection;

namespace RowadSystem.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<OTP> OTP { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<ProductUnit> ProductUnits { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<ContactNumber> contactNumbers { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
    public DbSet<SalesInvoice> SalesInvoices { get; set; }
    public DbSet<PurchaseReturnInvoice> PurchaseReturns { get; set; }
    public DbSet<SalesReturnInvoice> SalesReturnInvoices { get; set; }
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
    public DbSet<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }
    public DbSet<SalesInvoiceDetail> SalesInvoiceDetails { get; set; }
    public DbSet<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
    public DbSet<SalesReturnInvoiceDetail> SalesReturnDetails { get; set; }


    public DbSet<Governorate> Governorates { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Street> Streets { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseCategory>  ExpenseCategories { get; set; }
    public DbSet<Notification>  Notifications { get; set; }
    public DbSet<CashierHandover>  CashierHandovers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Invoice>().ToTable("Invoices");
        builder.Entity<PurchaseInvoice>().ToTable("PurchaseInvoices");
        builder.Entity<SalesInvoice>().ToTable("SalesInvoices");
        builder.Entity<PurchaseReturnInvoice>().ToTable("PurchaseReturns");
        builder.Entity<SalesReturnInvoice>().ToTable("SalesReturns");

        builder.Entity<InvoiceDetail>().ToTable("InvoiceDetails");
        builder.Entity<PurchaseInvoiceDetail>().ToTable("PurchaseInvoiceDetails");
        builder.Entity<SalesInvoiceDetail>().ToTable("SalesInvoiceDetails");
        builder.Entity<PurchaseReturnDetail>().ToTable("PurchaseReturnDetails");
        builder.Entity<SalesReturnInvoiceDetail>().ToTable("SalesReturnDetails");


     
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            foreach (var foreignKey in entity.GetForeignKeys())
            {
                if (foreignKey.DeleteBehavior == DeleteBehavior.Cascade)
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
        }

        builder.Entity<ProductImage>()
         .HasOne(pi => pi.Product)
         .WithMany(p => p.Images)
         .HasForeignKey(pi => pi.ProductId)
         .OnDelete(DeleteBehavior.Cascade);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
